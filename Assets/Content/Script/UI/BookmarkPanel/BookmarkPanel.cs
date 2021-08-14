using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookmarkPanel : IPanel
{
    [SerializeField]
    GameObject content;
    [SerializeField]
    GameObject bookmarkWidgetRef;
    [SerializeField]
    GameObject emptyEmphasisObject;
    BookmarkComponent bookmarkComponent = new BookmarkComponent();
    void Start()
    {
        Hide();
    }
    private void OnDestroy()
    {
    }
    void Clear()
    {

    }
    public override void OnBackEvent()
    {
        UIManager.Ins.PopPanel();
    }
    public override void Show() // ¡¯¿‘¡°
    {
        base.Show();

        Clear();
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

        emptyEmphasisObject.SetActive(bookMarkData.Count == 0);
    }

    public override string GetPanelName()
    {
        return "BookmarkPanel";
    }
}
