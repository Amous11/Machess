using BrokenMugStudioSDK;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField]
    private PopupText[] m_PopupTexts;
    public TextPopUpSettings ScoreText;

    private int m_TextIndex;
    public void ShowPopupText(string i_Text,Transform i_FollowTransform)
    {
        m_PopupTexts[m_TextIndex].gameObject.SetActive(true);
        m_PopupTexts[m_TextIndex].DoPopUp(i_Text, i_FollowTransform);
        m_TextIndex++;
        if(m_TextIndex>= m_PopupTexts.Length)
        {
            m_TextIndex = 0;
        }
    }
    [Serializable]
    public class TextPopUpSettings
    {
        public float InDurration = 1;
        public float OutDurration = 1;

        public Vector3 Scale = Vector3.one * 1.1f;
        public Ease InEase = Ease.OutBack;
        public Ease OutEase = Ease.Linear;

        public Color StartColor = Color.white;
        public Color EndColor = Color.white.SetAlpha(.5f);


    }
#if UNITY_EDITOR
    [Button]
    public void SetRefs()
    {
        m_PopupTexts = GetComponentsInChildren<PopupText>(true);
        for(int i=0;i< m_PopupTexts.Length;i++)
        {
            m_PopupTexts[i].gameObject.SetActive(false);
        }
    }
#endif
}
