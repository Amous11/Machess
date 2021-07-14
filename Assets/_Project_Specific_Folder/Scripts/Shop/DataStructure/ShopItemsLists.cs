using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK.Shop
{
    [System.Serializable]
    public class DictionaryShopItems : UnitySerializedDictionary<eItemType, ItemTypeData>
    {

    }
    [Serializable]
    public class ShopItemsLists
    {
        [SerializeField]//, UnityEngine.Serialization.FormerlySerializedAs("PoolItemsDefinition2")]
        public DictionaryShopItems ItemLists = new DictionaryShopItems();
    }
}
