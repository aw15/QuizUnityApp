using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase.Database;
using Firebase.Auth;
using System.IO;
using Firebase;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using SysTask = System.Threading.Tasks;

public class DataManager : MonoBehaviour
{
    [Serializable]
    public class QuizData
    {
        public string quiz;
        public bool isAnswer;
        public string description;
        public string category;
        public string source;
    }

    public class QuizDatabase
    {
        public Dictionary<string, List<QuizData>> data = new Dictionary<string, List<QuizData>>();
    }

    [Serializable]
    public class QuizDataForJson //! Json 저장을 위한 포맷
    {
        public string key = string.Empty;
        public List<QuizData> quizList = new List<QuizData>();
    }
    public class QuizDataListForJson
    {
        public List<QuizDataForJson> data = new List<QuizDataForJson>();
    }

    [Serializable]
    public class PhilosopherList
    {
        public List<string> data = new List<string>();
    }


    private const int QUIZDATA_ELEMENTCOUNT = 5;
    static DataManager instance;
    FirebaseDatabase db;
    FirebaseAuth auth;
    private static string DB_VERSION_KEY = "DatabaseVersion";
    private static string localQuizDataFile = "Quiz.json";
    private static string localPhilosopherDataFile = "Philosopher.json";
    [NonSerialized]
    public PhilosopherList philosopherList = new PhilosopherList();
    public QuizDatabase quizDatabase = new QuizDatabase();
    public UnityEvent OnDatabaseLoaded = new UnityEvent();

    public bool isContainAlreadySolved { get; set; }

