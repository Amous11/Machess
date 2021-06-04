using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    public class ManagersBase : Singleton<Managers>
    {
        public AdsManager Ads;
        public AnalyticsManager Analytics;
        public StorageManager Storage;
        public GameManager GameManager;
        public HapticManager HapticManager;
        public PoolManager PoolManager;


        [Button]
        public virtual void Reset()
        {

            Ads = Ads ?? this.GetComponentInChildren<AdsManager>();

            Analytics = Analytics ?? this.GetComponentInChildren<AnalyticsManager>();


            Storage = Storage ?? this.GetComponentInChildren<StorageManager>();

            GameManager = GameManager ?? this.GetComponentInChildren<GameManager>();


            HapticManager = HapticManager ?? this.GetComponentInChildren<HapticManager>();
            PoolManager = PoolManager ?? this.GetComponentInChildren<PoolManager>();




        }
    }
}

