using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    public class GameManagerBase : Singleton<GameManager>, IGameEvents
    {
        #region Events
        public delegate void GameEvent();
        public static event GameEvent OnGameStateChange = delegate { };

        public delegate void LevelEvent();
        public static event LevelEvent OnLevelReset = delegate { };
        public static event LevelEvent OnLevelLoaded = delegate { };
        public static event LevelEvent OnLevelLoadComplete = delegate { };
        public static event LevelEvent OnLevelStarted = delegate { };
        public static event LevelEvent OnLevelPause = delegate { };
        public static event LevelEvent OnLevelUnPause = delegate { };

        public static event LevelEvent OnLevelContinue = delegate { };
        public static event LevelEvent OnLevelCompleted = delegate { };

        public static event LevelEvent OnLevelFailed = delegate { };
        public static event LevelEvent OnLevelFailedNoContinue = delegate { };
        #endregion
        private eGameState m_GameState;
        private AdSettingsEditor m_AdSettings { get { return GameSettings.Instance.AdSettings; } }

        [ShowInInspector, ReadOnly]
        public eGameState GameState
        {
            get
            {
                return m_GameState;
            }
            set
            {
                if (m_GameState != value)
                {
                    if (OnGameStateChange != null)
                    {
                        m_GameState = value;
                        OnGameStateChange?.Invoke();
                    }
                }
                m_GameState = value;
            }
        }
        private bool m_DebugEnabled { get { return GameConfig.Instance.Debug.IsDebugMode; } }
        protected override void OnAwakeEvent()
        {
            base.OnAwakeEvent();
            StorageManagerBase.Instance.CurrentLevel = StorageManagerBase.Instance.HighScoreLevel;
            Application.targetFrameRate = 60;

        }
        public override void Start()
        {
            base.Start();
            ResetGame();
        }
        public virtual void OnEnable()
        {

        }
        public override void OnDisable()
        {
            base.OnDisable();

        }
        public virtual void ResetGame()
        {
            if (m_DebugEnabled)
            {
                Debug.Log(nameof(ResetGame));
            }
            if (GameState == eGameState.Completed && m_AdSettings.ShowInterestialsOnLevelCompleted)
            {
                AdsManager.Instance.ConditionalShowInterestials();
            }
            else if (GameState == eGameState.GameOver && m_AdSettings.ShowInterestialsOnLevelFailed)
            {
                AdsManager.Instance.ConditionalShowInterestials();
            }

            GameState = eGameState.Idle;
            if (OnLevelReset != null)
            {
                OnLevelReset?.Invoke();
            }
            LevelLoaded();
        }
        public virtual void LevelLoaded()
        {
            if (m_DebugEnabled)
            {
                Debug.Log(nameof(LevelLoaded));
            }

            if (OnLevelLoaded != null)
            {
                OnLevelLoaded?.Invoke();
            }
        }
        public virtual void LevelLoadComplete()
        {

            if (m_DebugEnabled)
            {
                Debug.Log(nameof(OnLevelLoadComplete));
            }

            if (OnLevelLoadComplete != null)
            {
                OnLevelLoadComplete?.Invoke();
            }
        }
        public void LoadLevel()
        {

        }
        public virtual void LevelStarted()
        {
            if (m_DebugEnabled)
            {
                Debug.Log(nameof(LevelStarted));
            }

            GameState = eGameState.Playing;

            if (OnLevelStarted != null)
            {
                OnLevelStarted?.Invoke();
            }
        }
        public virtual void LevelContinue()
        {
            if (m_DebugEnabled)
            {
                Debug.Log(nameof(LevelContinue));
            }

            GameState = eGameState.Playing;

            if (OnLevelStarted != null)
            {
                OnLevelContinue?.Invoke();
            }

        }


        private eGameState m_PrePauseState;
        public virtual void PauseGame()
        {
            if (m_DebugEnabled)
            {
                Debug.Log(nameof(PauseGame));
            }
            m_PrePauseState = GameState;
            GameState = eGameState.Paused;

            if (OnLevelPause != null)
            {
                OnLevelPause?.Invoke();
            }
        }

        public virtual void UnPauseGame()
        {
            if (m_DebugEnabled)
            {
                Debug.Log(nameof(UnPauseGame));
            }

            GameState = m_PrePauseState;

            if (OnLevelUnPause != null)
            {
                OnLevelUnPause?.Invoke();
            }
        }

        public virtual void LevelCompleted()
        {
            if (m_DebugEnabled)
            {
                Debug.Log(nameof(LevelCompleted));
            }

            GameState = eGameState.Completed;

            if (OnLevelCompleted != null)
            {
                OnLevelCompleted?.Invoke();
            }
        }

        public virtual void LevelFailed()
        {
            if (m_DebugEnabled)
            {
                Debug.Log(nameof(LevelFailed));
            }

            GameState = eGameState.GamePreOver;

            if (OnLevelFailed != null)
            {
                OnLevelFailed?.Invoke();
            }
        }

        public virtual void LevelFailedNoContinue()
        {
            if (m_DebugEnabled)
            {
                Debug.Log(nameof(LevelFailedNoContinue));
            }

            GameState = eGameState.GameOver;

            if (OnLevelFailedNoContinue != null)
            {
                OnLevelFailedNoContinue?.Invoke();
            }
        }

        public virtual void GameOver(bool i_ForceNoContinue = false)
        {
            if (m_DebugEnabled)
            {
                Debug.Log(nameof(GameOver) + "_" + nameof(i_ForceNoContinue) + "_" + i_ForceNoContinue);
            }

            if (i_ForceNoContinue)
            {
                LevelFailedNoContinue();
            }
            else
            {
                LevelFailed();

            }
        }
        public void StartGame()
        {
            if (m_DebugEnabled)
            {
                Debug.Log(nameof(StartGame));
            }

            LevelStarted();
        }

    }
}