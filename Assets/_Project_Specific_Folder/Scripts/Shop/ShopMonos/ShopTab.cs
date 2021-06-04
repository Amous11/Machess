using BrokenMugStudioSDK;
using BrokenMugStudioSDK.Shop;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTab : MonoBehaviour
{
    private eItemType m_TabType { get { return (eItemType)transform.GetSiblingIndex(); } }
    [SerializeField]
    private Button m_TabButton;
    [SerializeField]
    private Image m_TabIcon;
    [SerializeField]
    private Image m_SelectedImage;
    private bool m_IsSelected { get { return ShopManager.Instance.SelectedTab == m_TabType; } }
    [SerializeField]
    private RectTransform m_IsSelectedRect;
    [SerializeField]
    private RectTransform m_IsSelectedRectON;
    [SerializeField]
    private RectTransform m_IsSelectedRectOFF;

    private void OnEnable()
    {
        m_TabIcon.sprite = GameConfig.Instance.Shop.GetItemTypeData((eItemType)transform.GetSiblingIndex()).TabIcon;

        m_TabButton.onClick.AddListener(OnTabClicked);
        ShopManager.OnTabChanged += InitializeTab;
        InitializeTab();
    }
    private void OnDisable()
    {
        m_TabButton.onClick.RemoveAllListeners();
        ShopManager.OnTabChanged -= InitializeTab;

    }
    private void OnTabClicked()
    {
        ShopManager.Instance.ChangeTab(m_TabType);
    }
    private void ToggleSelected(bool i_IsSelected)
    {
        if (i_IsSelected)
        {
            m_TabIcon.DOColor(ShopManager.Instance.TypeColors.MainColor, .2f).SetEase(Ease.OutQuad);
            m_TabIcon.transform.DOScale(Vector3.one*1.1f,.1f).SetEase(Ease.OutQuad);
        }
        else
        {
            m_TabIcon.DOColor(ShopManager.Instance.TypeColors.SecondaryColor, .2f);
            m_TabIcon.transform.DOScale(Vector3.one, .1f).SetEase(Ease.InQuad);

        }
        if (i_IsSelected)
        {

            if (!m_IsSelectedRect.gameObject.activeSelf)
            {
                m_IsSelectedRect.gameObject.SetActive(i_IsSelected);
                m_IsSelectedRect.anchoredPosition = m_IsSelectedRectOFF.anchoredPosition;
                m_IsSelectedRect.DOAnchorMin(m_IsSelectedRectON.anchorMin, .2f);
                m_IsSelectedRect.DOAnchorMax(m_IsSelectedRectON.anchorMax, .2f);

            }
        }
        else
        {
            if (m_IsSelectedRect.gameObject.activeSelf)
            {
                m_IsSelectedRect.gameObject.SetActive(i_IsSelected);
                m_IsSelectedRect.anchoredPosition = m_IsSelectedRectON.anchoredPosition;
                m_IsSelectedRect.DOAnchorMin(m_IsSelectedRectOFF.anchorMin, .175f);
                m_IsSelectedRect.DOAnchorMax(m_IsSelectedRectOFF.anchorMax, .175f);

                //m_IsSelectedRect.DOAnchorPos(m_IsSelectedRectOFF.anchoredPosition, .175f).SetEase(Ease.InQuad);
            }
        }
        m_IsSelectedRect.gameObject.SetActive(i_IsSelected);

    }
    private void InitializeTab()
    {
        
        ToggleSelected(m_IsSelected);
    }
}
