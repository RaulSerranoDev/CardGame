using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MoreMountains.Tools
{
    /// <summary>
    /// Various static methods used throughout the Infinite Runner Engine and the Corgi Engine.
    /// </summary>

    public static class MMFade
    {

        /// <summary>
        /// Fades the specified image to the target opacity and duration.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="opacity">Opacity.</param>
        /// <param name="duration">Duration.</param>
        public static IEnumerator FadeImage(Image target, float duration, Color color)
        {
            if (target == null)
                yield break;

            float alpha = target.color.a;

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
            {
                if (target == null)
                    yield break;
                Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
                target.color = newColor;
                yield return null;
            }
            target.color = color;

        }
        /// <summary>
        /// Fades the specified image to the target opacity and duration.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="opacity">Opacity.</param>
        /// <param name="duration">Duration.</param>
        public static IEnumerator FadeText(Text target, float duration, Color color)
        {
            if (target == null)
                yield break;

            float alpha = target.color.a;

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
            {
                if (target == null)
                    yield break;
                Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
                target.color = newColor;
                yield return null;
            }
            target.color = color;
        }
        /// <summary>
        /// Fades the specified image to the target opacity and duration.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="opacity">Opacity.</param>
        /// <param name="duration">Duration.</param>
        public static IEnumerator FadeSprite(SpriteRenderer target, float duration, Color color)
        {
            if (target == null)
                yield break;

            float alpha = target.color.a;

            float t = 0f;
            while (t < 1.0f)
            {
                if (target == null)
                    yield break;

                Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
                target.color = newColor;

                t += Time.deltaTime / duration;

                yield return null;

            }
            Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
            if (target) target.color = finalColor;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        /// <param name="targetAlpha"></param>
        /// <returns></returns>
		public static IEnumerator FadeCanvasGroup(CanvasGroup target, float duration, float targetAlpha)
        {
            if (target == null)
                yield break;

            float currentAlpha = target.alpha;

            float t = 0f;
            while (t < 1.0f)
            {
                if (target == null)
                    yield break;

                float newAlpha = Mathf.SmoothStep(currentAlpha, targetAlpha, t);
                target.alpha = newAlpha;

                t += Time.deltaTime / duration;

                yield return null;

            }
            target.alpha = targetAlpha;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skRenderer"></param>
        /// <param name="duration"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        //public static IEnumerator FadeMaterial(SkeletonRenderer skRenderer, float duration, Color color)
        //{
        //    Color temp = new Color(skRenderer.skeleton.r, skRenderer.skeleton.g, skRenderer.skeleton.b, skRenderer.skeleton.a);

        //    float t = 0f;
        //    while (t < 1.0f)
        //    {
        //        Color newColor = new Color(
        //            Mathf.SmoothStep(temp.r, color.r, t),
        //            Mathf.SmoothStep(temp.g, color.g, t),
        //            Mathf.SmoothStep(temp.b, color.b, t),
        //            Mathf.SmoothStep(temp.a, color.a, t));
        //        skRenderer.skeleton.SetColor(newColor);

        //        t += Time.deltaTime / duration;

        //        yield return null;
        //    }
        //}
    }
}
