using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    [Serializable]
    public class AdSettingsEditor
    {
        public enum eInterstitial { TimeBased, LevelBased }
        public string MaxSdkKey = "9R5sR2pTfai196fIM_t0Tw4KlWHT2Xh4pURpiCWWr8TO5h4r5WyJfpfpuUSf9MUr72TK48N-AAp-WvuxWVdbO6";
        public bool EnableAds = true;

        //Interestials
        [Title("Interstitial ads ")]
        public bool EnableInterstitials = true;
        [ShowIf(nameof(EnableInterstitials))]
        public string InterstitialAdUnitId = "cb0ff6e3f8b886df";
        [ShowIf(nameof(EnableInterstitials))]
        public int InterestialsMaxRetry = 2;
        [ShowIf(nameof(EnableInterstitials))]
        public bool ShowInterestialsOnLevelFailed=true;
        [ShowIf(nameof(EnableInterstitials))]
        public bool ShowInterestialsOnLevelCompleted=true;
        [ShowIf(nameof(EnableInterstitials))]
        public eInterstitial InterestialDelayType;
        [ShowIf(nameof(InterestialDelayType), eInterstitial.TimeBased)]
        public float TimeInterval = 300f;
        [ShowIf(nameof(InterestialDelayType), eInterstitial.LevelBased)]
        public int LevelInterval = 2;

        //Reward Video Ads
        [Title("Reward Video Ads ")]
        public bool EnableRewardedAd = true;
        [ShowIf(nameof(EnableRewardedAd))]
        public string RewardedAdUnitId = "5a3f677156fecfcf";
        [ShowIf(nameof(EnableRewardedAd))]
        public int RewardedAdMaxRetry = 5;

        //Banner Ads
        [Title("Banner Ad")]
        public bool EnableBannerAd = true;
        [ShowIf(nameof(EnableBannerAd))]
        public string BannerAdUnitId = "e976a0cb2c86d552";
        [ShowIf(nameof(EnableBannerAd))]
        public MaxSdkBase.BannerPosition BannerPosition = MaxSdkBase.BannerPosition.BottomCenter;
        [ShowIf(nameof(EnableBannerAd))]
        public Color BannerBackgroundColor = Color.black;
        [ShowIf(nameof(EnableBannerAd))]
        public float BannerShowDelay = 5;

        //MRecAd
        [Title("MRec Ad")]
        public bool EnableMRecAd = false;
        [ShowIf(nameof(EnableMRecAd))]
        public string MRecAdUnitId = "ENTER_MREC_AD_UNIT_ID_HERE";
        [ShowIf(nameof(EnableMRecAd))]
        public MaxSdkBase.AdViewPosition AdViewPosition;

        //Rewarded Interstitial
        [Title("Rewarded Interstitial")]
        public bool EnableRewardedInterstitialAd = false;
        [ShowIf(nameof(EnableRewardedInterstitialAd))]
        public string RewardedInterstitialAdUnitId = "ENTER_REWARD_INTER_AD_UNIT_ID_HERE";
    }
}

