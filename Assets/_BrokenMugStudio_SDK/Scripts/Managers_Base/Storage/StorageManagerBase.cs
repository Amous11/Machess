using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManagerBase : Singleton<StorageManager>
{
    public int CurrentLevel
    {
        get
        {
            return PlayerPrefs.GetInt(nameof(CurrentLevel), 0);
        }
        set
        {
            PlayerPrefs.SetInt(nameof(CurrentLevel), value);
            if (value > HighScoreLevel) 
            {
                HighScoreLevel = value;
            }
        }
    }
    public int HighScoreLevel { get { return PlayerPrefs.GetInt(nameof(HighScoreLevel), 0); } set { PlayerPrefs.SetInt(nameof(HighScoreLevel), value); } }
    [Title("Currency")]

    [ShowInInspector, PropertyOrder(0)] 
    public int PlayerMoney { get { return PlayerPrefs.GetInt(nameof(PlayerMoney), 0); } set { PlayerPrefs.SetInt(nameof(PlayerMoney), value); OnMoneyAmountChanged.Invoke(value); } }
    public delegate void CoinsAmountChangedEvent(int i_CoinsAmount);
    [ShowInInspector, PropertyOrder(1)]
    public bool IsVibrationOn { get { return PlayerPrefs.GetInt(nameof(IsVibrationOn), 1)==1; } set { PlayerPrefs.SetInt(nameof(IsVibrationOn), value == true ? 1 : 0); } }
    [ShowInInspector, PropertyOrder(2)]
    public bool IsSoundEffectsOn { get { return PlayerPrefs.GetInt(nameof(IsSoundEffectsOn), 1) == 1; } set { PlayerPrefs.SetInt(nameof(IsSoundEffectsOn), value == true ? 1 : 0); } }
    [ShowInInspector, PropertyOrder(3)]
    public bool IsMusicOn { get { return PlayerPrefs.GetInt(nameof(IsMusicOn), 1) == 1; } set { PlayerPrefs.SetInt(nameof(IsMusicOn), value == true ? 1 : 0); } }
    public bool HasRatedGame { get { return PlayerPrefs.GetInt(nameof(HasRatedGame), 0) == 1; } set { PlayerPrefs.SetInt(nameof(HasRatedGame), value == true ? 1 : 0); } }

    public static event CoinsAmountChangedEvent OnMoneyAmountChanged = delegate { };
}
