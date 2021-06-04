using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace BrokenMugStudioSDK
{
    public class MenuScreenBase : MonoBehaviour
    {
        public bool HideOnReset = true;
        public bool EnableDebug { get { return GameConfig.Instance.Debug.IsDebugMode; } }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }


        public virtual void Reset()
        {
            gameObject.SetActive(!HideOnReset);
        }

        #region Open
        public virtual void Open()
        {
            OnScreenOpenStart();
        }

        protected virtual void OnScreenOpenStart()
        {
            MenuManager.Instance.OnMenu_ScreenOpenStart(this);

            OpenAnim();
        }

        protected virtual void OpenAnim()
        {
            gameObject.SetActive(true);

            OnScreenOpenEnd();
        }

        protected virtual void OnScreenOpenEnd()
        {
            MenuManager.Instance.OnMenu_ScreenOpenEnd(this);
        }
        #endregion

        #region Close
        public virtual void Close()
        {
            OnScreenCloseStart();
        }

        protected virtual void OnScreenCloseStart()
        {
            MenuManager.Instance.OnMenu_ScreenCloseStart(this);

            CloseAnim();
        }

        protected virtual void CloseAnim()
        {
            gameObject.SetActive(false);

            OnScreenCloseEnd();
        }

        protected virtual void OnScreenCloseEnd()
        {
            MenuManager.Instance.OnMenu_ScreenCloseEnd(this);
        }
        #endregion
        #region Debug
        [Button, BoxGroup("Debug")]
        public virtual void OpenDebug()
        {
            if (Application.isPlaying)
            {
                Open();
            }
        }
        [Button, BoxGroup("Debug")]
        public virtual void CloseDebug()
        {
            if (Application.isPlaying)
            {
                Close();
            }
        }
        #endregion
    }
}