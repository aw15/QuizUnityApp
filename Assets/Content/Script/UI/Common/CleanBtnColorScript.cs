using System;
using Content.Script.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Script.UI.Common
{
    public class CleanBtnColorScript : MonoBehaviour
    {
        [SerializeField] private Image imageWidget;

        public void Start()
        {
            SetColor(AppTypeManager.AppSetting.mainColor);
        }

        public void SetColor(Color color)
        {
            imageWidget.color = color;
        }

        public void SetAlpha(float alpha)
        {
            imageWidget.color = new Color(imageWidget.color.r, imageWidget.color.b, imageWidget.color.g, alpha);
        }
        
        public void SetDefaultColor()
        {
            imageWidget.color = AppTypeManager.AppSetting.mainColor;
        }
    }
}