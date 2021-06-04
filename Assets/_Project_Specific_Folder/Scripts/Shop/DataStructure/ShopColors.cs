using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK.Shop
{

    public class ShopColors : MonoBehaviour
    {
        [Serializable]
        public class TypeRelatedColors
        {
            public Color MainColor = Color.blue;
            public Color SecondaryColor = Color.cyan;
            public Color NormalyWhite = Color.white;

        }
        [Serializable]
        public class RarityRelatedColors
        {
            public Color BackgroundColor = Color.blue;
            public Color TextColor = Color.cyan;
            public Color SelectedTeint = Color.white;
        }

    }
}
