using BrokenMugStudioSDK.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BrokenMugStudioSDK
{
    public class HUDManagerBase : Singleton<HUDManager>
    {
        [SerializeField] private ExtendedButton m_InputButton;
        [SerializeField] private TextMeshProUGUI m_LevelText;
        [SerializeField] private MoneyUpdater m_CoinUpdater;

        private CoinLerpAnim m_DummyCoinLerpAnim;

        private HUDVariablesEditor m_HUDVars { get { return GameConfig.Instance.HUD; } }
        public virtual void OnEnable()
        {
            if(m_InputButton!=null)
            {
                m_InputButton.OnDownEvent += InputDown;
                m_InputButton.OnUpEvent += InputUp;
            }
            
            GameManager.OnLevelReset += OnReset;
            OnReset();
        }
        
        public virtual void OnReset()
        {
            UpdateCurrentLevelText();

        }
        private void UpdateCurrentLevelText()
        {
            m_LevelText.text = "Level " + (StorageManager.Instance.CurrentLevel+1);
        }
        public override void OnDisable()
        {
            base.OnDisable();
            if (m_InputButton != null)
            {
                m_InputButton.OnDownEvent -= InputDown;
                m_InputButton.OnUpEvent -= InputUp;
            }
            
            GameManager.OnLevelReset -= OnReset;

        }
        public void InputDown(PointerEventData i_PointerEventData)
        {
            InputManager.Instance.InputDown();
        }

        public void InputUp(PointerEventData i_PointerEventData)
        {
            InputManager.Instance.InputUp();

        }

        #region Coins
        private int m_FullAmountCoinsToReceive = 0;
        private int m_AmountCoinsReceivedPerAnim = 0;
        private Action m_CoinAnimCallback;
        /// <summary>
        /// Animate coins from spawn transform to default coin target
        /// </summary>
        /// <param name="i_CoinAmount"></param>
        /// <param name="i_Callback"></param>
        /// <param name="i_SpawnRectTransform"></param>
        public void AnimateCoins(int i_CoinAmount, RectTransform i_SpawnRectTransform, Action i_Callback)
        {
            AnimateCoins(i_CoinAmount, i_SpawnRectTransform, m_CoinUpdater.MoneyTarget, i_Callback);
        }

        /// <summary>
        /// Animate coins from spawn transform to custom coin target
        /// </summary>
        /// <param name="i_CoinAmount"></param>
        /// <param name="i_Callback"></param>
        /// <param name="i_SpawnRectTransform"></param>
        /// <param name="i_TargetRectTransform"></param>
        public void AnimateCoins(int i_CoinAmount, RectTransform i_SpawnRectTransform, RectTransform i_TargetRectTransform, Action i_Callback, RectTransform i_Parent = null)
        {
            m_FullAmountCoinsToReceive = i_CoinAmount;
            m_CoinAnimCallback = i_Callback;

            int amountCoinsUI = Mathf.Min(m_HUDVars.CoinUIAnimData.AmountCoins, i_CoinAmount);
            m_AmountCoinsReceivedPerAnim = Mathf.CeilToInt(m_FullAmountCoinsToReceive / (float)amountCoinsUI);
            //This will change the final amount of coins but it will make more sense since everything is more evenly distributed
            int finalAmountOfCoins = Mathf.CeilToInt(m_FullAmountCoinsToReceive / (float)m_AmountCoinsReceivedPerAnim);

            for (int i = 0; i < finalAmountOfCoins; i++)
            {
                StartCoroutine(AnimateCoinCoroutine(m_HUDVars.CoinUIAnimData.DelayBetweenCoins * i, i_SpawnRectTransform, i_TargetRectTransform, i_Parent));
            }
        }

        private IEnumerator AnimateCoinCoroutine(float i_Delay, RectTransform i_SpawnRectTransform, RectTransform i_TargetRectTransform, RectTransform i_Parent = null)
        {
            yield return new WaitForSeconds(i_Delay);

            m_DummyCoinLerpAnim = PoolManager.Instance.Dequeue(ePoolType.CoinUI).GetComponent<CoinLerpAnim>();

            m_DummyCoinLerpAnim.transform.SetParent(i_Parent == null ? i_TargetRectTransform : i_Parent);
            m_DummyCoinLerpAnim.Animate(i_SpawnRectTransform, m_CoinUpdater.MoneyTarget, m_HUDVars.CoinUIAnimData.AnimData, ReceiveCoins);
        }

        private void ReceiveCoins(SpriteLerpAnim i_CoinAnimated)
        {
            int amountToReceive = m_AmountCoinsReceivedPerAnim;
            if (m_AmountCoinsReceivedPerAnim > m_FullAmountCoinsToReceive)
                amountToReceive = m_FullAmountCoinsToReceive;

            StorageManager.Instance.PlayerMoney += amountToReceive;

            PoolManager.Instance.Queue(ePoolType.CoinUI, i_CoinAnimated.gameObject);

            m_FullAmountCoinsToReceive -= m_AmountCoinsReceivedPerAnim;

            if (m_FullAmountCoinsToReceive <= 0)
            {
                //Debug.LogError("Coin anim finished");
                m_CoinAnimCallback.InvokeSafe();
            }
        }
        #endregion
    }
}

