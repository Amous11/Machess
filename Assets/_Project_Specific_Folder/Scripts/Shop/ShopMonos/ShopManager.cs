using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrokenMugStudioSDK.Shop
{
    public class ShopManager : Singleton<ShopManager>
    {
        public delegate void ShopEvent();
        public static event ShopEvent OnTabChanged = delegate { };
        public static event ShopEvent OnSelectionChanged = delegate { };
        public static event ShopEvent OnShopClosed = delegate { };

        [SerializeField]
        private Image m_NavBarImage;
        [SerializeField]
        private Image m_ContentBgImage;
        [SerializeField]
        private Image m_ButtonsBgImage;
        [SerializeField]
        private Image m_ExitBgImage;
        [ReadOnly]
        public eItemType SelectedTab;
        private ShopItemData m_SelectedItem;
        [SerializeField]
        private ShopItemThumbnail[] m_Thumbnails;
        private ShopItemData[] m_TabItems { get { return GameConfig.Instance.Shop.GetItemList(SelectedTab); } }
        public ShopColors.TypeRelatedColors TypeColors { get { return GameConfig.Instance.Shop.GetTypeRelatedColors(SelectedTab); } }
        public ShopItemData SelectedItem 
        {
            get 
            {
                if(m_SelectedItem==null || m_SelectedItem.ItemType!= SelectedTab)
                {
                    m_SelectedItem=GameConfig.Instance.Shop.GetSelectedItem(SelectedTab);
                }
                return m_SelectedItem; 
            } 
        }
        [SerializeField]
        private Button m_CoinBuyButton;
        [SerializeField]
        private Button m_RVBuyButton;
        [SerializeField]
        private Button m_EquipButton;
        [SerializeField]
        private Button m_CloseButton;
        [SerializeField]
        private TextMeshProUGUI m_UnlockText;
        [SerializeField]
        private TextMeshProUGUI m_RequiredCoinText;
        [SerializeField]
        private TextMeshProUGUI m_RequiredRVText;
        [SerializeField]
        private GameObject m_UnlockAt;
        [SerializeField]
        private GameObject m_IsEquiped;
        private void OnEnable()
        {
            m_CoinBuyButton.onClick.AddListener(CoinBuyItem);
            m_RVBuyButton.onClick.AddListener(RVBuyItem);
            m_EquipButton.onClick.AddListener(EquipItem);
            m_CloseButton.onClick.AddListener(CloseShop);
            UpdateItemList();
            UpdateButtons();
            AdsManager.OnRewardVideoAvailabilityChange += UpdateButtons;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            m_CoinBuyButton.onClick.RemoveAllListeners();
            m_RVBuyButton.onClick.RemoveAllListeners();
            m_EquipButton.onClick.RemoveAllListeners();
            m_CloseButton.onClick.RemoveAllListeners();

            AdsManager.OnRewardVideoAvailabilityChange -= UpdateButtons;
        }
        public void CloseShop()
        {
            GameManager.Instance.UnPauseGame();
            MenuManager.Instance.OpenWelcomeScreen();
            if(OnShopClosed!=null)
            {
                OnShopClosed?.Invoke();
            }
        }

        private void UpdateButtons()
        {
            m_CoinBuyButton.gameObject.SetActive(false);
            m_RVBuyButton.gameObject.SetActive(false);
            m_EquipButton.gameObject.SetActive(false);
            m_UnlockAt.gameObject.SetActive(false);
            m_IsEquiped.SetActive(false);
            if (m_SelectedItem.IsLocked)
            {
                m_UnlockAt.gameObject.SetActive(true);
                m_UnlockText.text = "UNLOCK AT LEVEL " + m_SelectedItem.RequiredLevel;

            }else if(m_SelectedItem.IsOwned)
            {
                if(m_SelectedItem.IsSelected)
                {
                    m_IsEquiped.SetActive(true);

                }
                else
                {
                    m_EquipButton.gameObject.SetActive(true);
                }

            }else
            {
                m_RVBuyButton.gameObject.SetActive(m_SelectedItem.UnlockAds);
                m_CoinBuyButton.gameObject.SetActive(m_SelectedItem.UnlockMoney);
            }
            m_RequiredCoinText.text = m_SelectedItem.RequiredMoney.ToString();
            m_RequiredRVText.text = m_SelectedItem.WatchedAds+"/" +m_SelectedItem.RequiredAds;
            m_CoinBuyButton.interactable = m_SelectedItem.RequiredMoney <= StorageManager.Instance.PlayerMoney;
            m_RVBuyButton.interactable = AdsManager.Instance.IsRewardVideoLoaded;

        }
        public void UpdateItemList()
        {
            for(int i=0;i< m_Thumbnails.Length;i++)
            {
                if(i<m_TabItems.Length)
                {
                    m_Thumbnails[i].gameObject.SetActive(true);
                    m_Thumbnails[i].SetItem(m_TabItems[i]);
                }
                else
                {
                    m_Thumbnails[i].gameObject.SetActive(false);
                }
            }
        }

        public void ChangeSelection(ShopItemData i_Selected)
        {
            
            m_SelectedItem = i_Selected;
            
            UpdateItemList();
            UpdateButtons();
            if (OnSelectionChanged != null)
            {
                OnSelectionChanged?.Invoke();
            }
        }

        public void ChangeTab(eItemType i_Type)
        {
            if(SelectedTab!= i_Type)
            {
                SelectedTab = i_Type;
                m_SelectedItem = SelectedItem;
                UpdateItemList();
                ColorTweening();
                if (OnTabChanged!=null)
                {
                    OnTabChanged?.Invoke();
                }
            }
        }
        public void ColorTweening()
        {
            m_NavBarImage.DOColor(TypeColors.MainColor, .2f);
            m_ContentBgImage.DOColor(TypeColors.SecondaryColor, .2f);
            m_ButtonsBgImage.DOColor(TypeColors.MainColor, .2f);
            m_ExitBgImage.DOColor(TypeColors.MainColor, .2f);
        }

        public void CoinBuyItem()
        {
            if(StorageManager.Instance.PlayerMoney>= m_SelectedItem.RequiredMoney)
            {
                StorageManager.Instance.PlayerMoney -= m_SelectedItem.RequiredMoney;
                m_SelectedItem.SetOwned();
                m_SelectedItem.SelectItem();
                UpdateItemList();
                UpdateButtons();
            }
        }

        public void RVBuyItem()
        {
            AdsManager.Instance.ShowRewardVideo
                (
                () => 
                {
                    m_SelectedItem.OnAdWatched();
                },
                () => { },
                "Shop_Unlock"
                );
        }
        public void EquipItem()
        {
            m_SelectedItem.SelectItem();
            UpdateButtons();

        }

#if UNITY_EDITOR
        [Button]
        public void SetRefs()
        {
            m_Thumbnails = GetComponentsInChildren<ShopItemThumbnail>(true);
        }
#endif
    }
}

