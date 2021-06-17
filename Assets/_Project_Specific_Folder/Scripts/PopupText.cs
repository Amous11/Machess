using BrokenMugStudioSDK;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Text;
    private PopupManager.TextPopUpSettings m_Settings { get => PopupManager.Instance.ScoreText; }
    private Camera m_Camera{ get => CameraBehaviour.Instance.MainCamera; }

    private Sequence m_Sequence;
    private Transform m_FollowTarget;
    [SerializeField]
    private RectTransform m_Rect;
   /* [Button]
    public void TestTween()
    {
        DoPopUp("+1", transform);
    }*/
    public void DoPopUp(string i_Text,Transform i_Target)
    {
        m_FollowTarget = i_Target;
        m_Text.text = i_Text;
        m_Text.transform.localScale = Vector3.zero;

        m_Text.color = m_Settings.StartColor;
        if(m_Sequence!=null)
        {
            m_Sequence.Kill();
        }
        m_Sequence = DOTween.Sequence();
        m_Sequence.Insert(0,m_Text.transform.DOScale(m_Settings.Scale, m_Settings.InDurration).SetEase(m_Settings.InEase));
        m_Sequence.Insert(0,m_Text.DOColor(m_Settings.StartColor, m_Settings.InDurration).SetEase(m_Settings.InEase));
        m_Sequence.Insert(m_Settings.InDurration, m_Text.DOColor(m_Settings.EndColor, m_Settings.OutDurration).SetEase(m_Settings.OutEase));
        m_Sequence.Insert(m_Settings.InDurration, m_Text.transform.DOScale(Vector3.one, m_Settings.OutDurration).SetEase(m_Settings.OutEase));
        m_Sequence.OnComplete(HideText);
    }
    private void Update()
    {
        if(m_FollowTarget!=null)
        {
            FollowTransformPos();
        }
    }
    private Vector2 m_ScreenPos;
    private Vector2 m_LocalPos;

    public void FollowTransformPos()
    {
        /* m_ScreenPos = CameraBehaviour.Instance.MainCamera.WorldToScreenPoint(m_FollowTarget.position);
         m_LocalPos = Vector2.zero;
         RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Rect, m_ScreenPos, m_Camera, out m_LocalPos);*/
        m_Text.rectTransform.position= RectTransformUtility.WorldToScreenPoint(m_Camera, m_FollowTarget.position);//  m_Camera.WorldToScreenPoint(m_FollowTarget.position);

        //m_Text.rectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(m_Camera, m_FollowTarget.position);//  m_Camera.WorldToScreenPoint(m_FollowTarget.position);
    }
    public void HideText()
    {
        m_FollowTarget = null;
        gameObject.SetActive(false);
    }

}
