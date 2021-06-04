using BrokenMugStudioSDK;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Screen_InGame : MenuScreenBase
{
    
    [SerializeField]
    private RectTransform m_InputPanel;
    [SerializeField]

    private GameObject m_Joystick;
    [SerializeField]
    private RectTransform m_JoystickBG;
    [SerializeField]
    private RectTransform m_JoystickTip;
    [SerializeField]
    private Camera m_UICamera;

    private Vector2 m_InitialPosition;

    private Vector2 m_DummyLocalPoint;
    private Vector2 m_DummyDirection;
    [SerializeField]
    private float m_JoystickTipLimit = 70;
    [SerializeField]
    private TextMeshProUGUI m_LevelProgressText;
    [SerializeField]
    private Slider m_LevelProgressSlider; 
    [SerializeField]
    private TextMeshProUGUI m_LevelText;
    protected override void OnEnable()
    {
        base.OnEnable();
        m_Joystick.SetActive(false);
        InputManager.OnPointerDown += InputDown;
        InputManager.OnPointerUp += InputUp;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        InputManager.OnPointerDown -= InputDown;
        InputManager.OnPointerUp -= InputUp;
    }
    #region Input
    public void InputDown()
    {
        m_Joystick.SetActive(true);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_InputPanel, Input.mousePosition, m_UICamera, out m_InitialPosition);
        m_JoystickBG.anchoredPosition = m_InitialPosition;

    }

    public void InputUp()
    {
        m_Joystick.SetActive(false);
    }


    private void Update()
    {
        if(GameManager.Instance.GameState!=eGameState.Playing)
        {
            return;
        }

        if (InputManager.Instance.IsInputDown)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_InputPanel, Input.mousePosition, m_UICamera, out m_DummyLocalPoint);
            m_DummyDirection = m_DummyLocalPoint - m_InitialPosition;
            if (m_DummyDirection.magnitude > m_JoystickTipLimit)
            {
                m_DummyLocalPoint = m_InitialPosition + (m_DummyDirection.normalized * m_JoystickTipLimit);
            }
            m_JoystickTip.anchoredPosition = m_DummyLocalPoint;
            //m_JoystickTip.anchoredPosition = new Vector2(Mathf.Clamp(m_DummyLocalPoint.x,-70,70), Mathf.Clamp(m_DummyLocalPoint.y, -70, 70));
        }
    }
    
    #endregion

}
