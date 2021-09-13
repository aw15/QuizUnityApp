using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfilePanel : IPanel
{
    [SerializeField]
    TextMeshProUGUI playTimeText;
    [SerializeField]
    TextMeshProUGUI answerRateText;
    [SerializeField]
    TextMeshProUGUI totalQuizCountText;

    const string playTimeTitle = "ÃÑ ÇÃ·¹ÀÌ ½Ã°£ : ";
    const string answerRateTitle = "Á¤´ä·ü : ";
    const string totalQuizCountTitle = "ÃÑ Ç¬ ¹®Á¦¼ö : ";
    public override string GetPanelName()
    {
        return "ProfilePanel";
    }

    public override void OnBackEvent()
    {
        UIManager.Ins.PopPanel();
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }
    public override void Show()
    {
        base.Show();
        playTimeText.text = playTimeTitle + MyPlayDataComponent.playTimeString;
        answerRateText.text = answerRateTitle + MyPlayDataComponent.answerRateString;
        totalQuizCountText.text = totalQuizCountTitle + $"{MyPlayDataComponent.totalAnswerCount} °³";
    }
}
