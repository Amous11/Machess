using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameManagerBase
{
    protected override void OnAwakeEvent()
    {
        base.OnAwakeEvent();
        StorageManagerBase.Instance.CurrentLevel = StorageManagerBase.Instance.HighScoreLevel;

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

    }
    public override void LevelStarted()
    {
        base.LevelStarted();

    }
    public override void ResetGame()
    {
        base.ResetGame();

    }

    
    public override void LevelContinue()
    {
        base.LevelContinue();
    }

    public override void LevelFailed()
    {
        base.LevelFailed();

    }

}
