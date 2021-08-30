using System.Collections;
using System.Collections.Generic;
using UltimateClean;
using UnityEngine;
using UnityEngine.UI;
using static OptionManager;

public class SettingPanel : IPanel
{
    [SerializeField]
    CleanButton effectSoundBtn;
    [SerializeField]
    Image effectSoundBtnImage;
    [SerializeField]
    CleanButton buttonSoundBtn;
    [SerializeField]
    Image buttonSoundBtnImage;
    [SerializeField]
    CleanButton saveButton;
    [SerializeField]
    CleanButton backBtn;
    [SerializeField]
    UltimateClean.CleanButton copyrightBtn;

    private Dictionary<EFlagOption, bool> option;

    // Start is called before the first frame update
    void Start()
    {
        isPopup = true;
        Hide();
        effectSoundBtn.onClick.AddListener(OnEffectSoundBtnClicked);
        buttonSoundBtn.onClick.AddListener(OnButtonSoundBtnClicked);
        saveButton.onClick.AddListener(OnSaveOptionBtnClick);
        backBtn.onClick.AddListener(OnBackEvent);
        copyrightBtn.onClick.AddListener(OnCopyrightBtnClicked);
    }
    private void OnDestroy()
    {
        effectSoundBtn.onClick.RemoveListener(OnEffectSoundBtnClicked);
        buttonSoundBtn.onClick.RemoveListener(OnButtonSoundBtnClicked);
        saveButton.onClick.RemoveListener(OnSaveOptionBtnClick);
        backBtn.onClick.RemoveListener(OnBackEvent);
        copyrightBtn.onClick.RemoveListener(OnCopyrightBtnClicked);
    }
    public override void Show() //¡¯¿‘¡°
    {
        base.Show();

        option = OptionManager.GetEntireOptionCopy();
        SetImageAlpha(effectSoundBtnImage, option[EFlagOption.EffectSound]);
        SetImageAlpha(buttonSoundBtnImage, option[EFlagOption.ButtonSound]);
    }
    public void OnCopyrightBtnClicked()
    {
        UIManager.Ins.PushPanel("CopyrightPanel");
    }
    void SetImageAlpha(Image imageRef, bool isOn)
    {
        Color color = imageRef.color;
        color.a = isOn ? 1.0f : 0.4f;
        imageRef.color = color;
    }
    void OnEffectSoundBtnClicked()
    {
        option[EFlagOption.EffectSound] = !option[EFlagOption.EffectSound];
        SetImageAlpha(effectSoundBtnImage, option[EFlagOption.EffectSound]);
    }
    void OnButtonSoundBtnClicked()
    {
        option[EFlagOption.ButtonSound] = !option[EFlagOption.ButtonSound];
        SetImageAlpha(buttonSoundBtnImage, option[EFlagOption.ButtonSound]);
    }

    void OnSaveOptionBtnClick()
    {
        OptionManager.SetEntireOption(option);
        UIManager.Ins.PopPanel();
    }

    public override void OnBackEvent()
    {
        UIManager.Ins.PopPanel();
    }

    public override string GetPanelName()
    {
        return "SettingPanel";
    }
}
