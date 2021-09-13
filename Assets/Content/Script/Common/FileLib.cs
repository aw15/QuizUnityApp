using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Logger
{
    string fileName = Path.Combine(Application.persistentDataPath, "Log", "quiz.log");

    public static void Error(Exception ex)
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        var frame = stackTrace.GetFrame(1);
        Debug.LogError($"Message --- {ex.Message}\n" +
            $"source --- {ex.Source}\n" +
            $"StackTrace --- {frame.GetMethod()}\n" +
            $"TargetSite --- {ex.TargetSite}\n");
    }
}

public class FileLib
{

public static void writeStringToFile(string str, string filename)
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);
        Debug.Log($"[writeStringToFile] {path}");
        FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);

        StreamWriter sw = new StreamWriter(file);
        sw.WriteLine(str);

        sw.Close();
        file.Close();
#endif
    }
    public static string readStringFromFile(string filename)//, int lineIndex )
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);

        if (File.Exists(path))
        {
            Debug.Log($"[readStringFromFile] {path} exist");
            FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            string str = null;
            str = sr.ReadToEnd();

            sr.Close();
            file.Close();

            return str;
        }
        else
        {
            return null;
        }
#else
     return null;
#endif
    }

    public static string pathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }
}

