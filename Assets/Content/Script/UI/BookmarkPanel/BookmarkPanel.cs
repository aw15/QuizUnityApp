using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookmarkPanel : IPanel
{
    [SerializeField]
    GameObject content;
    [SerializeField]
    GameObject bookmarkWidgetRef;
    BookmarkComponent bookmarkComponent = new BookmarkComponent();
    void Start()
    {
        UIManager.Ins.AddPanel("BookmarkPanel", this);
        Hide();
    }
    void Clear()
    {

    }

    public void OnDataLoaded() // ¡¯¿‘¡°.
    {
        Clear();
        UIManager.Ins.PushPanel("BookmarkPanel");
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject newObj;
        var bookMarkData = bookmarkComponent.GetBookmarkData();
        foreach(var data in bookMarkData)
        {
            newObj = (GameObject)Instantiate(bookmarkWidgetRef, content.transform);
            var scriptRef = newObj.GetComponentInChildren<BookMarkWidget>();
            scriptRef.Initialized(data);
        }
    }
}
