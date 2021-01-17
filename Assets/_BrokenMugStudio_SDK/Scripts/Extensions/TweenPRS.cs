using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenPRS : MonoBehaviour
{
    public bool DoPosition = true;
    public bool DoRotation = false;
    public bool DoScale = false;
    public float Durration=.5f;
    [ShowIf(nameof(DoPosition))]
    public Vector3 TargetPosition;
    [ShowIf(nameof(DoRotation))]
    public Vector3 TargetEuler;
    [ShowIf(nameof(DoScale))]
    public Vector3 TargetScale;

    private Vector3 m_InitialPosition;

    private Vector3 m_InitialScale;
    private Vector3 m_InitialEuler;

    public int Loops = -1;
    public LoopType LoopType;

    public Ease EaseType;
    public float Delay = 0;



    public void OnEnable()
    {
        TweenIt();
    }

    public void TweenIt()
    {
        if(DoPosition)
        {
            m_InitialPosition = transform.localPosition;
            transform.localPosition = m_InitialPosition + TargetPosition;
            transform.DOLocalMove(m_InitialPosition, Durration)
                        .SetEase(EaseType)
                        .SetLoops(Loops, LoopType)
                        .SetDelay(Delay);
        }
        if (DoRotation)
        {
            m_InitialEuler = transform.localEulerAngles;
            //transform.localEulerAngles = m_InitialEuler + TargetEuler;
            transform.DOLocalRotate(TargetEuler, Durration,RotateMode.LocalAxisAdd)
                        .SetEase(EaseType)
                        .SetLoops(Loops, LoopType)
                        .SetDelay(Delay);
        }
        if (DoScale)
        {
            m_InitialScale = transform.localScale;
            transform.localScale =TargetScale;
            transform.DOScale(m_InitialScale, Durration)
                        .SetEase(EaseType)
                        .SetLoops(Loops, LoopType)
                        .SetDelay(Delay);
        }


    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
