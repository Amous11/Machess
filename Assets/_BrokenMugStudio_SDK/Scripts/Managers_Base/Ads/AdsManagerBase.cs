using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BrokenMugStudioSDK.RewardVideoEvents;

namespace BrokenMugStudioSDK
{
    public class AdsManagerBase : Singleton<AdsManager>
    {
        public delegate void AdsEvent();
        public static AdsEvent OnRewardVideoAvailabilityChange;
        public static AdsEvent OnRewardVideoSuccess;
        public static AdsEvent OnRewardVideoFailed;
        public static AdsEvent OnRewardVideoOpen;
        public static AdsEvent OnRewardVideoClose;
        private AdSettingsEditor m_AdSettings { get { return GameSettings.Instance.AdSettings; } }
        public bool IsRewardVideoLoaded { get { return m_AdSettings.EnableAds && m_AdSettings.EnableRewardedAd && MaxSdk.IsRewardedAdReady(m_AdSettings.RewardedAdUnitId); } }
        public bool IsInterestialReady { get { return m_AdSettings.EnableAds && m_AdSettings.EnableInterstitials && MaxSdk.IsInterstitialReady(m_AdSettings.InterstitialAdUnitId); } }
        private int m_InterstitialRetryAttempt;
        private float m_InterstitialLastTimeShown;
        private int m_InterstitialLastLevelShown;

        private int m_RewardedRetryAttempt;
        private int m_RewardedInterstitialRetryAttempt;
        [SerializeField]
        private bool m_Debug = true;
        public bool EnableDebug { get { return GameConfig.Instance.Debug.IsDebugMode && m_Debug; } }
        private bool m_IsBannerShowing;
        private bool m_IsMRecShowing;
        private bool m_RewardVideoRewardClaimed;
        private string m_LastPlacement;
        public override void Start()
        {
            base.Start();
            if (m_AdSettings.EnableAds)
            {
                InitializeAds();

            }
        }


        public void InitializeAds()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
            {
                // AppLovin SDK is initialized, configure and start loading ads.

                if (EnableDebug)
                {
                    Debug.Log("MAX SDK Initialized");
                }
                if (m_AdSettings.EnableInterstitials)
                {
                    InitializeInterstitialAds();
                }
                if (m_AdSettings.EnableRewardedAd)
                {
                    InitializeRewardedAds();
                }
                if (m_AdSettings.EnableBannerAd)
                {
                    InitializeBannerAds();
                }
                if (m_AdSettings.EnableMRecAd)
                {
                    InitializeMRecAds();
                }
                if (m_AdSettings.EnableRewardedInterstitialAd)
                {
                    InitializeRewardedInterstitialAds();
                }

            };

            MaxSdk.SetSdkKey(m_AdSettings.MaxSdkKey);
            MaxSdk.InitializeSdk();

        }

