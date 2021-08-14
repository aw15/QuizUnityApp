using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyrightPanel : IPanel
{
    public override string GetPanelName()
    {
        return "CopyrightPanel";
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
