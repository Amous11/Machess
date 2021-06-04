using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    public class CameraBehaviourBase : Singleton<CameraBehaviour>
    {
        public CameraMouvementsData CameraSettings { get { return GameConfig.Instance.Camera.CameraMouvements; } }
        public CameraZoomData CameraZoom { get { return GameConfig.Instance.Camera.ZoomData; } }
        public CameraPositionsData Positions { get { return GameConfig.Instance.Camera.CameraPositions; } }

       
        [SerializeField]
        private Camera m_Camera;
        public Camera MainCamera { get { return m_Camera; } }

        public Transform RotatorTransform;
        public Transform HandTransform;
        public Transform ZoomTransform;
        public bool CameraInPosition;

        public override void Start()
        {
            base.Start();
            if (m_Camera == null)
            {
                m_Camera = gameObject.GetComponentInChildren<Camera>();
            }
        }
        private void OnEnable()
        {
            resetCamera();
            GameManager.OnLevelLoaded += resetCamera;

            GameManager.OnGameStateChange += gameStateChangeCallback;

        }

        public override void OnDisable()
        {
            base.OnDisable();
            GameManager.OnLevelLoaded -= resetCamera;

            GameManager.OnGameStateChange -= gameStateChangeCallback;
           
        }

        private void resetCamera()
        {

        }
        
        private float m_DummyPosX;
        private float m_DummyPosY;
        private float m_DummyPosZ;

        public void FollowTarget(Transform i_Target)
        {
            //transform.position = Vector3.Lerp(transform.position, m_Player.position, m_CameraSettings.MouvementSpeed * Time.deltaTime);
            m_DummyPosX = Mathf.Lerp(transform.position.x, i_Target.position.x, CameraSettings.MouvementSpeed * Time.fixedDeltaTime * CameraSettings.MouvementSpeedVectorMultiply.x);
            m_DummyPosZ = Mathf.Lerp(transform.position.z, i_Target.position.z, CameraSettings.MouvementSpeed * Time.fixedDeltaTime * CameraSettings.MouvementSpeedVectorMultiply.z);
            m_DummyPosY = Mathf.Lerp(transform.position.y, i_Target.position.y, CameraSettings.MouvementSpeed * Time.fixedDeltaTime * CameraSettings.MouvementSpeedVectorMultiply.y);


            transform.position = new Vector3(m_DummyPosX, m_DummyPosY, m_DummyPosZ);

        }
        

        private Quaternion m_DummyLookRotation;
        public void LookAt(Transform i_Target)
        {
            m_DummyLookRotation = Quaternion.LookRotation(((i_Target.position.SetPositionX(m_Camera.transform.position.x) + CameraSettings.LookAtOffset) - m_Camera.transform.position).normalized);
            m_Camera.transform.rotation = Quaternion.Slerp(m_Camera.transform.rotation, m_DummyLookRotation, CameraSettings.LookAtSpeed * Time.deltaTime);
        }

        public void RotateWithTarget(Transform i_Target)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, i_Target.rotation, CameraSettings.RotateWithTargetSpeed * Time.deltaTime);
        }
        private void gameStateChangeCallback()
        {
            SetPosition(GameManager.Instance.GameState);
        }
        public void SetPosition(eGameState i_GameState)
        {
            CameraInPosition = false;
            ApplyPosition(Positions.GetCameraPosition(i_GameState));
        }

        public void ApplyPosition(CameraPositions i_Position)
        {
            RotatorTransform.DOKill();
            HandTransform.DOKill();
            RotatorTransform.DOLocalRotate(i_Position.RotatorRotation, i_Position.Durration).SetEase(i_Position.EaseType).SetDelay(i_Position.Delay);
            HandTransform.DOLocalMove(i_Position.HandPosition, i_Position.Durration).SetEase(i_Position.EaseType).SetDelay(i_Position.Delay).OnComplete(OnCompleteTween);

        }
        public void OnCompleteTween()
        {
            CameraInPosition = true;
        }
        private Vector2 m_DummyScreenPoint;
        public bool IsOnScreen(Vector3 i_Position)
        {

            m_DummyScreenPoint = m_Camera.WorldToScreenPoint(i_Position);
            if (
                (m_DummyScreenPoint.x <= 0 || m_DummyScreenPoint.x >= Screen.width)
                ||
                (m_DummyScreenPoint.y <= 0 || m_DummyScreenPoint.y >= Screen.height)
              )
            {
                return false;

            }
            return true;
        }
        #region Camera Shake
        private Coroutine m_ShakeCoroutine;
        public void DoShakeCoroutine(CameraShakeData i_Shake)
        {
            if(m_ShakeCoroutine!=null)
            {
                StopCoroutine(m_ShakeCoroutine);
            }
            m_ShakeCoroutine = StartCoroutine(IEShakeCoroutine(i_Shake));
        }
        private float m_TimeLeft;
        private float m_PositionShakeMagnitude;
        private float m_RotationShakeMagnitude;

        private float m_ShakeX;
        private float m_ShakeY;
        private float m_ShakeZ;


        IEnumerator IEShakeCoroutine(CameraShakeData i_Shake)
        {
            m_TimeLeft = i_Shake.Durration;
            m_Camera.transform.localPosition = Vector3.zero;
            m_Camera.transform.localEulerAngles = Vector3.zero;
            while (m_TimeLeft>0)
            {
                if(i_Shake.ShakePosition)
                {
                    m_PositionShakeMagnitude = i_Shake.EaseCurve.Evaluate(m_TimeLeft / i_Shake.Durration) * i_Shake.PositionMagnitude;
                    m_ShakeX = m_PositionShakeMagnitude * UnityEngine.Random.Range(-1f, 1f)* i_Shake.PositionAxis.x;
                    m_ShakeY = m_PositionShakeMagnitude * UnityEngine.Random.Range(-1f, 1f) * i_Shake.PositionAxis.y;
                    m_ShakeZ = m_PositionShakeMagnitude * UnityEngine.Random.Range(-1f, 1f) * i_Shake.PositionAxis.z;
                    m_Camera.transform.localPosition = new Vector3(m_ShakeX, m_ShakeY, m_ShakeZ);
                }
                if (i_Shake.ShakeRotation)
                {
                    m_RotationShakeMagnitude = i_Shake.EaseCurve.Evaluate(m_TimeLeft / i_Shake.Durration) * i_Shake.RotationMagnitude;
                    m_ShakeX = m_RotationShakeMagnitude * UnityEngine.Random.Range(-1f, 1f) * i_Shake.RotationAxis.x;
                    m_ShakeY = m_RotationShakeMagnitude * UnityEngine.Random.Range(-1f, 1f) * i_Shake.RotationAxis.y;
                    m_ShakeZ = m_RotationShakeMagnitude * UnityEngine.Random.Range(-1f, 1f) * i_Shake.RotationAxis.z;
                    m_Camera.transform.localEulerAngles = new Vector3(m_ShakeX, m_ShakeY, m_ShakeZ);
                }
                yield return new WaitForEndOfFrame();
                m_TimeLeft -= Time.deltaTime;
            }
            yield return null;
            m_Camera.transform.localPosition = Vector3.zero;
            m_Camera.transform.localEulerAngles = Vector3.zero;

        }
        #endregion
    }
    #region DataClasses
    [Serializable]
    public class CameraMouvementsData
    {
        public float MouvementSpeed = 5;
        public Vector3 MouvementSpeedVectorMultiply = Vector3.one;
        public bool DoLookAt;
        [ShowIf(nameof(DoLookAt))]
        public Vector3 LookAtOffset;
        [ShowIf(nameof(DoLookAt))]
        public float LookAtSpeed = 10;
        public bool DoRotateWithTarget;
        [ShowIf(nameof(DoRotateWithTarget))]
        public float RotateWithTargetSpeed = 10;


    }
    [Serializable]
    public class CameraPositionsData
    {

        public CameraPositions[] CameraPositions;
        public CameraPositions GetCameraPosition(eGameState i_GameState)
        {
            for (int i = 0; i < CameraPositions.Length; i++)
            {
                if (CameraPositions[i].GameState == i_GameState)
                {
                    return CameraPositions[i];
                }

            }

            return null;
        }
    }
    [Serializable]
    public class CameraPositions
    {
        public eGameState GameState;
        public Vector3 RotatorRotation;
        public Vector3 HandPosition;
        public float Delay = .3f;
        public float Durration = .5f;
        public Ease EaseType = Ease.OutSine;

    }

    [Serializable]
    public class CameraZoomData
    {
        public float ZoomSpeedIn = 2f;
        public float ZoomSpeedOut = 5f;

        public float MinZoom = -6.6f;
        public float MaxZoom = -10;

    }
    [Serializable]
    public class CameraShakeData
    {
        public float PositionMagnitude = .1f;
        public float RotationMagnitude = 5f;
        public float Durration = .2f;
        public bool ShakeRotation;
        public bool ShakePosition = true;
        public Vector3 PositionAxis = Vector3.right + Vector3.up;
        public Vector3 RotationAxis = Vector3.one;

        public AnimationCurve EaseCurve;
    }
    #endregion
}
