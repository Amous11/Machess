using Sirenix.OdinInspector;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Reflection;
#endif
namespace BrokenMugStudioSDK
{
    [Serializable]
    public class GeneralEditor
    {
        public string GameName;
        public string BuildVersion;
        public int BuildNumber;
        public string BundleIdentifier;
        [PreviewField]
        public Sprite DefaultIcon;
        [PreviewField]
        public Sprite GameIcon;
        [ReadOnly]
        public string GooglePlayId = string.Empty;
        //public string AppStoreId = string.Empty;

#if UNITY_EDITOR
        [Button]
        private void Save()
        {
            Sprite gameIcon;
            if(GameIcon==null)
            {
                gameIcon = DefaultIcon;
            }else
            {
                gameIcon = GameIcon;
            }
            GooglePlayId = BundleIdentifier;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, BundleIdentifier);
            PlayerSettings.companyName = Constants.k_Company;
            PlayerSettings.productName = GameName;
            PlayerSettings.bundleVersion = BuildVersion;
            PlayerSettings.Android.bundleVersionCode = BuildNumber;
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] { gameIcon.texture });
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, new Texture2D[] { gameIcon.texture });
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, new Texture2D[] { gameIcon.texture });
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        }
#endif
        public void OpenAppStoreURL()
        {
            Utils.OpenUrlStore(GooglePlayId, "");
        }
    }

}
