using BrokenMugStudioSDK;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace BrokenMugStudioSDK.Shop
{
    [CreateAssetMenu(fileName = "BrokenMugStudio/ShopItemList")]
    public class ItemTypeData : ScriptableObject
    {
        public Sprite TabIcon;
        public ShopColors.TypeRelatedColors TypeColors;
        public eItemType ListType { get { return ItemDataList[0].ItemType; } }
        [TableList]
        public ShopItemData[] ItemDataList;

        public ShopItemData GetSelectedItem()
        {
            return ItemDataList.First<ShopItemData>((item) => item.IsSelected);
        }

#if UNITY_EDITOR
        [Button]
        public void UnlockAll()
        {
            for (int i = 0; i < ItemDataList.Length; i++)
            {
                ItemDataList[i].SetOwned();
            }
        }
        /*[SerializeField]
        private Sprite[] m_Sprites;
        [Button]
        public void GenerateListFromSprites(eItemType i_ItemType)
        {
            ItemDataList = new ShopItemData[m_Sprites.Length];
            for(int i=0;i< m_Sprites.Length;i++)
            {
                ItemDataList[i] = new ShopItemData(i, m_Sprites[i], i_ItemType);
            }
        }*/
#endif

    }
    

}
