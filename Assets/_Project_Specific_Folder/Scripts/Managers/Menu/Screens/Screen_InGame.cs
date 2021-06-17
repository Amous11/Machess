using BrokenMugStudioSDK;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Screen_InGame : MenuScreenBase
{
    [SerializeField]
    private Button m_RollDiceButton;
    [SerializeField]
    private Button m_EndTurnButton;
    [SerializeField]
    private TextMeshProUGUI m_ActionPointsText;
    private bool m_CanClickRoll = true;
    [SerializeField]
    private Image m_APIcon;
    [SerializeField]
    private Image m_TimerFill;

    protected override void OnEnable()
    {
        base.OnEnable();
       
        m_RollDiceButton.onClick.AddListener(RollDice);
        m_EndTurnButton.onClick.AddListener(EndTurn);
        OnTurnEnds();
        GameManager.OnTurnEnds += OnTurnEnds;

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        m_RollDiceButton.RemoveListener(RollDice);
        m_EndTurnButton.RemoveListener(EndTurn);
        GameManager.OnTurnEnds -= OnTurnEnds;

    }
    public void OnTurnEnds()
    {
        ResetTimer();
        m_APIcon.DOColor(GameConfig.Instance.GamePlay.PlayerColors[GameManager.Instance.CurrentPlayerIndex],.25f);
        m_TimerFill.DOColor(GameConfig.Instance.GamePlay.PlayerColors[GameManager.Instance.CurrentPlayerIndex], .25f);

    }
    public void ResetTimer()
    {
        m_TimerFill.DOKill();
        m_TimerFill.DOFillAmount(1, .25f).OnComplete(StartTimer);
    }
    public void StartTimer()
    {
        m_TimerFill.DOFillAmount(0, GameConfig.Instance.GamePlay.RoundDurration).OnComplete(TimeOut);
    }
    public void TimeOut()
    {
        EndTurn();
    }
    public void RollDice()
    {
        if (m_CanClickRoll)
        {
            m_CanClickRoll = false;
            m_RollDiceButton.interactable = m_CanClickRoll;
            GameManager.Instance.RollDice();
        }
    }

    public void ResetDice()
    {
        if (!m_CanClickRoll)
        {
            m_CanClickRoll = true;
            m_RollDiceButton.gameObject.SetActive(true);
            Dice.Instance.ShowDice();
        }
        m_RollDiceButton.interactable = m_CanClickRoll;

    }

    public void UpdateActionPoints(int i_ActionPoints)
    {
        m_ActionPointsText.text = i_ActionPoints.ToString();

        m_ActionPointsText.transform.DOPunchScale(Vector3.one * .1f, .2f, 1, 0);
    }

    public void EndTurn()
    {
        GameManager.Instance.EndTurn();
        ResetDice();
    }
    
}
