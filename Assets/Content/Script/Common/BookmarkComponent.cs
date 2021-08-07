using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookmarkComponent
{
    [Serializable]
    class BookmarkJsonData
    {
        public List<DataManager.QuizData> data = new List<DataManager.QuizData>();
    }

    static BookmarkJsonData bookmarkJsonData; //LoadOnStart���� �Ҵ�
    static HashSet<DataManager.QuizData> bookmarkData = new HashSet<DataManager.QuizData>();
    static readonly string bookmarkSaveFile = "bookmark.json";
    public void LoadOnStart()
    {
        LoadBookmark();
        foreach (var data in bookmarkJsonData.data)
        {
            Debug.Log(data.category);
            bookmarkData.Add(data);
        }
    }
    public HashSet<DataManager.QuizData> GetBookmarkData()
    {
        return bookmarkData;
    }
    public void AddBookmark(DataManager.QuizData quizData)
    {
        int prevCount = bookmarkData.Count;
        bookmarkData.Add(quizData);
        if (prevCount != bookmarkData.Count)
        {
            bookmarkJsonData.data.Add(quizData);
            SaveBookmark(); 
        }
    }
    public void RemoveBookmark(DataManager.QuizData quizData)
    {
        bookmarkData.Remove(quizData);
    }
    public void SaveBookmark()
    {
        DataManager.SaveJson<BookmarkJsonData>(bookmarkSaveFile, bookmarkJsonData);
    }

    public void LoadBookmark()
    {
        bookmarkJsonData = DataManager.LoadJson<BookmarkJsonData>(bookmarkSaveFile);
        if (bookmarkJsonData == null)
            bookmarkJsonData = new BookmarkJsonData();
    }
}
