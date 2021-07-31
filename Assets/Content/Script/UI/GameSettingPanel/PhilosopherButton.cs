using System.Collections;
using System.Collections.Generic;
using TMPro;
using UltimateClean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhilosopherButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI textMesh;
    [SerializeField]
    private TextMeshProUGUI numberText;
    [SerializeField]
    private CleanButton buttonRef;
    public string categoryName { set => textMesh.text = value; get => textMesh.text; }
    public string number { set => numberText.text = value; get => numberText.text; }

    private void Start()
    {
        buttonRef.onClick.AddListener(HandleGameStart);
    }

    private void HandleGameStart()
    {
        UIManager.Ins.PushPanel("InGamePanel");
        var inGamePanelRef = UIManager.Ins.GetPanel("InGamePanel") as InGamePanel;
        if (inGamePanelRef)
            inGamePanelRef.OnGameStart(categoryName);
    }
}
