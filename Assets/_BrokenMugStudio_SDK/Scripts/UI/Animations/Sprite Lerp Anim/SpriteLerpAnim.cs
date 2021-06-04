using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace BrokenMugStudioSDK.Animation
{
    /// <summary>
    /// Easily Lerp sprites from current RectTransform Data to Target RectTransform Data
    /// </summary>
    public class SpriteLerpAnim : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform rectTransform;

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
            DOTween.Kill(this);
        }

        protected virtual void CopyTarget(RectTransform i_OriginalTransform)
        {
            transform.position = i_OriginalTransform.position;
            rectTransform.sizeDelta = i_OriginalTransform.sizeDelta;
            transform.localScale = i_OriginalTransform.localScale;
            rectTransform.sizeDelta = i_OriginalTransform.sizeDelta;
        }

        /// <summary>
        /// Animates from Origin Rect Transform to Target
        /// </summary>
        /// <param name="i_OriginalTransform"></param>
        /// <param name="i_TargetTransform"></param>
        /// <param name="i_Data"></param>
        /// <param name="i_Callback"></param>
        public virtual void Animate(RectTransform i_OriginalTransform, RectTransform i_TargetTransform, TweenData i_Data, Action<SpriteLerpAnim> i_Callback)
        {
            CopyTarget(i_OriginalTransform);

            Animate(i_TargetTransform, i_Data, i_Callback);
        }

        /// <summary>
        /// Animates from current position to Target
        /// </summary>
        /// <param name="i_OriginalTransform"></param>
        /// <param name="i_TargetTransform"></param>
        /// <param name="i_Data"></param>
        /// <param name="i_Callback"></param>
        public virtual void Animate(RectTransform i_TargetTransform, TweenData i_Data, Action<SpriteLerpAnim> i_Callback)
        {
            rectTransform.DOMove(i_TargetTransform.position, i_Data.Duration)
                .SetEase(i_Data.Ease)
                .SetId(this)
                .OnComplete(() =>
                {
                    i_Callback.Invoke(this);
                });

            rectTransform.DOSizeDelta(i_TargetTransform.sizeDelta * i_TargetTransform.localScale, i_Data.Duration)
                .SetEase(i_Data.Ease)
                .SetId(this);
        }
    }
}