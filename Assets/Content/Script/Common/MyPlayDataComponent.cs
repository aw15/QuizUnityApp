using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum MyPlayDataType
{
    PlayTime,
    AnswerRate
}

public static class MyPlayDataComponent
{
    public static int playTime = 0;
    public static string playTimeString = "00:00:00";
    public static int totalAnswerCount = 0;
    public static int correctAnswerCount = 0;
    public static string answerRateString = "0 %";
    const string totalAnswerCountKey = "TotalAnswerCount";
    const string correctAnswerCountKey = "CorrectAnswerCount";
    const string playTimeKey = "PlayTime";
    public static void LoadOnStart()
    {
        totalAnswerCount = PlayerPrefs.GetInt(totalAnswerCountKey, 0);
        correctAnswerCount = PlayerPrefs.GetInt(correctAnswerCountKey, 0);
        playTime = PlayerPrefs.GetInt(playTimeKey, 0);
        playTimeString = Utils.GetSecondToHMS(playTime);
        answerRateString = GetAnswerRateString();
    }

    static string GetAnswerRateString()
    {
        if(totalAnswerCount == 0)
        {
            return "0 %";
        }

        float answerRate = (correctAnswerCount / (float)totalAnswerCount) * 100;
        return $"{(int)answerRate} %";
    }
    public static void UpdatePlayTime(float currentPlayTime)
    {
        playTime += (int)currentPlayTime;
        PlayerPrefs.SetInt(playTimeKey, playTime);
        playTimeString = Utils.GetSecondToHMS(playTime); 
    }
    public static void UpdateAnswerRate(int currentCorrectAnswerCount, int currentTotalAnswerCount)
    {
        correctAnswerCount += currentCorrectAnswerCount;
        totalAnswerCount += currentTotalAnswerCount;

        PlayerPrefs.SetInt(totalAnswerCountKey, totalAnswerCount);
        PlayerPrefs.SetInt(correctAnswerCountKey, correctAnswerCount);

        answerRateString = GetAnswerRateString();
    }
}
