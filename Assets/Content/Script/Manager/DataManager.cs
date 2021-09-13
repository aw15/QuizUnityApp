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
        public string quiz = string.Empty;
        public bool isAnswer = false;
        public string description = string.Empty;
        public string category = string.Empty;
    }

    public class QuizDatabase
    {
        public Dictionary<string, List<QuizData>> data = new Dictionary<string, List<QuizData>>();
    }

    [Serializable]
    public class QuizDataFromJson
    {
        public List<QuizData> Data = new List<QuizData>();
    }

    static DataManager instance;
    private static string localQuizDataFile = $@"Data\QuizData"; //resource 하위에 위치
    public QuizDataFromJson quizDatabase = new QuizDataFromJson();
    public UnityEvent OnDatabaseLoaded = new UnityEvent();
    public UnityEvent OnDatabaseLoadFailed = new UnityEvent();
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
        BookmarkComponent.LoadOnStart();
        BestScoreComponent.LoadOnStart();
        MyPlayDataComponent.LoadOnStart();
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
            Debug.Log($"save Json {name}");
            string json = JsonUtility.ToJson(instance);
            FileLib.writeStringToFile(json, name);
        }
        catch(Exception ex)
        {
            Debug.LogError($"save json fail : {ex}");
        }

    }
    public static T LoadJsonFromString<T>(string jsonData) where T : new()
    {
        Debug.Log($"Load Json from string");
        T myObject = JsonUtility.FromJson<T>(jsonData);
        return myObject;
    }
    public static T LoadJson<T>(string name) where T : new()
    {
        Debug.Log($"Load Json {name}");
        string json = FileLib.readStringFromFile(name);
        T myObject = JsonUtility.FromJson<T>(json);
        return myObject;
    }
    public void DatabaseInit()
    {
        try
        {
            if (Application.isEditor)
            {
                quizDatabase = LoadJson<QuizDataFromJson>(Path.Combine(Application.dataPath, "Resources", @"Data\QuizData.txt"));
            }
            else
            {
                Debug.Log($"Load {localQuizDataFile}...");
                var textData = Resources.Load(localQuizDataFile) as TextAsset;
                quizDatabase = LoadJsonFromString<QuizDataFromJson>(textData.ToString());
            }
            OnDatabaseLoaded.Invoke();
            //// Get the root reference location of the database.
            //db = FirebaseDatabase.GetInstance(Firebase.FirebaseApp.DefaultInstance, @"https://soborobreadstudio-default-rtdb.asia-southeast1.firebasedatabase.app/");
            //if (db == null)
            //{
            //    UIManager.Ins.ShowNotification("DB 서버 접속 실패", "조금 뒤에 다시 시도해주세요.", Notification.InfoType.Error);
            //    return;
            //}

            ////DB 버전 체크
            //var versionSnapshot = await db.GetReference("Version").GetValueAsync();
            //var dbVersion = PlayerPrefs.GetString(DB_VERSION_KEY, string.Empty);
            //var remoteDbVersion = versionSnapshot.Value as string;
            //Debug.Log($"db version {remoteDbVersion} {dbVersion}");

            //DataSnapshot philosophersSnapshot = null;
            //bool philosopherReadFail = false;
            //bool quizReadFail = false;
            ////LocalFile에 데이터가 있고
            ////버전이 일치할 때
            //if (remoteDbVersion != null && dbVersion.Equals(remoteDbVersion))
            //{
            //    philosopherList.data.Clear();
            //    philosopherList = LoadJson<PhilosopherList>(localPhilosopherDataFile);
            //    if (philosopherList == null || philosopherList.data.Count <= 0)
            //    {
            //        philosopherReadFail = true;
            //    }

            //    quizDatabase.data.Clear();
            //    QuizDataListForJson parsingQuizDataList = LoadJson<QuizDataListForJson>(localQuizDataFile);
            //    if (parsingQuizDataList != null && parsingQuizDataList.data.Count > 0)
            //    {
            //        Debug.Log("Data From localFile");
            //        foreach (var data in parsingQuizDataList.data)
            //        {
            //            quizDatabase.data.Add(data.key, data.quizList);
            //        }
            //    }
            //    else
            //    {
            //        quizReadFail = true;
            //    }
            //}
            ////db에서 철학자 리스트 읽어옴.
            //if (philosopherReadFail)
            //{
            //    Debug.Log("Load philosopher data on db");
            //    philosophersSnapshot = await db.GetReference("Philosophers").GetValueAsync();
            //    if (philosophersSnapshot != null)
            //    {
            //        var list = philosophersSnapshot.GetValue(true) as List<object>;

            //        philosopherList = new PhilosopherList();
            //        foreach (var philosopher in list)
            //        {
            //            philosopherList.data.Add(philosopher as string);
            //            Debug.Log(philosopher as string);
            //        }
            //    }
            //    else
            //    {
            //        UIManager.Ins.ShowNotification("DB 서버 접속 실패", "조금 뒤에 다시 시도해주세요.", Notification.InfoType.Error);
            //        return;
            //    }

            //    SaveJson<PhilosopherList>(localPhilosopherDataFile, philosopherList);
            //}
            ////db에서 퀴즈 데이터 읽어옴.
            //if (quizReadFail)
            //{
            //    Debug.Log("Load quiz data on db");
            //    var quizSnapshot = await db.GetReference("QuizData").GetValueAsync();
            //    if (quizSnapshot == null)
            //    {
            //        UIManager.Ins.ShowNotification("DB 서버 접속 실패", "조금 뒤에 다시 시도해주세요.", Notification.InfoType.Error);
            //        return;
            //    }

            //    var items = quizSnapshot.Value as List<object>;
            //    if (items == null)
            //    {
            //        UIManager.Ins.ShowNotification("DB 서버 접속 실패", "조금 뒤에 다시 시도해주세요.", Notification.InfoType.Error);
            //        return;
            //    }

            //    quizDatabase = new QuizDatabase();
            //    foreach (var item in items)
            //    {
            //        var innerItems = item as List<object>;
            //        if (innerItems == null)
            //        {
            //            UIManager.Ins.ShowNotification("DB 서버 접속 실패", "조금 뒤에 다시 시도해주세요.", Notification.InfoType.Error);
            //            return;
            //        }

            //        QuizData quizData = new QuizData();
            //        if (innerItems.Count == QUIZDATA_ELEMENTCOUNT)
            //        {
            //            quizData.quiz = innerItems[0] as string;
            //            quizData.isAnswer = (bool)innerItems[1];
            //            quizData.description = innerItems[2] as string;
            //            quizData.category = innerItems[3] as string;
            //        }

            //        if (quizDatabase.data.ContainsKey(quizData.category) == false)
            //        {
            //            quizDatabase.data.Add(quizData.category, new List<QuizData>());
            //        }
            //        quizDatabase.data[quizData.category].Add(quizData);
            //    }

            //    QuizDataListForJson quizDataForJson = new QuizDataListForJson();
            //    foreach (var quizData in quizDatabase.data)
            //    {
            //        QuizDataForJson temp = new QuizDataForJson();
            //        temp.key = quizData.Key;
            //        temp.quizList = quizData.Value;
            //        quizDataForJson.data.Add(temp);
            //    }
            //    SaveJson<QuizDataListForJson>(localQuizDataFile, quizDataForJson);
            //}
            //OnDatabaseLoaded.Invoke();

            ////현재 버전 정보 저장.
            //PlayerPrefs.SetString(DB_VERSION_KEY, remoteDbVersion);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }
}