        public void ShowRewardVideo(Action i_SuccessCallback = null,
                                    Action i_FailCallback = null,
                                    Action i_OpenCallback = null,
                                    Action i_CloseCallback = null,
                                    string i_PlacementName = Constants.k_None)
        {
            ResetRvCallbacks();
            OnRewardVideoSuccess += () => { i_SuccessCallback?.InvokeSafe(); };
            OnRewardVideoFailed += () => { i_FailCallback?.InvokeSafe(); };
            OnRewardVideoOpen += () => { i_OpenCallback?.InvokeSafe(); };
            OnRewardVideoClose += () => { i_CloseCallback?.InvokeSafe(); };
            m_LastPlacement = i_PlacementName;
            ShowRewardedAd(i_PlacementName);

        }
        public void ShowRewardVideo(Action i_SuccessCallback = null,
                                    Action i_FailCallback = null,
                                    string i_PlacementName = Constants.k_None)
        {
            ShowRewardVideo(i_SuccessCallback, i_FailCallback, null, null, i_PlacementName);
        }
        public void ConditionalShowInterestials()
        {
            if (IsInterestialReady)
            {
                if (m_AdSettings.InterestialDelayType == AdSettingsEditor.eInterstitial.LevelBased)
                {
                    if (StorageManager.Instance.CurrentLevel % m_AdSettings.LevelInterval == 0)
                    {
                        ShowInterstitial();
                        m_InterstitialLastLevelShown = StorageManager.Instance.CurrentLevel;
                        m_InterstitialLastTimeShown = Time.time;
                    }
                }
                else if (m_AdSettings.InterestialDelayType == AdSettingsEditor.eInterstitial.TimeBased)
                {
                    if (Time.time - m_InterstitialLastTimeShown >= m_AdSettings.TimeInterval)
                    {
                        ShowInterstitial();
                        m_InterstitialLastLevelShown = StorageManager.Instance.CurrentLevel;
                        m_InterstitialLastTimeShown = Time.time;
                    }
                }

            }
        }
        public void ResetRvCallbacks()
        {
            m_RewardVideoRewardClaimed = false;
            OnRewardVideoSuccess = null;
            OnRewardVideoFailed = null;
            OnRewardVideoOpen = null;
            OnRewardVideoClose = null;
        }

        private void RewardVideoSuccess()
        {
            m_RewardVideoRewardClaimed = true;
            if (OnRewardVideoSuccess != null)
            {
                OnRewardVideoSuccess?.Invoke();
            }
        }
        private void RewardVideoFailed()
        {
            if (OnRewardVideoFailed != null)
            {
                OnRewardVideoFailed?.Invoke();
            }
        }
        private void RewardVideoClose()
        {
            if (OnRewardVideoClose != null)
            {
                OnRewardVideoClose?.Invoke();
            }
        }
        private void RewardVideoOpen()
        {
            if (OnRewardVideoOpen != null)
            {
                OnRewardVideoOpen?.Invoke();
            }
        }
        private void RewardVideoAvailabilityChanged(string i_Id = "")
        {
            if (OnRewardVideoAvailabilityChange != null)
            {
                OnRewardVideoAvailabilityChange?.Invoke();
            }
        }
        private void RewardVideoAvailabilityChanged(string i_Id, int i_ErrorCode)
        {
            if (OnRewardVideoAvailabilityChange != null)
            {
                OnRewardVideoAvailabilityChange?.Invoke();
            }
        }
        #region Max SDK methods
        #region Interstitial Ad Methods

        private void InitializeInterstitialAds()
        {
            // Attach callbacks
            MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;

            // Load the first interstitial
            LoadInterstitial();
        }

        void LoadInterstitial()
        {
            //interstitialStatusText.text = "Loading...";
            MaxSdk.LoadInterstitial(m_AdSettings.InterstitialAdUnitId);
        }

        void ShowInterstitial()
        {
            if (IsInterestialReady)
            {
                //interstitialStatusText.text = "Showing";
                MaxSdk.ShowInterstitial(m_AdSettings.InterstitialAdUnitId);
            }
            else
            {
                //interstitialStatusText.text = "Ad not ready";
            }
            AnalyticsManager.Instance.LogInterestialShowing("Interestials_" + StorageManager.Instance.CurrentLevel);

        }

        private void OnInterstitialLoadedEvent(string adUnitId)
        {
            // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
            //interstitialStatusText.text = "Loaded";
            if (EnableDebug)
            {
                Debug.Log("Interstitial loaded");
            }

            // Reset retry attempt
            m_InterstitialRetryAttempt = 0;
        }

