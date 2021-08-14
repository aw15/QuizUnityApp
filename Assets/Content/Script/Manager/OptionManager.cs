using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public static bool buttonSoundOption { get; set; } = true;
    public static bool effectSoundOption { get; set; } = true;

    public enum EFlagOption
    {
        ButtonSound,
        EffectSound
    }

    static Dictionary<EFlagOption, bool> option = new Dictionary<EFlagOption, bool>();
    public static AudioSource effectSoundAudioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        effectSoundAudioSource = GetComponentInParent<AudioSource>();
        LoadOption();
    }

    public static void OptionChange(EFlagOption optionEnum, bool value)
    {
        switch (optionEnum)
        {
            case EFlagOption.EffectSound:
                {
                    option[optionEnum] = value;
                    OptionManager.effectSoundAudioSource.mute = value;
                }
                break;
            default:
                {
                    option[optionEnum] = value;
                }
                break;
        }
    }

    public static bool GetOption(EFlagOption optionEnum)
    {
        return option.ContainsKey(optionEnum) ? option[optionEnum] : true;
    }

    public static Dictionary<EFlagOption, bool> GetEntireOptionCopy()
    {
        var temp = new Dictionary<EFlagOption, bool>(option);
        return temp;
    }

    public static void SetEntireOption(Dictionary<EFlagOption, bool> param)
    {
        option = param;
        SaveAndApplyOption();
    }

    public static void SaveAndApplyOption()
    {
        foreach (var optionData in option)
        {
            switch (optionData.Key)
            {
                case EFlagOption.EffectSound:
                    {
                        OptionManager.effectSoundAudioSource.mute = !optionData.Value;
                    }
                    break;
            }

            PlayerPrefs.SetInt(optionData.Key.ToString(), optionData.Value ? 1 : 0);
        }
    }
    public static void ApplyOption()
    {
        foreach (var optionData in option)
        {
            switch (optionData.Key)
            {
                case EFlagOption.EffectSound:
                    {
                        OptionManager.effectSoundAudioSource.mute = !optionData.Value;
                    }
                    break;
            }
        }
    }
    public void LoadOption()
    {
        foreach (EFlagOption flagOption in Enum.GetValues(typeof(EFlagOption)))
        {
            int value = PlayerPrefs.GetInt(flagOption.ToString(), -1);
            if (value == -1)
            {
                option[flagOption] = true;
            }
            else
            {
                option[flagOption] = value == 1;
            }
        }

        ApplyOption();
    }
}
