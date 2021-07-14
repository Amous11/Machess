using BrokenMugStudioSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_LevelFailed : MenuScreenBase
{
    [SerializeField]
    private Button m_TapToRestart;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_TapToRestart.onClick.AddListener(onClickNoThanks);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        m_TapToRestart.onClick.RemoveAllListeners();

    }

    public void onClickNoThanks()
    {
        GameManager.Instance.ResetGame();
    }
}
