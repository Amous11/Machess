
using DG.Tweening;

namespace BrokenMugStudioSDK
{
    public class MenuManager : MenuManagerBase
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.OnLevelLoaded += OpenWelcomeScreen;

            GameManager.OnLevelStarted += OpenInGameScreen;
            GameManager.OnLevelContinue += OpenInGameScreen;

            GameManager.OnLevelFailedNoContinue += OpenLoseScreen;
            GameManager.OnLevelFailed += OpenLoseReviveScreen;
            GameManager.OnLevelCompleted += OpenWinScreen;


        }
        public override void OnDisable()
        {
            base.OnDisable();
            GameManager.OnLevelLoaded -= OpenWelcomeScreen;
            GameManager.OnLevelContinue -= OpenInGameScreen;

            GameManager.OnLevelStarted -= OpenInGameScreen;
            GameManager.OnLevelFailedNoContinue -= OpenLoseScreen;
            GameManager.OnLevelFailed -= OpenLoseReviveScreen;
            GameManager.OnLevelCompleted -= OpenWinScreen;

        }

        public void OpenWelcomeScreen()
        {
            CloseAll();
            OpenMenuScreen(eMenuScreens.Screen_Welcome.ToString());
        }
        public void OpenShopScreen()
        {
            CloseAll();
            OpenMenuScreen(eMenuScreens.Screen_Shop.ToString());
        }
        public void OpenInGameScreen()
        {
            CloseAll();
            OpenMenuScreen(eMenuScreens.Screen_InGame.ToString());

        }

        public void OpenWinScreen()
        {
            CloseAll();
            DOVirtual.DelayedCall(GameConfig.Instance.HUD.ShowLevelCompleteDelay, () => { OpenMenuScreen(eMenuScreens.Screen_LevelCompleted.ToString()); });
            

        }
        public void OpenLoseScreen()
        {
            CloseAll();
            DOVirtual.DelayedCall(GameConfig.Instance.HUD.ShowLevelFailedDelay, () => { OpenMenuScreen(eMenuScreens.Screen_LevelFailed.ToString()); });

            
        }

        public void OpenLoseReviveScreen()
        {
            CloseAll();
            DOVirtual.DelayedCall(GameConfig.Instance.HUD.ShowLevelFailedDelay, () => { OpenMenuScreen(eMenuScreens.Screen_LevelFailedRevive.ToString()); });

        }
    }
}

