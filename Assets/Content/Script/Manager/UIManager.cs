using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Notification
{
    public enum InfoType
    {
        Error = 0,
        Success = 1,
        Normal = 2
    }
}

public abstract class IPanel : MonoBehaviour //Awake override 시 반드시 부모의 Awake도 호출할 것.
{
    public GameObject panelRef;
    public bool isPopup = false;
    protected virtual void Awake()
    {
        UIManager.Ins.AddPanel(GetPanelName(), this);
    }
    public virtual void Show()
    {
        panelRef.SetActive(true);
    }
    public virtual void Hide()
    {
        panelRef.SetActive(false);
    }
    protected void Delay(int second)
    {
        StartCoroutine(Wait(second));
    }
    IEnumerator Wait(int second)
    {
        yield return new WaitForSeconds(second);
    }
    public abstract void OnBackEvent();
    public abstract string GetPanelName();
}

public class PanelStack
{
    private Stack<IPanel> panels = new Stack<IPanel>();
    public void Push(IPanel panel)
    {
        if (panel != null)
        {
            if (panels.Count > 0 &&  !panel.isPopup)
            {
                panels.Peek().Hide();
            }
            panel.Show();
            panels.Push(panel);
        }
    }

    public void Pop()
    {
        if (panels.Count > 0)
        {
            panels.Pop().Hide();
        }

        if(panels.Count > 0)
        {
            panels.Peek().Show();
        }
    }
    public IPanel Top()
    {
        if (panels.Count > 0)
        {
            return panels.Peek();
        }
        else
        {
            return null;
        }
    }
}



public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Ins
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(UIManager)) as UIManager;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
[SerializeField]
    private List<GameObject> notificationPrefs;
    [SerializeField]
    private UltimateClean.NotificationLauncher notificationLauncher;
    public PanelStack panelStack = new PanelStack();
    private Dictionary<string, IPanel> panelList; 

    void Awake() 
    {
        Ins = this;
        DontDestroyOnLoad(gameObject);
    }
    public void ShowNotification(string title, string text, Notification.InfoType infoType = Notification.InfoType.Normal)
    {
        Debug.Log(title);
        Debug.Log(text);
        notificationLauncher.Title = title;
        notificationLauncher.Message = text;
        notificationLauncher.Prefab = notificationPrefs[(int)infoType];
        notificationLauncher.LaunchNotification();
    }
    public IPanel GetPanel(string key)
    {
        if (UIManager.Ins.panelList.ContainsKey(key))
            return UIManager.Ins.panelList[key]; 
        else
            return null;
    }
    public IPanel GetCurrentPanel()
    {
        return panelStack.Top();
    }
    public void AddPanel(string key, IPanel panel)
    {
        if (UIManager.Ins.panelList == null)
            UIManager.Ins.panelList = new Dictionary<string, IPanel>();

        panelList[key] =  panel; 
    }

    public void PushPanel(string key)
    {
        if (UIManager.Ins.panelList == null)
            UIManager.Ins.panelList = new Dictionary<string, IPanel>();

        if (UIManager.Ins.panelList.ContainsKey(key))
            panelStack.Push(UIManager.Ins.panelList[key]);
    }

    public void PopPanel()
    {
        panelStack.Pop();
    }
}
