using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManagerBase : Singleton<StorageManagerBase>
{
    public int CurrentLevel { get { return PlayerPrefs.GetInt(nameof(CurrentLevel), 0); } set { PlayerPrefs.SetInt(nameof(CurrentLevel), value); } }
    public int Money { get { return PlayerPrefs.GetInt(nameof(Money), 0); } set { PlayerPrefs.SetInt(nameof(Money), value); } }
    public int HighScoreLevel { get { return PlayerPrefs.GetInt(nameof(HighScoreLevel), 0); } set { PlayerPrefs.SetInt(nameof(HighScoreLevel), value); } }
}
