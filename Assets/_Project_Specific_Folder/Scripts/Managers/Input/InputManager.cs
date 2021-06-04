
using UnityEngine;
namespace BrokenMugStudioSDK
{
    public class InputManager : InputManagerBase
    {
        public static event InputEvent OnActionButtonDown = delegate { };
        public static event InputEvent OnActionButtonUp = delegate { };
       
        private bool m_DoDebug { get { return GameConfig.Instance.Debug.IsDebugMode; } }
        private Vector2 m_KeyboardInput = Vector2.zero;

#if UNITY_EDITOR
        public override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ActionButtonDown();

            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ActionButtonUp();

            }
            m_KeyboardInput = (Vector2.up * Input.GetAxis("Vertical")) + (Input.GetAxis("Horizontal") * Vector2.right);
        }
#endif

        public void ActionButtonDown()
        {
            
            if (OnActionButtonDown != null)
            {
                OnActionButtonDown?.Invoke();
            }
        }
        public void ActionButtonUp()
        {
            
            if (OnActionButtonUp != null)
            {
                OnActionButtonUp?.Invoke();
            }
        }

    }

}