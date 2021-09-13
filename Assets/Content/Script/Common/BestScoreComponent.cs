using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BestScoreComponent
{
    [Serializable]
    class BestScoreData
    {
        public BestScoreData(int inStage, int inScore)
        {
            stage = inStage;
            score = inScore;
        }
        public int stage = -1;
        public int score = 0;
    }

    class BestScoreList
    {
        public List<BestScoreData> data = new List<BestScoreData>();
    }

    static BestScoreList bestScoreListFromJson; //LoadOnStart에서 할당
    static Dictionary<int, int> bestScoreDatas = new Dictionary<int, int>();
    static readonly string bestScoreSaveFile = "bestScore.json";
    public static void LoadOnStart()
    {
        LoadBestScoreList();
        for(int i = 0; i< bestScoreListFromJson.data.Count; ++i)
        {
            bestScoreDatas.Add(bestScoreListFromJson.data[i].stage, bestScoreListFromJson.data[i].score);
        }
    }
    public static Dictionary<int, int> GetBestScoreDatas()
    {
        return bestScoreDatas;
    }
    public static int GetScore(int stage)
    {
        if(bestScoreDatas.ContainsKey(stage))
        {
            return bestScoreDatas[stage];
        }
        return 0;
    }
    public static void UpdateBestScore(int stage, int score)
    {
        if(bestScoreDatas.ContainsKey(stage))
        {
            if (bestScoreDatas[stage] >= score)
                return;

            for (int i = 0; i < bestScoreListFromJson.data.Count; ++i)
            {
                if (stage == bestScoreListFromJson.data[i].stage)
                {
                    bestScoreListFromJson.data[i].score = score;
                }
            }
        }
        else
        {
            bestScoreListFromJson.data.Add(new BestScoreData(stage, score));
        }
        bestScoreDatas[stage] = score;

        SaveBestScoreList();
    }
    public static void SaveBestScoreList()
    {
        DataManager.SaveJson<BestScoreList>(bestScoreSaveFile, bestScoreListFromJson);
    }

    public static void LoadBestScoreList()
    {
        bestScoreListFromJson = DataManager.LoadJson<BestScoreList>(bestScoreSaveFile);
        if (bestScoreListFromJson == null)
            bestScoreListFromJson = new BestScoreList();
    }
}
