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
    private TextMeshProUGUI m_ActionPointsText;

    protected override void OnEnable()
    {
        base.OnEnable();
       
        m_RollDiceButton.onClick.AddListener(RollDice);

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        m_RollDiceButton.RemoveListener(RollDice);
        
    }

    public void RollDice()
    {
        GameManager.Instance.RollDice();
        m_RollDiceButton.gameObject.SetActive(false);
    }

    public void UpdateActionPoints(int i_ActionPoints)
    {
        m_ActionPointsText.text = i_ActionPoints.ToString();
    }
    
}
