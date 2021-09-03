using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestScoreComponent
{
    public class QuizDataEqualityComparer : IEqualityComparer<DataManager.QuizData>
    {
        public bool Equals(DataManager.QuizData x, DataManager.QuizData y)
        {
            return x.quiz == y.quiz;
        }

        public int GetHashCode(DataManager.QuizData obj)
        {
            return obj.quiz.ToString().GetHashCode();
        }
    }

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
    public void LoadOnStart()
    {
        LoadBestScoreList();
        for(int i = 0; i< bestScoreListFromJson.data.Count; ++i)
        {
            bestScoreDatas.Add(bestScoreListFromJson.data[i].stage, bestScoreListFromJson.data[i].score);
        }
    }
    public Dictionary<int, int> GetBestScoreDatas()
    {
        return bestScoreDatas;
    }
    public int GetScore(int stage)
    {
        if(bestScoreDatas.ContainsKey(stage))
        {
            return bestScoreDatas[stage];
        }
        return 0;
    }
    public void UpdateBestScore(int stage, int score)
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
    public void RemoveBookmark(int stage)
    {
        if (bestScoreDatas.ContainsKey(stage))
        {
            bestScoreDatas.Remove(stage);
            SaveBestScoreList();
        }
    }
    public void SaveBestScoreList()
    {
        DataManager.SaveJson<BestScoreList>(bestScoreSaveFile, bestScoreListFromJson);
    }

    public void LoadBestScoreList()
    {
        bestScoreListFromJson = DataManager.LoadJson<BestScoreList>(bestScoreSaveFile);
        if (bestScoreListFromJson == null)
            bestScoreListFromJson = new BestScoreList();
    }
}
