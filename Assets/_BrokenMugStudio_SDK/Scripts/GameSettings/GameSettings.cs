using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace BrokenMugStudioSDK
{
    [CreateAssetMenu(fileName = "GameSettings")]
    public class GameSettings : SingletonScriptableObject<GameSettings>
    {
        [TabGroup("General"), ShowInInspector, HideReferenceObjectPicker, HideLabel] public GeneralEditor General = new GeneralEditor();
        [TabGroup("Screenshots"), ShowInInspector, HideReferenceObjectPicker, HideLabel] public ScreenshotsEditor Screenshots = new ScreenshotsEditor();
        [TabGroup("Build Settings"), ShowInInspector, HideReferenceObjectPicker, HideLabel] public BuildSettingsEditor BuildSettings = new BuildSettingsEditor();
        [TabGroup("Ads Settings"), ShowInInspector, HideReferenceObjectPicker, HideLabel] public AdSettingsEditor AdSettings = new AdSettingsEditor();
        [TabGroup("Analytics"), ShowInInspector, HideReferenceObjectPicker, HideLabel] public AnalyticsEditor Analytics = new AnalyticsEditor();

        [TabGroup("Define Symbols"), ShowInInspector, HideReferenceObjectPicker, HideLabel] public DefineSymbolsEditor DefineSymbols = new DefineSymbolsEditor();
        [TabGroup("Feedback"), ShowInInspector, HideReferenceObjectPicker, HideLabel] public FeedbackEditor Feedback = new FeedbackEditor();

        [TabGroup("SDK Updates"), ShowInInspector, HideReferenceObjectPicker, HideLabel] public SDKUpdaterEditor SDKUpdater = new SDKUpdaterEditor();




#if UNITY_EDITOR
        #region SDKLogo
        private Texture m_KobGamesInspectorTexture;
        public Texture KobGamesInspectorTexture
        {
            get
            {
                if (m_KobGamesInspectorTexture == null)
                {
                    m_KobGamesInspectorTexture = AssetDatabase.LoadAssetAtPath<Texture>(Constants.k_BMS_Logo_Path);
                }

                return m_KobGamesInspectorTexture;
            }
        }

        [OnInspectorGUI, PropertyOrder(-10)]
        private void ShowImage()
        {
            try
            {
                GUILayout.Label(KobGamesInspectorTexture, EditorStyles.centeredGreyMiniLabel, GUILayout.MinHeight(90),GUILayout.MaxHeight(200));
            }
            catch { }
        }
        #endregion
#endif
    }
}

