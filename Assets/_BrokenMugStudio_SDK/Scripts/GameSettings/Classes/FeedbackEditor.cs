using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    [Serializable]
    public class FeedbackEditor
    {
        public int[] ShowRateUsAtLevels=new int[] { 3, 6, 9, 15 };
        public bool ShowRateUsNow()
        {
            return ShowRateUsAtLevels.Contains(StorageManager.Instance.CurrentLevel);
        }

    }
}
