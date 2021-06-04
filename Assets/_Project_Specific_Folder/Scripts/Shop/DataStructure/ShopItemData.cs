using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK.Shop
{
    [Serializable]
    public class ShopItemData
    {
        [VerticalGroup("Identification")]
        public int Index;
        [VerticalGroup("Identification")]
        public string Name="UNAMED";
        [VerticalGroup("Identification")]
        [ShowInInspector, ReadOnly]
        public string Identifier { get { return Index + "_" + Name + "_" + ItemType.ToString(); } }
        [PreviewField]
        [TableColumnWidth(56, false)]
        public Sprite Sprite;
        [VerticalGroup("enum")]
        [GUIColor(nameof(InEditorColorType))]
        [ReadOnly]
        public eItemType ItemType;
        [GUIColor(nameof(InEditorColorRarity))]
        [VerticalGroup("enum")]
        public eItemRarity ItemRarity;

        [VerticalGroup("Unlock")]
        [HorizontalGroup("Unlock/Ads")]
        public bool UnlockAds=true;
        [ShowIf(nameof(UnlockAds))]
        [VerticalGroup("Unlock")]
        [HorizontalGroup("Unlock/Ads")]
        public int RequiredAds;

        [VerticalGroup("Unlock")]
        [HorizontalGroup("Unlock/Money")]
        public bool UnlockMoney;
        [ShowIf(nameof(UnlockMoney))]
        [VerticalGroup("Unlock")]
        [HorizontalGroup("Unlock/Money")]
        public int RequiredMoney;
        [VerticalGroup("Unlock")]
        public int RequiredLevel;

        /*
         * [VerticalGroup("Unlock")]
        [Range(1, 10)]
        [GUIColor(nameof(InEditorColorRange))]
        public int Chance = 1;*/


        [VerticalGroup("ItemState")]

        [ShowInInspector, ReadOnly]
        public bool IsOwned
        {
            get
            {
                if (Index == 0) PlayerPrefs.SetInt(nameof(IsOwned) + Identifier, 1);
                return PlayerPrefs.GetInt(nameof(IsOwned) + Identifier, 0) == 1;
            }
        }
        [VerticalGroup("ItemState")]
        [ShowInInspector, ReadOnly]
        public bool IsSelected { get { return PlayerPrefs.GetInt(ItemType.ToString(), 0) == Index; } }
        [VerticalGroup("ItemState")]
        [ShowInInspector, ReadOnly]
        public bool IsLocked { get { return RequiredLevel > StorageManager.Instance.HighScoreLevel; } }
        [VerticalGroup("ItemState")]
        [ShowInInspector, ReadOnly]
        public int WatchedAds { get { return PlayerPrefs.GetInt(nameof(WatchedAds) + Identifier, 0); } set { PlayerPrefs.SetInt(nameof(WatchedAds) + Identifier, value); } }

        public ShopItemData()
        {

        }
        public ShopItemData(ShopItemData i_ItemData)
        {
            ItemType = i_ItemData.ItemType;
            Index = i_ItemData.Index + 1;
            Name = i_ItemData.Name;
            Sprite = i_ItemData.Sprite;
            ItemRarity = i_ItemData.ItemRarity;
            RequiredAds = i_ItemData.RequiredAds;
            RequiredMoney = i_ItemData.RequiredMoney;
            RequiredLevel = i_ItemData.RequiredLevel;

        }
        public ShopItemData(int i_Indedx,Sprite i_Sprite,eItemType i_Type)
        {
            ItemType = i_Type;
            Index = i_Indedx;
            Name = i_Sprite.name;
            Sprite = i_Sprite;

        }
        public void OnAdWatched()
        {
            WatchedAds++;
            if (WatchedAds >= RequiredAds)
            {
                SetOwned();
                SelectItem();
            }
        }
        public void SelectItem()
        {
            PlayerPrefs.SetInt(ItemType.ToString(), Index);
        }
        public void SetOwned()
        {
            PlayerPrefs.SetInt(nameof(IsOwned) + Identifier, 1);
        }

        public Color InEditorColorRarity()
        {
            /*if (ShopManager.Instance != null)
            {
                return ShopManager.Instance.ColorSettings.GetColorSet(ItemRarity).ItemBackgroundColor;
            }*/
            switch (ItemRarity)
            {
                case eItemRarity.Common: return Color.gray;
                case eItemRarity.Rare: return Color.blue;
                case eItemRarity.Epic: return Color.red;
                case eItemRarity.Legendary: return Color.red + Color.yellow;

                default: return Color.gray;
            }


        }

        public Color InEditorColorType()
        {

            switch (ItemType)
            {
                case eItemType.Outfit: return Color.yellow;
                case eItemType.Head: return Color.cyan;
                case eItemType.Weapon: return Color.red;
                case eItemType.Upgrade: return Color.green;

                default: return Color.gray;
            }


        }
        /* public Color InEditorColorRange()
         {

             return Color.Lerp(Color.yellow, Color.red, (float)Chance / 10);


         }*/
        
    }
}
