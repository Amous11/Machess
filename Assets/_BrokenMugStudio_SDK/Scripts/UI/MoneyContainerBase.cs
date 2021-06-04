using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    public class MoneyContainerBase : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI m_CoinText;
        [SerializeField] protected UIScaler m_TextScaler;

        [SerializeField] protected RectTransform m_MoneyTarget;
        public RectTransform MoneyTarget { get => m_MoneyTarget; }

        [SerializeField] protected UIScaler m_IconScaler;


        [Button]
        protected virtual void SetRefs()
        {
            m_CoinText = GetComponentInChildren<TextMeshProUGUI>();
            m_TextScaler = m_CoinText.GetComponentInChildren<UIScaler>();
            m_MoneyTarget = transform.FindDeepChild<RectTransform>("Icon");
            m_IconScaler = m_MoneyTarget.GetComponentInChildren<UIScaler>();
        }



        protected virtual void Awake()
        {
            if (m_CoinText.transform.localScale != Vector3.one)
                Debug.LogError("Original Scale should be 1", this);
        }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }


        public virtual void SetMoney(int i_Value, bool i_PopAnim)
        {
            SetCoinText(i_Value);

            if (i_PopAnim)
                Pop();
        }

        private void SetCoinText(int i_Value)
        {
            m_CoinText.text = i_Value.ToString();
        }

        public void Pop()
        {
            m_IconScaler.StopAnimation();
            m_IconScaler.StartAnimation();
        }
    }
}
