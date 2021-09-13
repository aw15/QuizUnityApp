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

        RefreshPanel();
    }

    private void RefreshPanel()
    {
        Clear();
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject newObj;
        var bookMarkData = BookmarkComponent.GetBookmarkData();
        foreach (var data in bookMarkData)
        {
            newObj = (GameObject)Instantiate(bookmarkWidgetRef, content.transform);
            var scriptRef = newObj.GetComponentInChildren<BookMarkWidget>();
            scriptRef.Initialized(data);
            scriptRef.OnNeedRefreshPanel.AddListener(RefreshPanel);
        }

        emptyEmphasisObject.SetActive(bookMarkData.Count == 0);
    }

    public override string GetPanelName()
    {
        return "BookmarkPanel";
    }
}
