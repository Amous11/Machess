using BrokenMugStudioSDK;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{

    
    private void OnEnable()
    {
        GameManager.OnLevelStarted += LevelStarted;
        //GetRefs();
    }

    private void OnDisable()
    {
        GameManager.OnLevelStarted -= LevelStarted;

    }
    public void LoadCharacters()
    {
        

    }
    public void LevelStarted()
    {
    }

    public void PlayerDied()
    {
        GameManager.Instance.GameOver();

    }
    public void LevelFinished()
    {
        GameManager.Instance.LevelCompleted();
    }
   
/*#if UNITY_EDITOR
    [Button]
    public void InitializeData()
    {
        Enemies = GetComponentsInChildren<Enemy>(true);
        //Characters = GetComponentsInChildren<CharacterBase>(true);
        EnemiesSpots = new Vector3[Enemies.Length];
        Player = GetComponentInChildren<Player>(true);
        
        for (int i = 0; i < Enemies.Length; i++)
        {
            Enemies[i].gameObject.name = "Enemy_" + (i + 1);
            EnemiesSpots[i] = Enemies[i].transform.position;
        }
    }

    
#endif*/
}
