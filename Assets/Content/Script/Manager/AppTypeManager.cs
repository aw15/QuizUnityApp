using System;
using System.Collections.Generic;
using UnityEngine;

namespace Content.Script.Manager
{
    public enum AppType
    {
        KoreanHistory,
        WorldHistory
    }


    public class AppSetting
    {
        public AppType appType = AppType.KoreanHistory;
        public string title = "한국사";
        public Color mainColor = new Color(242, 120, 48, 255);
        public readonly List<Sprite> titleSprites = new List<Sprite>();
        public string dataFilePath = @"Data\QuizData";
    }

    public class AppTypeManager : MonoBehaviour
    {
        public static readonly AppSetting AppSetting = new AppSetting();
        [SerializeField] private AppType appType = AppType.KoreanHistory;
        [SerializeField] private Color koreanHistoryMainColor = new Color(242, 120, 48, 255);
        [SerializeField] private Color worldHistoryMainColor = Color.yellow;
        public void Awake()
        {
            switch (appType)
            {
                case AppType.KoreanHistory:
                {
                    AppSetting.appType = AppType.KoreanHistory; 
                    AppSetting.title = "한국사";
                    AppSetting.mainColor = koreanHistoryMainColor;
                    AppSetting.titleSprites.Add(Resources.Load<Sprite>(@"Image\gyeongbokgung-palace"));
                    AppSetting.titleSprites.Add(Resources.Load<Sprite>(@"Image\king"));
                    AppSetting.titleSprites.Add(Resources.Load<Sprite>(@"Image\south-korean"));
                    AppSetting.dataFilePath = @"Data\QuizData";
                }
                break;
                case AppType.WorldHistory:
                {
                    AppSetting.appType = AppType.KoreanHistory; 
                    AppSetting.title = "세계사";
                    AppSetting.mainColor = worldHistoryMainColor;
                    AppSetting.titleSprites.Add(Resources.Load<Sprite>(@"Image\gyeongbokgung-palace"));
                    AppSetting.titleSprites.Add(Resources.Load<Sprite>(@"Image\giza"));
                    AppSetting.titleSprites.Add(Resources.Load<Sprite>(@"Image\leaning-tower-of-pisa"));
                    AppSetting.dataFilePath = @"Data\QuizData"; //파일 교체 필요.
                }
                break;
            }
        }
    }
}