        private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
        {
            // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
            m_InterstitialRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, m_InterstitialRetryAttempt));

            //interstitialStatusText.text = "Load failed: " + errorCode + "\nRetrying in " + retryDelay + "s...";
            if (EnableDebug)
            {
                Debug.Log("Interstitial failed to load with error code: " + errorCode);
            }
            if (m_InterstitialRetryAttempt < m_AdSettings.InterestialsMaxRetry)
            {
                Invoke(nameof(LoadInterstitial), (float)retryDelay);

            }
            else
            {
                m_InterstitialRetryAttempt = 0;
            }
            AnalyticsManager.Instance.LogInterestialFailed("Interestials_" + StorageManager.Instance.CurrentLevel);


        }

        private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
        {
            // Interstitial ad failed to display. We recommend loading the next ad
            if (EnableDebug)
            {
                Debug.Log("Interstitial failed to display with error code: " + errorCode);
            }
            LoadInterstitial();
        }

        private void OnInterstitialDismissedEvent(string adUnitId)
        {
            // Interstitial ad is hidden. Pre-load the next ad
            if (EnableDebug)
            {
                Debug.Log("Interstitial dismissed");
            }
            LoadInterstitial();
        }

        #endregion
        #region Rewarded Ad Methods

        private void InitializeRewardedAds()
        {
            // Attach callbacks
            MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.OnRewardedAdLoadedEvent += RewardVideoAvailabilityChanged;

            MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += RewardVideoAvailabilityChanged;

            MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            // Load the first RewardedAd
            LoadRewardedAd();
        }

        private void LoadRewardedAd()
        {
            //rewardedStatusText.text = "Loading...";
            MaxSdk.LoadRewardedAd(m_AdSettings.RewardedAdUnitId);
        }

        private void ShowRewardedAd(string i_Placement = "")
        {
            if (MaxSdk.IsRewardedAdReady(m_AdSettings.RewardedAdUnitId))
            {
                //rewardedStatusText.text = "Showing";
                if (EnableDebug)
                {
                    Debug.Log("Showing Reward AD");
                }
                MaxSdk.ShowRewardedAd(m_AdSettings.RewardedAdUnitId, i_Placement);
                AnalyticsManager.Instance.LogRewardAdsClicked(i_Placement);

            }
            else
            {
                if (EnableDebug)
                {
                    Debug.Log("Reward AD not ready");
                }
                AnalyticsManager.Instance.LogRewardAdsFailed(i_Placement);

                //rewardedStatusText.text = "Ad not ready";
            }
        }

        private void OnRewardedAdLoadedEvent(string adUnitId)
        {

            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
            //rewardedStatusText.text = "Loaded";
            if (EnableDebug)
            {
                Debug.Log("Rewarded ad loaded");
            }
            // Reset retry attempt
            m_RewardedRetryAttempt = 0;
        }

        private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
        {
            // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
            m_RewardedRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, m_RewardedRetryAttempt));

            //rewardedStatusText.text = "Load failed: " + errorCode + "\nRetrying in " + retryDelay + "s...";
            if (EnableDebug)
            {
                Debug.Log("Rewarded ad failed to load with error code: " + errorCode);
            }
            if (m_RewardedRetryAttempt < m_AdSettings.RewardedAdMaxRetry)
            {
                Invoke(nameof(LoadRewardedAd), (float)retryDelay);

            }
            else
            {
                m_RewardedRetryAttempt = 0;
            }
            AnalyticsManager.Instance.LogRewardAdsFailed(m_LastPlacement);

        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
        {
            // Rewarded ad failed to display. We recommend loading the next ad
            if (EnableDebug)
            {
                Debug.Log("Rewarded ad failed to display with error code: " + errorCode);
            }
            RewardVideoFailed();
            LoadRewardedAd();
            AnalyticsManager.Instance.LogRewardAdsFailed(m_LastPlacement);


        }

        private void OnRewardedAdDisplayedEvent(string adUnitId)
        {
            if (EnableDebug)
            {
                Debug.Log("Rewarded ad displayed");

            }
            RewardVideoOpen();
            AnalyticsManager.Instance.LogRewardAdsSuccess(m_LastPlacement);

        }

        private void OnRewardedAdClickedEvent(string adUnitId)
        {
            if (EnableDebug)
            {
                Debug.Log("Rewarded ad clicked");
            }
            AnalyticsManager.Instance.LogRewardAdsClicked(m_LastPlacement);


        }

        private void OnRewardedAdDismissedEvent(string adUnitId)
        {
            // Rewarded ad is hidden. Pre-load the next ad
            if (EnableDebug)
            {
                Debug.Log("Rewarded ad dismissed");
            }
            LoadRewardedAd();
            if (m_RewardVideoRewardClaimed)
            {
                RewardVideoClose();

            }
            else
            {
                RewardVideoFailed();
            }


        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
        {
            // Rewarded ad was displayed and user should receive the reward
            if (EnableDebug)
            {
                Debug.Log("Rewarded ad received reward");
            }
            RewardVideoSuccess();

        }

        #endregion
        #region Banner Ad Methods

        private void InitializeBannerAds()
        {
            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
            MaxSdk.CreateBanner(m_AdSettings.BannerAdUnitId, m_AdSettings.BannerPosition);

            // Set background or background color for banners to be fully functional.
            MaxSdk.SetBannerBackgroundColor(m_AdSettings.BannerAdUnitId, m_AdSettings.BannerBackgroundColor);
            Invoke(nameof(ShowBanner), m_AdSettings.BannerShowDelay);
        }
        private void ShowBanner()
        {
            if (!m_IsBannerShowing)
            {
                MaxSdk.ShowBanner(m_AdSettings.BannerAdUnitId);
            }
            m_IsBannerShowing = true;
            if (EnableDebug)
            {
                Debug.Log("ToggleBannerVisibility " + m_IsBannerShowing);
            }
        }
        private void HideBanner()
        {
            if (m_IsBannerShowing)
            {
                MaxSdk.HideBanner(m_AdSettings.BannerAdUnitId);
            }
            m_IsBannerShowing = false;
            if (EnableDebug)
            {
                Debug.Log("ToggleBannerVisibility " + m_IsBannerShowing);
            }
        }
        private void ToggleBannerVisibility()
        {
            if (!m_IsBannerShowing)
            {
                MaxSdk.ShowBanner(m_AdSettings.BannerAdUnitId);

            }
            else
            {
                MaxSdk.HideBanner(m_AdSettings.BannerAdUnitId);
                //showBannerButton.GetComponentInChildren<Text>().text = "Show Banner";
            }

            m_IsBannerShowing = !m_IsBannerShowing;
            if (EnableDebug)
            {
                Debug.Log("ToggleBannerVisibility " + m_IsBannerShowing);
            }
        }

        #endregion
        #region MREC Ad Methods

        private void InitializeMRecAds()
        {
            // MRECs are automatically sized to 300x250.
            MaxSdk.CreateMRec(m_AdSettings.MRecAdUnitId, m_AdSettings.AdViewPosition);
        }

        private void ToggleMRecVisibility()
        {
            if (!m_IsMRecShowing)
            {
                MaxSdk.ShowMRec(m_AdSettings.MRecAdUnitId);
                //showMRecButton.GetComponentInChildren<Text>().text = "Hide MREC";
            }
            else
            {
                MaxSdk.HideMRec(m_AdSettings.MRecAdUnitId);
                //showMRecButton.GetComponentInChildren<Text>().text = "Show MREC";
            }

            m_IsMRecShowing = !m_IsMRecShowing;
        }

        #endregion
        #region Rewarded Interstitial Ad Methods

        private void InitializeRewardedInterstitialAds()
        {
            // Attach callbacks
            MaxSdkCallbacks.OnRewardedInterstitialAdLoadedEvent += OnRewardedInterstitialAdLoadedEvent;
            MaxSdkCallbacks.OnRewardedInterstitialAdLoadFailedEvent += OnRewardedInterstitialAdFailedEvent;
            MaxSdkCallbacks.OnRewardedInterstitialAdFailedToDisplayEvent += OnRewardedInterstitialAdFailedToDisplayEvent;
            MaxSdkCallbacks.OnRewardedInterstitialAdDisplayedEvent += OnRewardedInterstitialAdDisplayedEvent;
            MaxSdkCallbacks.OnRewardedInterstitialAdClickedEvent += OnRewardedInterstitialAdClickedEvent;
            MaxSdkCallbacks.OnRewardedInterstitialAdHiddenEvent += OnRewardedInterstitialAdDismissedEvent;
            MaxSdkCallbacks.OnRewardedInterstitialAdReceivedRewardEvent += OnRewardedInterstitialAdReceivedRewardEvent;

            // Load the first RewardedInterstitialAd
            LoadRewardedInterstitialAd();
        }

        private void LoadRewardedInterstitialAd()
        {
            //rewardedInterstitialStatusText.text = "Loading...";
            MaxSdk.LoadRewardedInterstitialAd(m_AdSettings.RewardedInterstitialAdUnitId);
        }

        private void ShowRewardedInterstitialAd()
        {
            if (MaxSdk.IsRewardedInterstitialAdReady(m_AdSettings.RewardedInterstitialAdUnitId))
            {
                //rewardedInterstitialStatusText.text = "Showing";
                if (EnableDebug)
                {
                    Debug.Log("Showing Rewarded InterstitialAd");

                }
                MaxSdk.ShowRewardedInterstitialAd(m_AdSettings.RewardedInterstitialAdUnitId);
            }
            else
            {
                if (EnableDebug)
                {
                    Debug.Log(" Rewarded InterstitialAd Ad not ready");

                }
                //rewardedInterstitialStatusText.text = "Ad not ready";
            }
        }

        private void OnRewardedInterstitialAdLoadedEvent(string adUnitId)
        {
            // Rewarded interstitial ad is ready to be shown. MaxSdk.IsRewardedInterstitialAdReady(rewardedInterstitialAdUnitId) will now return 'true'
            //rewardedInterstitialStatusText.text = "Loaded";
            if (EnableDebug)
            {
                Debug.Log("Rewarded interstitial ad loaded");

            }

            // Reset retry attempt
            m_RewardedInterstitialRetryAttempt = 0;
        }

        private void OnRewardedInterstitialAdFailedEvent(string adUnitId, int errorCode)
        {
            // Rewarded interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
            m_RewardedInterstitialRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, m_RewardedInterstitialRetryAttempt));

            //rewardedInterstitialStatusText.text = "Load failed: " + errorCode + "\nRetrying in " + retryDelay + "s...";
            if (EnableDebug)
            {
                Debug.Log("Rewarded interstitial ad failed to load with error code: " + errorCode);

            }

            Invoke(nameof(LoadRewardedInterstitialAd), (float)retryDelay);
        }

        private void OnRewardedInterstitialAdFailedToDisplayEvent(string adUnitId, int errorCode)
        {
            // Rewarded interstitial ad failed to display. We recommend loading the next ad
            if (EnableDebug)
            {
                Debug.Log("Rewarded interstitial ad failed to display with error code: " + errorCode);

            }
            LoadRewardedInterstitialAd();
        }

        private void OnRewardedInterstitialAdDisplayedEvent(string adUnitId)
        {
            if (EnableDebug)
            {
                Debug.Log("Rewarded interstitial ad displayed");
            }
        }

        private void OnRewardedInterstitialAdClickedEvent(string adUnitId)
        {
            if (EnableDebug)
            {
                Debug.Log("Rewarded interstitial ad clicked");
            }
        }

        private void OnRewardedInterstitialAdDismissedEvent(string adUnitId)
        {
            // Rewarded interstitial ad is hidden. Pre-load the next ad
            if (EnableDebug)
            {
                Debug.Log("Rewarded interstitial ad dismissed");
            }
            LoadRewardedInterstitialAd();
        }

        private void OnRewardedInterstitialAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
        {
            // Rewarded interstitial ad was displayed and user should receive the reward
            if (EnableDebug)
            {
                Debug.Log("Rewarded interstitial ad received reward");
            }
        }

        #endregion
        #endregion

    }


}

