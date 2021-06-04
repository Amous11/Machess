using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;

namespace BrokenMugStudioSDK
{
    /// <summary>
    /// Automatically updates Coins when Storage is changed
    /// </summary>
    public class MoneyUpdater : MoneyContainerBase
    {
        [SerializeField] protected bool AutoPopAnimation = true;


        protected override void OnEnable()
        {
            base.OnEnable();

            StorageManager.OnMoneyAmountChanged += MoneyChanged;

            SetMoney(StorageManager.Instance.PlayerMoney, false);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            StorageManager.OnMoneyAmountChanged -= MoneyChanged;
        }


        protected virtual void MoneyChanged(int i_Value)
        {
            SetMoney(i_Value, AutoPopAnimation);
        }
    }
}

