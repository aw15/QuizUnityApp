using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePanel : IPanel
{
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
}
