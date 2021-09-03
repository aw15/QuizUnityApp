using System.Collections;
using System.Collections.Generic;
using TMPro;
using UltimateClean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StageButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI numberText;
    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    [SerializeField]
    private CleanButton buttonRef;
    public string number { set => numberText.text = value; get => numberText.text; }
    public string bestScore { set => bestScoreText.text =  $"최고점수 : {value}점"; get => bestScoreText.text; }
    public int stage = -1;
    public int startIndex { set; get; }
    public int lastIndex { set; get; }

    private void Start()
    {
        buttonRef.onClick.AddListener(HandleGameStart);
    }

    private void HandleGameStart()
    {
        UIManager.Ins.PushPanel("InGamePanel");
        var inGamePanelRef = UIManager.Ins.GetPanel("InGamePanel") as InGamePanel;
        if (inGamePanelRef)
            inGamePanelRef.OnGameStart(stage, startIndex, lastIndex);
    }
}
