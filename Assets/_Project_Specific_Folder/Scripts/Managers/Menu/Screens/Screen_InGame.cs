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
    private Button m_MenuButton;
    [SerializeField]
    private Button m_MenuButton2;
    [SerializeField]
    private Button m_ExitButton;
    [SerializeField]
    private GameObject m_MenuGO;
    [SerializeField] 
    private Slider m_VolumeSlider;
    [SerializeField] 
    private GameObject m_WinPanel;
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
        m_MenuButton.onClick.AddListener(BringMenu);
        m_MenuButton2.onClick.AddListener(BringMenu);
        m_ExitButton.onClick.AddListener(ExitGame);
        m_VolumeSlider.onValueChanged.AddListener(ChangeVolume);
        OnTurnEnds();
        GameManager.OnTurnEnds += OnTurnEnds;

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        m_RollDiceButton.RemoveListener(RollDice);
        m_EndTurnButton.RemoveListener(EndTurn);
        m_MenuButton.onClick.RemoveListener(BringMenu);
        m_MenuButton2.onClick.RemoveListener(BringMenu);
        m_ExitButton.onClick.RemoveListener(ExitGame);
        GameManager.OnTurnEnds -= OnTurnEnds;

    }
    public void BringMenu()
    {
        if (m_MenuGO.IsActive())
        {
            m_MenuGO.SetActive(false);
        }
        else
        {
            m_MenuGO.SetActive(true);
        }
            
    }

    public void ExitGame()
    {
        GameManager.Instance.ResetGame();
    }
    public void ChangeVolume(float i_Value)
    {
        AudioListener.volume = i_Value;
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

    public void VictoryPanel()
    {

    }
    
}