    public static DataManager Ins
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(DataManager)) as DataManager;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        
    }
    void Start()
    {
        philosopherList.data.Sort();
    }
    public static void Save<T>(string name, T instance)
    {
        using (var ms = new MemoryStream())
        {
            new BinaryFormatter().Serialize(ms, instance);
            try
            {
                PlayerPrefs.SetString(name, System.Convert.ToBase64String(ms.ToArray()));
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    public static T Load<T>(string name) where T : new()
    {
        if (!PlayerPrefs.HasKey(name)) return default(T);
        byte[] bytes = System.Convert.FromBase64String(PlayerPrefs.GetString(name));
        using (var ms = new MemoryStream(bytes))
        {
            object obj = new BinaryFormatter().Deserialize(ms);
            return (T)obj;
        }
    }

    public static void SaveJson<T>(string name , T instance)
    {
        try
        {
            string json = JsonUtility.ToJson(instance);
            FileLib.writeStringToFile(json, name);
        }
        catch(Exception ex)
        {
            Logger.Error(ex);
        }

    }

    public static T LoadJson<T>(string name) where T : new()
    {
        string json = FileLib.readStringFromFile(name);
        T myObject = JsonUtility.FromJson<T>(json);
        return myObject;
    }
    public async void DatabaseInit()
    {
        try
        {
            // Get the root reference location of the database.
            db = FirebaseDatabase.GetInstance(Firebase.FirebaseApp.DefaultInstance, @"https://soborobreadstudio-default-rtdb.asia-southeast1.firebasedatabase.app/");
            if (db == null)
            {
                UIManager.Ins.ShowNotification("DB 서버 접속 실패", "조금 뒤에 다시 시도해주세요.", Notification.InfoType.Error);
                return;
            }

            //DB 버전 체크
            var versionSnapshot = await db.GetReference("Version").GetValueAsync();
            var dbVersion = PlayerPrefs.GetString(DB_VERSION_KEY, string.Empty);
            var remoteDbVersion = versionSnapshot.Value as string;
            Debug.Log($"db version {remoteDbVersion} {dbVersion}");

            DataSnapshot philosophersSnapshot = null;
            bool philosopherReadFail = false;
            bool quizReadFail = false;
            //LocalFile에 데이터가 있고
            //버전이 일치할 때
            if (remoteDbVersion != null && dbVersion.Equals(remoteDbVersion))
            {
                philosopherList.data.Clear();
                philosopherList = LoadJson<PhilosopherList>(localPhilosopherDataFile);
                if (philosopherList == null || philosopherList.data.Count <= 0)
                {
                    philosopherReadFail = true;
                }

                quizDatabase.data.Clear();
                QuizDataListForJson parsingQuizDataList = LoadJson<QuizDataListForJson>(localQuizDataFile);
                if (parsingQuizDataList != null && parsingQuizDataList.data.Count > 0)
                {
                    Debug.Log("Data From localFile");
                    foreach (var data in parsingQuizDataList.data)
                    {
                        quizDatabase.data.Add(data.key, data.quizList);
                    }
                }
                else
                {
                    quizReadFail = true;
                }
            }
            //db에서 철학자 리스트 읽어옴.
            if (philosopherReadFail)
            {
                philosophersSnapshot = await db.GetReference("Philosophers").GetValueAsync();
                if (philosophersSnapshot != null)
                {
                    Debug.Log($"{philosophersSnapshot.Value.GetType()}");
                    var list = philosophersSnapshot.GetValue(true) as List<object>;

                    philosopherList = new PhilosopherList();
                    foreach (var philosopher in list)
                    {
                        philosopherList.data.Add(philosopher as string);
                        Debug.Log(philosopher as string);
                    }
                }
                else
                {
                    UIManager.Ins.ShowNotification("DB 서버 접속 실패", "조금 뒤에 다시 시도해주세요.", Notification.InfoType.Error);
                    return;
                }

                SaveJson<PhilosopherList>(localPhilosopherDataFile, philosopherList);
            }
            //db에서 퀴즈 데이터 읽어옴.
            if (quizReadFail)
            {
                var quizSnapshot = await db.GetReference("QuizData").GetValueAsync();
                if (quizSnapshot == null)
                {
                    UIManager.Ins.ShowNotification("DB 서버 접속 실패", "조금 뒤에 다시 시도해주세요.", Notification.InfoType.Error);
                    return;
                }

                var items = quizSnapshot.Value as List<object>;
                if (items == null)
                {
                    UIManager.Ins.ShowNotification("DB 서버 접속 실패", "조금 뒤에 다시 시도해주세요.", Notification.InfoType.Error);
                    return;
                }

                quizDatabase = new QuizDatabase();
                foreach (var item in items)
                {
                    var innerItems = item as List<object>;
                    if (innerItems == null)
                    {
                        UIManager.Ins.ShowNotification("DB 서버 접속 실패", "조금 뒤에 다시 시도해주세요.", Notification.InfoType.Error);
                        return;
                    }

                    QuizData quizData = new QuizData();
                    if (innerItems.Count == QUIZDATA_ELEMENTCOUNT)
                    {
                        quizData.quiz = innerItems[0] as string;
                        quizData.isAnswer = (bool)innerItems[1];
                        quizData.description = innerItems[2] as string;
                        quizData.category = innerItems[3] as string;
                        quizData.source = innerItems[4] as string;
                    }

                    if (quizDatabase.data.ContainsKey(quizData.category) == false)
                    {
                        quizDatabase.data.Add(quizData.category, new List<QuizData>());
                    }
                    quizDatabase.data[quizData.category].Add(quizData);
                }

                QuizDataListForJson quizDataForJson = new QuizDataListForJson();
                foreach (var quizData in quizDatabase.data)
                {
                    QuizDataForJson temp = new QuizDataForJson();
                    temp.key = quizData.Key;
                    temp.quizList = quizData.Value;
                    quizDataForJson.data.Add(temp);
                }
                SaveJson<QuizDataListForJson>(localQuizDataFile, quizDataForJson);
            }
            OnDatabaseLoaded.Invoke();

            //현재 버전 정보 저장.
            PlayerPrefs.SetString(DB_VERSION_KEY, remoteDbVersion);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }
}
