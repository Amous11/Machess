#if USING_ADJUST
using com.adjust.sdk;
#endif
using Facebook.Unity;
using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    public class AnalyticsManagerBase : Singleton<AnalyticsManager>
    {
        public const string k_LevelProgress = "Level_Progress";
        public const string k_LevelStarted = "Level_Started";
        public const string k_LevelRestarted = "Level_Restarted";
        public const string k_LevelReset = "Level_Reset";
        public const string k_LevelContinue = "Level_Continue";

        public const string k_LevelSkipped = "Level_Skipped";
        public const string k_LevelFailed = "Level_Failed";
        public const string k_LevelComplete = "Level_Complete";
        public const string k_UpgradePurchased = "Upgrade_Purchased";
        public const string k_ContentUnlocked = "Content_Unlocked";
        public const string k_TutorialComplete = "Tutorial_Complete";
        public const string k_HighScoreAchieved = "HighScore_Achieved";
        public const string k_RoundStarted = "Round_Started";
        public const string k_RoundComplete = "Round_Complete";



        public const string k_MaxSDK = "MaxSDK";
        private AnalyticsEditor AnalyticsSettings { get { return GameSettings.Instance.Analytics; } }
        private bool m_IsFBEnabledAndReady { get { return AnalyticsSettings.LogFacebookEvents && FB.IsInitialized; } }
        private bool m_IsGAEnabledAndReady { get { return AnalyticsSettings.LogGameAnalyticsEvents && m_GAInitCalled; } }
        private bool m_GAInitCalled = false;
        public override void Start()
        {
            base.Start();
            if (AnalyticsSettings.LogGameAnalyticsEvents)
            {
                GameAnalytics.Initialize();
                m_GAInitCalled = true;
            }
            if (AnalyticsSettings.LogFacebookEvents)
            {
                FB.Init();
            }
#if USING_ADJUST

            if (AnalyticsSettings.EnableAdjust)
            {
#if UNITY_ANDROID
                /* Mandatory - set your Android app token here */
                InitAdjust(AnalyticsSettings.AdjustToken);
#endif
            }
#endif
        }
#if USING_ADJUST

        private void InitAdjust(string adjustAppToken)
        {
            var adjustConfig = new AdjustConfig(
                adjustAppToken,
                AdjustEnvironment.Production, // AdjustEnvironment.Sandbox to test in dashboard
                true
            );
            adjustConfig.setLogLevel(AdjustLogLevel.Info); // AdjustLogLevel.Suppress to disable logs
            adjustConfig.setSendInBackground(true);
            Adjust adjust = new GameObject("Adjust").AddComponent<Adjust>(); // do not remove or rename
            adjust.transform.parent = transform;
            // Adjust.addSessionCallbackParameter("foo", "bar"); // if requested to set session-level parameters

            //adjustConfig.setAttributionChangedDelegate((adjustAttribution) => {
            //  Debug.LogFormat("Adjust Attribution Callback: ", adjustAttribution.trackerName);
            //});

            Adjust.start(adjustConfig);

        }
#endif
        public virtual void OnEnable()
        {
            GameManager.OnLevelStarted += LevelStartEvent;
            GameManager.OnLevelCompleted += LevelCompleteEvent;
            GameManager.OnLevelFailed += LevelFailedEvent;
            GameManager.OnLevelReset += LevelResetEvent;
            GameManager.OnLevelContinue += LevelContinueEvent;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            GameManager.OnLevelStarted -= LevelStartEvent;
            GameManager.OnLevelCompleted -= LevelCompleteEvent;
            GameManager.OnLevelFailed -= LevelFailedEvent;
            GameManager.OnLevelReset -= LevelResetEvent;
            GameManager.OnLevelContinue -= LevelContinueEvent;


        }
        public virtual void LogEvent(string i_Event, string i_GooglePlayId)
        {
            if (m_IsGAEnabledAndReady)
            {
                GameAnalytics.NewDesignEvent(i_Event + "_" + i_GooglePlayId);

            }
            if (m_IsFBEnabledAndReady)
            {
                FB.LogAppEvent(
                 i_Event,
                 null,
                 new Dictionary<string, object>()
                 {
                        { AppEventParameterName.Description, StorageManager.Instance.CurrentLevel }
                 });
            }


        }
        public virtual void LogAnalyticsEvent(eAnalyticsEvents i_Event, float i_FloatDetails)
        {
            if (m_IsGAEnabledAndReady)
            {
                GameAnalytics.NewDesignEvent(i_Event.ToString(), i_FloatDetails);

            }
            if (m_IsFBEnabledAndReady)
            {
                FB.LogAppEvent(
                i_Event.ToString(),
                i_FloatDetails);
            }
        }
        public virtual void LogAnalyticsEvent(eAnalyticsEvents i_Event)
        {
            if (m_IsGAEnabledAndReady)
            {
                GameAnalytics.NewDesignEvent(i_Event.ToString());

            }
            if (m_IsFBEnabledAndReady)
            {
                FB.LogAppEvent(
                i_Event.ToString());
            }
        }
        public virtual void LogCustomEvent(string i_Event, string i_Parameter1, string i_Detail1, string i_Parameter2, string i_Detail2)
        {
            if (m_IsGAEnabledAndReady)
            {
                GameAnalytics.NewDesignEvent(i_Event + ":" + i_Detail1 + ":" + i_Detail2);

            }
            if (m_IsFBEnabledAndReady)
            {
                FB.LogAppEvent(
                i_Event,
                null,
                new Dictionary<string, object>()
                {
                        { AppEventParameterName.Level, StorageManager.Instance.CurrentLevel },
                        {i_Parameter1, i_Detail1},
                        {i_Parameter2, i_Detail2}


                });
            }
        }
        public virtual void LogAdsEvent(GAAdAction i_Action, GAAdType i_ADType, string i_ADProvider, string i_Placement)
        {
            if (m_IsGAEnabledAndReady)
            {
                GameAnalytics.NewAdEvent(i_Action, i_ADType, i_ADProvider, i_Placement);

            }
            if (m_IsFBEnabledAndReady)
            {
                FB.LogAppEvent(
                    i_Action.ToString(),
                    null,
                    new Dictionary<string, object>()
                    {
                        { AppEventParameterName.Level, StorageManager.Instance.CurrentLevel },
                        {"Ad Type", i_ADType.ToString()},
                        {"Ad Placement", i_Placement}


                    });
            }
        }
        public virtual void LogRewardAdsClicked(string i_Placement)
        {
            if (m_IsGAEnabledAndReady)
            {
                LogAdsEvent(GAAdAction.Clicked, GAAdType.RewardedVideo, k_MaxSDK, i_Placement);

            }
            if (m_IsFBEnabledAndReady)
            {

            }
        }
        public virtual void LogRewardAdsSuccess(string i_Placement)
        {
            if (m_IsGAEnabledAndReady)
            {
                LogAdsEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, k_MaxSDK, i_Placement);

            }
            if (m_IsFBEnabledAndReady)
            {

            }
        }
        public virtual void LogRewardAdsFailed(string i_Placement)
        {
            if (m_IsGAEnabledAndReady)
            {
                LogAdsEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo, k_MaxSDK, i_Placement);

            }
            if (m_IsFBEnabledAndReady)
            {

            }
        }
        public virtual void LogInterestialShowing(string i_Placement)
        {
            if (m_IsGAEnabledAndReady)
            {
                LogAdsEvent(GAAdAction.Show, GAAdType.Interstitial, k_MaxSDK, i_Placement);

            }
            if (m_IsFBEnabledAndReady)
            {

            }
        }
        public virtual void LogInterestialFailed(string i_Placement)
        {
            if (m_IsGAEnabledAndReady)
            {
                LogAdsEvent(GAAdAction.FailedShow, GAAdType.Interstitial, k_MaxSDK, i_Placement);

            }
            if (m_IsFBEnabledAndReady)
            {

            }
        }
        public void LevelStartEvent()
        {
            if (m_IsGAEnabledAndReady)
            {
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level_" + GameConfig.Instance.Levels.CurrentLevelIndex, k_LevelProgress);

            }
            if (m_IsFBEnabledAndReady)
            {
                FB.LogAppEvent(
                 k_LevelStarted,
                 null,
                 new Dictionary<string, object>()
                 {
                        { AppEventParameterName.Level, StorageManager.Instance.CurrentLevel }
                 });
            }


        }
        public void LevelFailedEvent()
        {
            if (m_IsGAEnabledAndReady)
            {
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level_" + GameConfig.Instance.Levels.CurrentLevelIndex, k_LevelProgress);

            }
            if (m_IsFBEnabledAndReady)
            {
                FB.LogAppEvent(
                 k_LevelFailed,
                 null,
                 new Dictionary<string, object>()
                 {
                        { AppEventParameterName.Level, StorageManager.Instance.CurrentLevel }
                 });
            }

        }
        public void LevelCompleteEvent()
        {
            if (m_IsGAEnabledAndReady)
            {
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level_" + GameConfig.Instance.Levels.CurrentLevelIndex, k_LevelProgress);

            }
            if (m_IsFBEnabledAndReady)
            {
                FB.LogAppEvent(
                 k_LevelComplete,
                 null,
                 new Dictionary<string, object>()
                 {
                        { AppEventParameterName.Level, StorageManager.Instance.CurrentLevel }
                 });
            }

        }
        public void LevelResetEvent()
        {
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level_" + StorageManager.Instance.CurrentLevel);
            if (m_IsGAEnabledAndReady)
            {
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, "LevelReset_" + StorageManager.Instance.CurrentLevel);

            }
            if (m_IsFBEnabledAndReady)
            {
                FB.LogAppEvent(
                 k_LevelReset,
                 null,
                 new Dictionary<string, object>()
                 {
                        { AppEventParameterName.Level, StorageManager.Instance.CurrentLevel }
                 });
            }

        }
        public void LevelContinueEvent()
        {
            if (m_IsGAEnabledAndReady)
            {
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, "LevelContinue_" + StorageManager.Instance.CurrentLevel);

            }
            if (m_IsFBEnabledAndReady)
            {
                FB.LogAppEvent(
                 k_LevelContinue,
                 null,
                 new Dictionary<string, object>()
                 {
                        { AppEventParameterName.Level, StorageManager.Instance.CurrentLevel }
                 });
            }

        }
    }
}
