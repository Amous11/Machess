using Sirenix.OdinInspector;

namespace BrokenMugStudioSDK.Animation
{
    [System.Serializable]
    public struct CoinUIAnimData
    {
        [InfoBox("This might be slightly different during the game if the amount of coins received doesn't divide that well by this number")]
        public int AmountCoins;
        public float DelayBetweenCoins;
        public CoinLerpAnimData AnimData;
    }
}