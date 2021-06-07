﻿using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    public class GameManager : GameManagerBase
    {
        public static event LevelEvent OnDiceRoll = delegate { };

        private GameObject m_CurrentLevelInstance;
        [ReadOnly]
        public Level CurrentLevel;

        public int CurrentPlayerIndex;

        protected override void OnAwakeEvent()
        {
            base.OnAwakeEvent();
            StorageManagerBase.Instance.CurrentLevel = StorageManagerBase.Instance.HighScoreLevel;
#if USING_ADDRESSABLES
            LoadingManager.Instance.PreLoadStuff(GameConfig.Instance.Levels.PreloadAssets);
#endif
        }
        public override void OnEnable()
        {
            base.OnEnable();

        }
        public override void OnDisable()
        {
            base.OnDisable();

        }
        public override void LevelLoaded()
        {
            base.LevelLoaded();
#if USING_ADDRESSABLES
            LoadingManager.Instance.LoadLevel();
#else
            if(m_CurrentLevelInstance!=null)
            {
                Destroy(m_CurrentLevelInstance);
            }
            if (GameConfig.Instance.Levels.CurrentLevel != null)
            {
                m_CurrentLevelInstance = Instantiate(GameConfig.Instance.Levels.CurrentLevel.gameObject);
                CurrentLevel = m_CurrentLevelInstance.GetComponent<Level>();
            }
#endif
        }
        public override void LevelLoadComplete()
        {
            base.LevelLoadComplete();
        }
        public override void LevelStarted()
        {
            base.LevelStarted();
            MenuManager.Instance.GetInGameScreen().UpdateActionPoints(0);
        }

        public void LevelInstanciated(Level i_Level)
        {
            CurrentLevel = i_Level;
            LevelLoadComplete();
        }

        public override void ResetGame()
        {
            base.ResetGame();

        }


        public override void LevelContinue()
        {
            base.LevelContinue();
        }
        public override void LevelCompleted()
        {
            base.LevelCompleted();
            StorageManager.Instance.CurrentLevel++;
        }
        public override void LevelFailed()
        {
            base.LevelFailed();

        }
        public void RollDice()
        {
            if(OnDiceRoll!=null)
            {
                OnDiceRoll?.Invoke();
            }
        }

        
    }
}
