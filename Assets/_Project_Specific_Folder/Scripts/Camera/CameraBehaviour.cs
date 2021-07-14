using BrokenMugStudioSDK;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : CameraBehaviourBase
{
    /*[SerializeField]
    private Player m_Player 
    { 
        get 
        {
            if(GameManager.Instance.CurrentLevel!=null)
            {
                return GameManager.Instance.CurrentLevel.Player;

            }
            return null;
        } 
    }*/
    private Transform m_Player;

    private void FixedUpdate()
    {
        if (m_Player != null)
        {
            FollowTarget(m_Player.transform);
            //handleZoom();
            if (CameraSettings.DoLookAt)
            {
                LookAt(m_Player.transform);
            }
            if (CameraSettings.DoRotateWithTarget)
            {
                RotateWithTarget(m_Player.transform);
            }
        }

    }

   /* private void handleZoom()
    {
        if (m_Player.RequireZoomOut)
        {
            ZoomTransform.localPosition = Vector3.forward * Mathf.Lerp(ZoomTransform.localPosition.z, CameraZoom.MaxZoom, Time.fixedDeltaTime * CameraZoom.ZoomSpeedOut);
        }
        else
        {
            ZoomTransform.localPosition = Vector3.forward * Mathf.Lerp(ZoomTransform.localPosition.z, CameraZoom.MinZoom, Time.fixedDeltaTime * CameraZoom.ZoomSpeedIn);

        }
    }*/
}



