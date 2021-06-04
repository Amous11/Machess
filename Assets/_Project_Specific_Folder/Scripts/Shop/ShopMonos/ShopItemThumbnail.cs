using BrokenMugStudioSDK;
using BrokenMugStudioSDK.Shop;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemThumbnail : MonoBehaviour
{
    [SerializeField]
    private Button m_Button;
    private ShopItemData m_DisplayItem;
    [Title("Images")]
    [SerializeField]
    private Image m_ItemSprite;
    [SerializeField]
    private Image m_ItemBackground;
    [Title("Textes")]
    [SerializeField]
    private TextMeshProUGUI m_ItemNameText;
    [SerializeField]
    private TextMeshProUGUI m_ItemRarityText;
    [SerializeField]
    private TextMeshProUGUI m_RequiredLevelText;
    [Title("GameObjects")]
    [SerializeField]
    private GameObject m_Selected;
    [SerializeField]
    private GameObject m_Locked;
    [SerializeField]
    private GameObject m_RequiredLevel;

    private void OnEnable()
    {
        m_Button.onClick.AddListener(OnClicked);

    }

    private void OnDisable()
    {
        m_Button.onClick.RemoveAllListeners();
    }


    public void OnClicked()
    {
        ShopManager.Instance.ChangeSelection(m_DisplayItem);
    }
    public void SetItem(ShopItemData i_DisplayItem)
    {
        m_DisplayItem = i_DisplayItem;
        InitializeThumbnail();
    }
    public void InitializeThumbnail()
    {
        m_ItemSprite.sprite = m_DisplayItem.Sprite;
        m_Selected.SetActive(ShopManager.Instance.SelectedItem == m_DisplayItem);
        if(ShopManager.Instance.SelectedItem == m_DisplayItem)
        {
            transform.DOScale(Vector3.one * 1.05f, .2f).SetEase(Ease.InOutBack);
        }else
        {
            transform.DOScale(Vector3.one , .2f);

        }
        m_Locked.SetActive(m_DisplayItem.IsLocked);
        m_RequiredLevel.SetActive(m_DisplayItem.IsLocked);
        m_ItemNameText.text = m_DisplayItem.Name;
        m_RequiredLevelText.text = "LVL " + m_DisplayItem.RequiredLevel;
        m_ItemRarityText.text = m_DisplayItem.ItemRarity.ToString();

        m_ItemRarityText.color = GameConfig.Instance.Shop.RarityColors[m_DisplayItem.ItemRarity].TextColor;
        m_ItemBackground.color = GameConfig.Instance.Shop.RarityColors[m_DisplayItem.ItemRarity].BackgroundColor;
        m_ItemSprite.color = GameConfig.Instance.Shop.RarityColors[m_DisplayItem.ItemRarity].SelectedTeint;

        //m_ItemBackground.color=
    }

}
