using BrokenMugStudioSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_LevelFailedRevive : MenuScreenBase
{
    [SerializeField]
    private Button m_TapToRestartButton;
    [SerializeField]
    private Button m_ReviveButton;
    protected override void OnEnable()
    {
        base.OnEnable();
        m_TapToRestartButton.onClick.AddListener(onClickNoThanks);
        m_ReviveButton.onClick.AddListener(onClickRevive);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        m_TapToRestartButton.onClick.RemoveAllListeners();
        m_ReviveButton.onClick.RemoveAllListeners();

    }

    public void onClickNoThanks()
    {
        GameManager.Instance.ResetGame();
    }
    public void onClickRevive()
    {
        AdsManager.Instance.ShowRewardVideo(GameManager.Instance.LevelContinue, null, "Revive_RV");
    }
}

