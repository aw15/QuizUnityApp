using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BookmarkComponent
{
    public class QuizDataEqualityComparer : IEqualityComparer<DataManager.QuizData>
    {
        public bool Equals(DataManager.QuizData x, DataManager.QuizData y)
        {
            return x.quiz == y.quiz;
        }

        public int GetHashCode(DataManager.QuizData obj)
        {
            return obj.quiz.ToString().GetHashCode();
        }
    }

    [Serializable]
    class BookmarkJsonData
    {
        public List<DataManager.QuizData> data = new List<DataManager.QuizData>();
    }

    static BookmarkJsonData bookmarkJsonData; //LoadOnStart에서 할당
    static HashSet<DataManager.QuizData> bookmarkData = new HashSet<DataManager.QuizData>(new QuizDataEqualityComparer());
    static readonly string bookmarkSaveFile = "bookmark.json";
    public static void LoadOnStart()
    {
        LoadBookmark();
        foreach (var data in bookmarkJsonData.data)
        {
            bookmarkData.Add(data);
        }
    }
    public static HashSet<DataManager.QuizData> GetBookmarkData()
    {
        return bookmarkData;
    }
    public static bool IsContainBookmark(DataManager.QuizData quizData)
    {
        return bookmarkData.Contains(quizData);
    }
    public static bool AddBookmark(DataManager.QuizData quizData)
    {
        if(false == bookmarkData.Contains(quizData))
        { 
            bookmarkData.Add(quizData);
            bookmarkJsonData.data.Add(quizData);
            SaveBookmark();
            return true;
        }
        return false;
    }
    public static bool RemoveBookmark(DataManager.QuizData quizData)
    {
        if (bookmarkData.Contains(quizData))
        {
            bookmarkData.Remove(quizData);
            bookmarkJsonData.data.Remove(quizData);
            SaveBookmark();
            return true;
        }
        return false;
    }
    public static void SaveBookmark()
    {
        DataManager.SaveJson<BookmarkJsonData>(bookmarkSaveFile, bookmarkJsonData);
    }

    public static void LoadBookmark()
    {
        bookmarkJsonData = DataManager.LoadJson<BookmarkJsonData>(bookmarkSaveFile);
        if (bookmarkJsonData == null)
            bookmarkJsonData = new BookmarkJsonData();
    }
}
