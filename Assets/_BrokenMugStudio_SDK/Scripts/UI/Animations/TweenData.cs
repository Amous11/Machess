using DG.Tweening;
using System;

namespace BrokenMugStudioSDK.Animation
{
    [Serializable]
    public struct TweenData
    {
        public float Duration;
        public Ease Ease;

        public int LoopCount;
    }

    [Serializable]
    public struct TweenDataInOut
    {
        public float DurationIn;
        public float DurationOut;

        public Ease EaseIn;
        public Ease EaseOut;
    }
}