// Copyright (C) 2015-2021 gamevanilla - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace UltimateClean
{
    /// <summary>
    /// Miscellaneous utilities.
    /// </summary>
    public static class Utils
    {
        public static IEnumerator FadeIn(UnityEngine.CanvasGroup group, float alpha, float duration)
        {
            var time = 0.0f;
            var originalAlpha = group.alpha;
            while (time < duration)
            {
                time += UnityEngine.Time.deltaTime;
                group.alpha = UnityEngine.Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new UnityEngine.WaitForEndOfFrame();
            }

            group.alpha = alpha;
        }

        public static IEnumerator FadeOut(UnityEngine.CanvasGroup group, float alpha, float duration)
        {
            var time = 0.0f;
            var originalAlpha = group.alpha;
            while (time < duration)
            {
                time += UnityEngine.Time.deltaTime;
                group.alpha = UnityEngine.Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new UnityEngine.WaitForEndOfFrame();
            }

            group.alpha = alpha;
        }
    }
}
