using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : InputManagerBase
{

    public delegate void InputEvent();
    public static event InputEvent OnPointerDown = delegate { };
    public static event InputEvent OnPointerUp = delegate { };

    public bool IsInputDown { get { return m_IsInputDown; } }
    private bool m_IsInputDown;

    private Vector3 m_InputDownPos;
    private Vector3 m_LastInputPos;

    public Vector3 Drag3D
    {
        get { return new Vector3(Drag.x, 0, Drag.y); }
    }
    public Vector2 DeltaDrag { get { return m_DeltaDrag * GameConfig.Instance.Input.DragSensitivity * GetScreenRatio(); } }
    private Vector2 m_DeltaDrag;

    public Vector2 Drag { get { return m_Drag * GameConfig.Instance.Input.DragSensitivity * GetScreenRatio(); } }
    private Vector2 m_Drag;

    private float ScreenXRatio;
    private float ScreenYRatio;

    private float ScreenPhysicalSize = 0;
    private Vector2 ScreenRatio = new Vector2(0, 0);
    private float m_PointerDownTime;
    private bool m_IsTap;
    #region Screen calculations
    private float GetScreenPhysicalSize()
    {
        if (ScreenPhysicalSize == 0)
        {
            ScreenPhysicalSize = ((float)1f / Screen.dpi) * Screen.width;
        }
        return ScreenPhysicalSize;
    }

    private Vector2 GetScreenRatio()
    {
        if (ScreenRatio == Vector2.zero)
        {
            ScreenXRatio = Screen.width / GetScreenPhysicalSize();
            ScreenYRatio = Screen.height / GetScreenPhysicalSize();
#if UNITY_EDITOR
           /* ScreenXRatio = Screen.width / (GetScreenPhysicalSize() * GameViewEditorRes.GetGameViewScale().x);
            ScreenYRatio = Screen.height / (GetScreenPhysicalSize() * GameViewEditorRes.GetGameViewScale().y);
            if (ScreenXRatio == 0 || ScreenYRatio == 0)
            {
                ScreenXRatio = Screen.width / ((((float)1f / 96) * Screen.width) * GameViewEditorRes.GetGameViewScale().x);
                ScreenYRatio = Screen.height / ((((float)1f / 96) * Screen.width) * GameViewEditorRes.GetGameViewScale().y);
            }*/
#endif
            if (ScreenXRatio == 0 || ScreenYRatio == 0)
            {
                ScreenXRatio = Screen.width / (((float)1f / 350) * Screen.width);
                ScreenYRatio = Screen.height / (((float)1f / 350) * Screen.height);
            }

            //Normalization
            ScreenRatio = Vector2.right * ScreenXRatio * .001f + Vector2.up * ScreenYRatio * .001f;
        }


        return ScreenRatio;
    }
    #endregion

    private void ResetValues()
    {
        m_IsInputDown = false;
        m_DeltaDrag = m_Drag = Vector2.zero;
    }

    public static bool WasDebugKeyPressed = false;
    private void OnEnable()
    {
        GameManager.OnLevelStarted += ResetValues;
        GameManager.OnLevelContinue += ResetValues;

    }
    public override void OnDisable()
    {
        base.OnDisable();
        GameManager.OnLevelStarted -= ResetValues;
        GameManager.OnLevelContinue -= ResetValues;

    }
    public void Update()
    {
       

        if (m_IsInputDown)
        {
            m_Drag = Input.mousePosition - m_InputDownPos;
            if (m_Drag.sqrMagnitude < (GameConfig.Instance.Input.InputThreshold * GetScreenRatio().x))
            {
                m_Drag = Vector2.zero;
            }
            m_DeltaDrag = Input.mousePosition - m_LastInputPos;

            m_LastInputPos = Input.mousePosition;
        }
    }

    public void InputDown()
    {
        ResetValues();
        m_PointerDownTime = Time.time;
        m_IsInputDown = true;
        m_InputDownPos = m_LastInputPos = Input.mousePosition;
        if (OnPointerDown != null)
        {
            OnPointerDown?.Invoke();
        }
    }


    public void InputUp()
    {
        ResetValues();
        m_IsInputDown = false;

        if (OnPointerUp != null)
        {
            OnPointerUp?.Invoke();
        }
    }

}
