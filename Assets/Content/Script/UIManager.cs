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

public abstract class IPanel : MonoBehaviour
{
    public GameObject panelRef;
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
}

public class PanelStack
{
    private Stack<IPanel> panels = new Stack<IPanel>();
    public void Push(IPanel panel)
    {
        if (panel != null)
        {
            if (panels.Count > 0)
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
    public static PanelStack panelStack = new PanelStack();
    private static Dictionary<string, IPanel> panelList = new Dictionary<string, IPanel>(); 

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
        if (UIManager.panelList.ContainsKey(key))
            return UIManager.panelList[key]; 
        else
            return null;
    }
    public void AddPanel(string key, IPanel panel)
    {
        if (UIManager.panelList == null)
            UIManager.panelList = new Dictionary<string, IPanel>();

        panelList[key] =  panel; 
    }

    public void PushPanel(string key)
    {
        if (UIManager.panelList == null)
            UIManager.panelList = new Dictionary<string, IPanel>();

        if (UIManager.panelList.ContainsKey(key))
            panelStack.Push(UIManager.panelList[key]);
    }

    public void PopPanel()
    {
        panelStack.Pop();
    }
}
