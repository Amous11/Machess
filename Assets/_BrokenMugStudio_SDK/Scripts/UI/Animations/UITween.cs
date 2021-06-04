using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITween : MonoBehaviour
{
    public bool DoOnEnable = true;
    public bool KillOnEnable = false;
    public bool DisableOnComplete = false;
    public bool OnEnableSetAsLastSibling = false;
    public bool DoPosition = true;
    public bool DoScale = false;
    public bool DoColor = false;
    public float Delay = 0;
    public float Durration = 1;
    public int Loop = 1;
    public LoopType LoopType;
    public Ease EaseType = Ease.OutQuad;

    [ShowIf(nameof(DoScale))]
    public Vector3 InitialScale = Vector3.one * .1f;
    [ShowIf(nameof(DoScale))]
    public Vector3 TargetScale = Vector3.one;
    [ShowIf(nameof(DoPosition))]

    public Vector3 StartPosition;
    [ShowIf(nameof(DoPosition))]
    public bool XScreenSize = true;
    
    [ShowIf(nameof(DoColor))]
    public Color StartColor = Color.white;
    [ShowIf(nameof(DoColor))]
    public Color EndColor = Color.white;

    private Vector3 m_InitialPosition;

    private RectTransform m_RectTransform;
    private Image m_Image;


    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_InitialPosition = m_RectTransform.anchoredPosition3D;
        m_Image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        if(OnEnableSetAsLastSibling)
        {
            transform.SetAsLastSibling();
        }
        if(KillOnEnable)
        {
            transform.DOKill();
        }
        if(DoOnEnable)
        {
            if (DoPosition)
            {
                DoTweenMove();
            }
            if (DoScale)
            {
                DoTweenScaleToOne();
            }
            if(DoColor)
            {
                DoTweenColor();
            }
        }
       
        

    }
    public void DoTweenMove()
    {
            
        m_RectTransform.anchoredPosition3D = new Vector3(StartPosition.x * Screen.width, StartPosition.y * Screen.height) + m_InitialPosition;
        m_RectTransform.DOAnchorPos3D(m_InitialPosition, Durration).SetLoops(Loop, LoopType).SetEase(EaseType).SetDelay(Delay).OnComplete(OnTweenComplete);

    }
    public void DoTweenScaleToOne()
    {
        transform.localScale = InitialScale;
        transform.DOScale(TargetScale, Durration).SetLoops(Loop, LoopType).SetEase(EaseType).SetDelay(Delay).OnComplete(OnTweenComplete);
    }
    public void DoTweenColor()
    {
        if(m_Image!=null)
        {
            m_Image.color = StartColor;
            m_Image.DOColor(EndColor, Durration).SetLoops(Loop, LoopType).SetEase(EaseType).SetDelay(Delay).OnComplete(OnTweenComplete);
        }
        
    }

    public void OnTweenComplete()
    {
        if(DisableOnComplete)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        m_RectTransform.DOKill();
    }

    
}


