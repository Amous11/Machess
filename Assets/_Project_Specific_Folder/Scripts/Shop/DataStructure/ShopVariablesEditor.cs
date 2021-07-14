using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK.Shop
{
    [System.Serializable]
    public class DictionaryRarityColors : UnitySerializedDictionary<eItemRarity, ShopColors.RarityRelatedColors>
    {

    }
    [Serializable]
    public class ShopVariablesEditor
    {
        public ShopItemsLists ShopItemsLists;
        public DictionaryRarityColors RarityColors;

        public ShopItemData GetSelectedItem(eItemType i_Type)
        {
            return GetItemTypeData(i_Type).GetSelectedItem();
        }
        public ShopColors.TypeRelatedColors GetTypeRelatedColors(eItemType i_Type)
        {
            return GetItemTypeData(i_Type).TypeColors;
        }
        public ShopItemData[] GetItemList(eItemType i_Type)
        {
            return ShopItemsLists.ItemLists[i_Type].ItemDataList;
        }
        public ItemTypeData GetItemTypeData(eItemType i_Type)
        {
            return ShopItemsLists.ItemLists[i_Type];
        }

    }
}

