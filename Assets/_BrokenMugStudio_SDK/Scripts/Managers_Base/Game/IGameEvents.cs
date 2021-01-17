﻿public interface IGameEvents
{
    void GameReset();
    void LevelReset();
    void LevelLoaded();
    void LevelStarted();
    void PauseGame();
    void UnPauseGame();
    void LevelCompleted();
    void LevelContinue();
    void LevelFailed();
    void LevelFailedNoContinue();

}