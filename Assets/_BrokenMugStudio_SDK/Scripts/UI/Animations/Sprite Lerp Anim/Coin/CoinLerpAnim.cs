using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenMugStudioSDK.Animation
{
    [Serializable]
    public struct CoinLerpAnimData
    {
        [Header("Phase 1 - Scale Up")]
        public float InitialSizeDeltaAmount;
        public float InitialSizeDeltaDuration;
        public Ease InitialSizeDeltaEase;

        [Header("Phase 2 - Move")]
        public Vector3 MoveOffset;
        public float MoveDuration;
        public Ease MoveEase;

        [Header("Phase 3 - Scale Down")]
        public float FinalSizeDeltaAmount;
        public float FinalSizeDeltaDuration;
        public Ease FinalSizeDeltaEase;

        [Header("Phase 4 - Animate to Final Pos")]
        public TweenData FinalAnimData;
    }

    /// <summary>
    /// Animation that triggers when receiving coins. At first it's hardcoded, then it uses SpriteLerpAnim to move towards the target sprite
    /// </summary>
    public class CoinLerpAnim : SpriteLerpAnim
    {
        private CoinLerpAnimData m_LerpData;

        private RectTransform m_TargetTransform;
        private Action<SpriteLerpAnim> m_Callback;

        /// <summary>
        /// Performs Coin Anim from Origin Rect Transform
        /// </summary>
        /// <param name="i_OriginalTransform"></param>
        /// <param name="i_TargetTransform"></param>
        /// <param name="i_Data"></param>
        /// <param name="i_Callback"></param>
        public void Animate(RectTransform i_OriginalTransform, RectTransform i_TargetTransform, CoinLerpAnimData i_LerpData, Action<SpriteLerpAnim> i_Callback)
        {
            CopyTarget(i_OriginalTransform);

            Animate(i_TargetTransform, i_LerpData, i_Callback);
        }

        /// <summary>
        /// Performs Coin Anim from current position
        /// </summary>
        /// <param name="i_OriginalTransform"></param>
        /// <param name="i_TargetTransform"></param>
        /// <param name="i_Data"></param>
        /// <param name="i_Callback"></param>
        public void Animate(RectTransform i_TargetTransform, CoinLerpAnimData i_LerpData, Action<SpriteLerpAnim> i_Callback)
        {
            m_LerpData = i_LerpData;
            m_TargetTransform = i_TargetTransform;
            m_Callback = i_Callback;

            InitialScale();
        }

        private void InitialScale()
        {
            rectTransform.DOSizeDelta(Vector2.one * m_LerpData.InitialSizeDeltaAmount, m_LerpData.InitialSizeDeltaDuration)
                .SetEase(m_LerpData.InitialSizeDeltaEase)
                .OnComplete(MoveOffset)
                .SetId(this);
        }

        private void MoveOffset()
        {
            rectTransform.DOAnchorPos(m_LerpData.MoveOffset, m_LerpData.MoveDuration)
                .SetEase(m_LerpData.MoveEase)
                .SetRelative(true)
                .OnComplete(FinalScale)
                .SetId(this);
        }

        private void FinalScale()
        {
            rectTransform.DOSizeDelta(Vector2.one * m_LerpData.FinalSizeDeltaAmount, m_LerpData.FinalSizeDeltaDuration)
                .SetEase(m_LerpData.FinalSizeDeltaEase)
                .OnComplete(() => 
                { 
                    base.Animate(m_TargetTransform, m_LerpData.FinalAnimData, m_Callback); 
                })
                .SetId(this);
        }
    }
}