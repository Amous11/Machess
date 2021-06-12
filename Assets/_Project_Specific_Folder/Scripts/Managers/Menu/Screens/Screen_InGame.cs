using BrokenMugStudioSDK;
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

    protected override void OnEnable()
    {
        base.OnEnable();
       
        m_RollDiceButton.onClick.AddListener(RollDice);
        m_EndTurnButton.onClick.AddListener(EndTurn);

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        m_RollDiceButton.RemoveListener(RollDice);
        m_EndTurnButton.RemoveListener(EndTurn);
    }

    public void RollDice()
    {
        if (m_CanClickRoll)
        {
            m_CanClickRoll = false;
            GameManager.Instance.RollDice();
        }
    }

    public void ResetDice()
    {
        if (!m_CanClickRoll)
        {
            m_CanClickRoll = true;
            m_RollDiceButton.gameObject.SetActive(true);
            Dice.Instance.gameObject.SetActive(true);
        }
        
    }

    public void UpdateActionPoints(int i_ActionPoints)
    {
        m_ActionPointsText.text = i_ActionPoints.ToString();
    }

    public void EndTurn()
    {
        GameManager.Instance.EndTurn();
        ResetDice();
    }
    
}
