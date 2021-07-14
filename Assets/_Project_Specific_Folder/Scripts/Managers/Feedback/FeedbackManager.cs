using UnityEngine;
using UnityEngine.UI;
namespace BrokenMugStudioSDK
{
    public class FeedbackManager : Singleton<FeedbackManager>
    {
        [SerializeField]
        private GameObject m_RateUsCanvas;
        [SerializeField]
        private Button m_RateItButton;
        [SerializeField]
        private Button m_LaterButton;
        private void OnEnable()
        {
            m_RateItButton.onClick.AddListener(OnRateIt);
            m_LaterButton.onClick.AddListener(OnLater);
            GameManager.OnLevelLoaded += ShowRateUs;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            m_RateItButton.onClick.RemoveAllListeners();
            m_LaterButton.onClick.RemoveAllListeners();
            GameManager.OnLevelLoaded -= ShowRateUs;

        }

        private void ShowRateUs()
        {
            if (!StorageManager.Instance.HasRatedGame && GameSettings.Instance.Feedback.ShowRateUsNow())
            {
                m_RateUsCanvas.SetActive(true);

            }
        }
        private void OnRateIt()
        {
            m_RateUsCanvas.SetActive(false);
            GameSettings.Instance.General.OpenAppStoreURL();
            StorageManager.Instance.HasRatedGame = true;

        }
        private void OnLater()
        {
            m_RateUsCanvas.SetActive(false);

        }

    }

}
