
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

public static class CommonDefines
{
    public static char[] consonant = { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ',
                           'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ','ㅋ','ㅌ', 'ㅍ', 'ㅎ' };

    public static string[] consonantCombination = { "가", "까", "나", "다", "따", "라", "마", "바", "빠", "사", "싸",
                           "아", "자", "짜", "차","카","타", "파", "하" };

    public static int[] consonantNum = {44032,44620,45208,45796,46384,46972,47560,48148,48736,49324,49912,
                               50500,51088,51676,52264,52852,53440,54028,54616,55204};

    public static readonly int stageQuizCount = 5;
}

public class WaitUntilForSeconds : CustomYieldInstruction
{
    float pauseTime;
    float timer;
    bool waitingForFirst;
    Func<bool> myChecker;
    Action<float> onInterrupt;
    bool alwaysTrue;

    public WaitUntilForSeconds(Func<bool> myChecker, float pauseTime,
            Action<float> onInterrupt = null)
    {
        this.myChecker = myChecker;
        this.pauseTime = pauseTime;
        this.onInterrupt = onInterrupt;

        waitingForFirst = true;
    }

    public override bool keepWaiting
    {
        get
        {
            bool checkThisTurn = myChecker();
            if (waitingForFirst)
            {
                if (checkThisTurn)
                {
                    waitingForFirst = false;
                    timer = pauseTime;
                    alwaysTrue = true;
                }
            }
            else
            {
                timer -= Time.deltaTime;

                if (onInterrupt != null && !checkThisTurn && alwaysTrue)
                {
                    onInterrupt(timer);
                }
                alwaysTrue &= checkThisTurn;

                // Alternate version: Interrupt the timer on false, 
                // and restart the wait
                // if (!alwaysTrue || timer <= 0)

                if (timer <= 0)
                {
                    if (alwaysTrue)
                    {
                        return false;
                    }
                    else
                    {
                        waitingForFirst = true;
                    }
                }
            }

            return true;
        }
    }
}

public static class Utils
{
    public static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
    }

    public static string HangulSearchPattern(string param)
    {
        string pattern = string.Empty;
        for (int i = 0; i < param.Length; i++)
        {
            //초성만 입력되었을때
            if (param[i] >= 'ㄱ' && param[i] <= 'ㅎ')
            {
                for (int j = 0; j < CommonDefines.consonant.Length; j++)
                {
                    if (param[i] == CommonDefines.consonant[j])
                    {
                        pattern += string.Format("[{0}-{1}]", CommonDefines.consonantCombination[j], (char)(CommonDefines.consonantNum[j + 1] - 1));
                    }
                }
            }
            //완성된 문자를 입력했을때 검색패턴 쓰기
            else if (param[i] >= '가')
            {
                //받침이 있는지 검사
                int magic = ((param[i] - '가') % 588);

                //받침이 없을때.
                if (magic == 0)
                {
                    pattern += string.Format("[{0}-{1}]", param[i], (char)(param[i] + 27));
                }

                //받침이 있을때
                else
                {
                    magic = 27 - (magic % 28);
                    pattern += string.Format("[{0}-{1}]", param[i], (char)(param[i] + magic));
                }
            }
            //영어를 입력했을때
            else if (param[i] >= 'A' && param[i] <= 'z')
            {
                pattern += param[i];
            }
            //숫자를 입력했을때.
            else if (param[i] >= '0' && param[i] <= '9')
            {
                pattern += param[i];
            }
        }

        return pattern;
    }
    public static string GetSecondToHMS(int seconds)
    {
        int hour = (int)seconds / 3600;
        int minute = ((int)seconds % 3600) / 60;
        int second = (int)seconds % 60;
        return string.Format("{0} : {1:00} : {2:00}", hour, minute, second);
    }
    public static class ThreadSafeRandom
    {
        [System.ThreadStatic] private static System.Random Local;

        public static System.Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new System.Random(unchecked(System.Environment.TickCount * 31 + System.Threading.Thread.CurrentThread.ManagedThreadId))); }
        }
    }
    public static void Shuffle<T>(this System.Collections.Generic.IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
