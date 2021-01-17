using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBase : Singleton<GameManagerBase>,IGameEvents
{
    public delegate void GameEvent();
    public static event GameEvent OnGameReset = delegate { };
    public static event GameEvent OnGamePause = delegate { };
    public static event GameEvent OnGameUnPause = delegate { };
    public static event GameEvent OnLevelReset = delegate { }; 

    public delegate void LevelEvent();
    public static event LevelEvent OnLevelContinue = delegate { };
    public static event LevelEvent OnLevelStarted = delegate { };
    public static event LevelEvent OnLevelCompleted = delegate { };
    public static event LevelEvent OnLevelLoaded = delegate { };
    public static event LevelEvent OnLevelFailed = delegate { };
    public static event LevelEvent OnLevelFailedNoContinue = delegate { };
    protected override void OnAwakeEvent()
    {
        base.OnAwakeEvent();
        StorageManagerBase.Instance.CurrentLevel = StorageManagerBase.Instance.HighScoreLevel;
        Application.targetFrameRate = 60;

    }
    public override void Start()
    {
        base.Start();

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
    }
    public virtual void LevelLoaded()
    {
        if (OnLevelLoaded != null)
        {
            OnLevelLoaded.Invoke();
        }
    }
    public virtual void LevelStarted()
    {
    }
    public virtual void LevelContinue()
    {
        
    }


    public void GameReset()
    {
    }

    public void LevelReset()
    {
    }

    

    public virtual void PauseGame()
    {
    }

    public virtual void UnPauseGame()
    {
    }

    public virtual void LevelCompleted()
    {
    }

    public virtual void LevelFailed()
    {
    }

    public virtual void LevelFailedNoContinue()
    {

    }
}
