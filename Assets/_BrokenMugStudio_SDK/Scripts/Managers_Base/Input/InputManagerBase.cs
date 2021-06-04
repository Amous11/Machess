using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;
#endif
using UnityEngine;
namespace BrokenMugStudioSDK
{
    public class InputManagerBase : Singleton<InputManager>
    {
        public delegate void InputEvent();
        public static event InputEvent OnPointerDown = delegate { };
        public static event InputEvent OnPointerUp = delegate { };
        public bool IsUsingGamePad = false;
        public virtual Vector3 TouchPosition
        {
            get
            {
                return Input.mousePosition;
            }
        }
        public virtual Vector3 Drag3D
        {
            get { return new Vector3(Drag.x, 0, Drag.y); }
        }
        public virtual Vector2 Drag2D
        {
            get { return m_Drag; }

        }
        public virtual bool IsInputDown { get { return m_IsInputDown; } }
        [SerializeField, ReadOnly]
        private bool m_IsInputDown;

        private Vector3 m_InputDownPos;
        public Vector3 TapStartPosition { get { return m_InputDownPos; } }
        private Vector3 m_LastInputPos;


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
                ScreenXRatio = Screen.width / (GetScreenPhysicalSize() * GameViewEditorRes.GetGameViewScale().x);
                ScreenYRatio = Screen.height / (GetScreenPhysicalSize() * GameViewEditorRes.GetGameViewScale().y);
                if (ScreenXRatio == 0 || ScreenYRatio == 0)
                {
                    ScreenXRatio = Screen.width / ((((float)1f / 96) * Screen.width) * GameViewEditorRes.GetGameViewScale().x);
                    ScreenYRatio = Screen.height / ((((float)1f / 96) * Screen.width) * GameViewEditorRes.GetGameViewScale().y);
                }
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
            IsUsingGamePad = false;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            GameManager.OnLevelStarted -= ResetValues;
            GameManager.OnLevelContinue -= ResetValues;

        }
        public virtual void Update()
        {

            #region Debug
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.ResetGame();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StorageManager.Instance.CurrentLevel++;
                GameManager.Instance.ResetGame();

            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (StorageManager.Instance.CurrentLevel > 0)
                {
                    StorageManager.Instance.CurrentLevel--;
                }
                GameManager.Instance.ResetGame();

            }
#endif
            #endregion

            if (m_IsInputDown)
            {
                m_Drag = TouchPosition - m_InputDownPos;
                if (m_Drag.sqrMagnitude < (GameConfig.Instance.Input.InputThreshold * GetScreenRatio().x))
                {
                    m_Drag = Vector2.zero;
                }
                m_DeltaDrag = TouchPosition - m_LastInputPos;

                m_LastInputPos = TouchPosition;
            }

        }

        public virtual void InputDown()
        {
            ResetValues();

            m_PointerDownTime = Time.time;
            m_IsInputDown = true;
            m_InputDownPos = m_LastInputPos = TouchPosition;
            if (OnPointerDown != null)
            {
                OnPointerDown?.Invoke();
            }
        }


        public virtual void InputUp()
        {
            ResetValues();
            m_IsInputDown = false;
            if (OnPointerUp != null)
            {
                OnPointerUp?.Invoke();
            }
        }
    }
    public class GameViewEditorRes
    {
#if UNITY_EDITOR

        public static Vector2 GetGameViewScale()
        {
            Type gameViewType = GetGameViewType();
            EditorWindow gameViewWindow = GetGameViewWindow(gameViewType);

            if (gameViewWindow == null)
            {
                Debug.LogError("GameView is null!");
                return Vector2.one;
            }

            var defScaleField = gameViewType.GetField("m_defaultScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            //whatever scale you want when you click on play
            //float defaultScale = 0.1f;

            var areaField = gameViewType.GetField("m_ZoomArea", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var areaObj = areaField.GetValue(gameViewWindow);

            var scaleField = areaObj.GetType().GetField("m_Scale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return (Vector2)scaleField.GetValue(areaObj);
            ////scaleField.SetValue(areaObj, new Vector2(defaultScale, defaultScale));
        }


        private static Type GetGameViewType()
        {
            Assembly unityEditorAssembly = typeof(EditorWindow).Assembly;
            Type gameViewType = unityEditorAssembly.GetType("UnityEditor.GameView");
            return gameViewType;
        }
        public static Vector2 GetMainGameViewSize()
        {
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)Res;
        }
        private static EditorWindow GetGameViewWindow(Type gameViewType)
        {
            Object[] obj = Resources.FindObjectsOfTypeAll(gameViewType);
            if (obj.Length > 0)
            {
                return obj[0] as EditorWindow;
            }
            return null;
        }
#endif

    }
}
