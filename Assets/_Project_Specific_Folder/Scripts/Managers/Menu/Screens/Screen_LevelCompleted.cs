using BrokenMugStudioSDK;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_LevelCompleted : MenuScreenBase
{
    [SerializeField]
    private ExtendedButton m_DoubleRvButton;
    [SerializeField]
    private Button m_NoThanksButton;
    private MenuVariablesEditor m_MenuVars { get => GameConfig.Instance.Menus; }
    private bool m_CanClickButtons;
    private int m_MoneyToReceive;
    [SerializeField] private MoneyContainerBase m_CoinGroup;
    private int m_MoneyRewardMultiplier { get { return 1; } }
    protected override void OnEnable()
    {
        base.OnEnable();
        m_DoubleRvButton.SetInteractable(AdsManager.Instance.IsRewardVideoLoaded);

        
        m_NoThanksButton.gameObject.SetActive(false);
        DOVirtual.DelayedCall(m_MenuVars.TimeToShowNoThanks, () => { m_NoThanksButton.gameObject.SetActive(true); });
        m_CanClickButtons = true;
        m_MoneyToReceive = (GameConfig.Instance.GamePlay.LevelCompleteMoneyReward* m_MoneyRewardMultiplier)+(GameManager.Instance.PlayerKillCount*GameConfig.Instance.GamePlay.BonusPerKillMoney);
        m_CoinGroup.SetMoney(m_MoneyToReceive, false);
        m_DoubleRvButton.onClick.AddListener(onDoubleRvClick);
        m_NoThanksButton.onClick.AddListener(onNoThanksClick);

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        m_DoubleRvButton.onClick.RemoveAllListeners();
        m_NoThanksButton.onClick.RemoveAllListeners();

    }
    private const string k_DoubleRv= "Double_RV";
    private void onDoubleRvClick()
    {
        if (!m_CanClickButtons)
            return;
        m_CanClickButtons = false;
        AdsManager.Instance.ShowRewardVideo(onDoubleRvSuccess, onDoubleRvFailed, k_DoubleRv);

        
    }
    private void onDoubleRvSuccess()
    {
        m_MoneyToReceive = m_MoneyToReceive*2;
        m_CoinGroup.SetMoney(m_MoneyToReceive, true);
        HUDManager.Instance.AnimateCoins(m_MoneyToReceive, m_CoinGroup.MoneyTarget, OnAnimationFinished);
        if (EnableDebug)
        {
            Debug.LogError("onDoubleRvSuccess");

        }
    }
    private void onDoubleRvFailed()
    {
        if (EnableDebug)
        {
            Debug.LogError("onDoubleRvFailed");
        }
        m_CanClickButtons = true;

    }
    private void onNoThanksClick()
    {

        if (!m_CanClickButtons)
            return;

        m_CanClickButtons = false;
       
        HUDManager.Instance.AnimateCoins(m_MoneyToReceive, m_CoinGroup.MoneyTarget, OnAnimationFinished);
    }
    private void OnAnimationFinished()
    {
        m_CanClickButtons = false;

        DOVirtual.DelayedCall(GameConfig.Instance.HUD.HideLevelCompleteDelay, GameManager.Instance.ResetGame);
    }
}
