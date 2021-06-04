/*using System;
using DG.Tweening.Core;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor.U2D;
using UnityEditor.SceneManagement;

namespace BrokenMugStudioSDK
{
    [InitializeOnLoad]
    public class ProjectCustomBuildProcessor
    {
        static ProjectCustomBuildProcessor()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(CustomBuildProcessor);
        }

        public static void PerformBuildSilent()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Contains(nameof(GeneralEditor.k_None)))
            {
                UnityEngine.Debug.Log($"PerformBuildSilent with k_None");
                GameSettings.Instance.General.BuildSuccessCmd = GeneralEditor.k_None;
            }
            else if (args.Contains(nameof(GeneralEditor.k_UpdateMetaData)))
            {
                UnityEngine.Debug.Log($"PerformBuildSilent with k_UpdateMetaData");
                GameSettings.Instance.General.BuildSuccessCmd = GeneralEditor.k_UpdateMetaData;
            }
            else if (args.Contains(nameof(GeneralEditor.k_UpdatePolicy)))
            {
                UnityEngine.Debug.Log($"PerformBuildSilent with k_UpdatePolicy");
                GameSettings.Instance.General.BuildSuccessCmd = GeneralEditor.k_UpdatePolicy;
            }
            else if (args.Contains(nameof(GeneralEditor.k_BuildAndUpload)))
            {
                UnityEngine.Debug.Log($"PerformBuildSilent with k_BuildAndUpload");
                GameSettings.Instance.General.BuildSuccessCmd = GeneralEditor.k_BuildAndUpload;
            }
            else if (args.Contains(nameof(GeneralEditor.k_UploadOnly)))
            {
                UnityEngine.Debug.Log($"PerformBuildSilent with k_UploadOnly");
                GameSettings.Instance.General.BuildSuccessCmd = GeneralEditor.k_UploadOnly;
            }
            else
            {
                UnityEngine.Debug.Log($"PerformBuildSilent with No Args, Using GameSettings.Instance.General.BuildSuccessCmd: {GameSettings.Instance.General.BuildSuccessCmd}");
            }

            EditorSceneManager.SaveOpenScenes();

            GameSettings.Instance.Sync.ReImportPostProcessBuildScripts();

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
            buildPlayerOptions.locationPathName = GameSettings.Instance.General.BuildIOSFullPath;
            buildPlayerOptions.target = BuildTarget.iOS;
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.AcceptExternalModificationsToPlayer | GameSettings.Instance.General.BuildOptionIOS;

            CustomBuildProcessor(buildPlayerOptions);
        }

        public static void CustomBuildProcessor(BuildPlayerOptions i_BuildPlayerOptions)
        {
            //SAVE CURRENT PROJECT STATUS
            bool wasDebugEnable = UtilsEditor.IsDefineDirectiveExists(DirectiveConstants.k_ENABLE_LOGS_DIRECTIVE);

            //BUILD PROJECT
            BuildReport report = BuildPlayerWithReport(i_BuildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                UnityEngine.Debug.LogError("Build succeeded: " + summary.totalSize + " bytes");

                GameSettings.Instance.General.OnBuildSuccess();
            }

            if (summary.result == BuildResult.Failed)
            {
                UnityEngine.Debug.LogError("Build failed");
            }

            //LOAD PREVIOUS PROJECT STATUS
            UnityEngine.Debug.Log($"Changing IsDebugLogs from {GameSettings.Instance.General.IsDebugEnabled} to {wasDebugEnable}");
            GameSettingsEditor.SetDebugLogs(wasDebugEnable);
        }

        public static BuildReport BuildPlayerWithReport(BuildPlayerOptions i_BuildPlayerOptions)
        {

            UnityEngine.Debug.LogError($"BuildPlayerWithReport: {EditorUserBuildSettings.activeBuildTarget}");

            bool isAddressableBuild = (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android && GameSettings.Instance.General.AddressableAndroid) ||
                                 (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS && GameSettings.Instance.General.AddressableIOS);

            ValidateBuild(i_BuildPlayerOptions, isAddressableBuild);

            return BuildPipeline.BuildPlayer(i_BuildPlayerOptions);
        }

        public static void ValidateBuild(BuildPlayerOptions i_BuildPlayerOptions, bool i_ShowAddressableDialog = true)
        {
            //Rule #0
            if (GameSettings.Instance.General.IOSTeamIDTest)
            {
                PlayerSettings.iOS.appleEnableAutomaticSigning = false;

                //if (GameSettings.Instance.General.SignIOSTeamID == GeneralEditor.k_BepNew)
                //{
                //    PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "com.gamestest.test");
                //}
                //else
                //if (GameSettings.Instance.General.SignIOSTeamID == GeneralEditor.k_Kpic)
                {
                    PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "com.beptest.test");
                }
            }

            //Rule #1 - DOTween Safe Mode force true for production build
            if (!i_BuildPlayerOptions.options.HasFlag(BuildOptions.Development))
            {
                if (Resources.Load<DOTweenSettings>("DOTweenSettings").useSafeMode == false)
                {
                    throw new Exception("DOTWeen Safe Mode is set to false, please set to true for production build.\nBuilding Stopped.");
                }
            }

            //Rule #2 - Setting up Addressables
            if (i_ShowAddressableDialog)
            {
                if (EditorUtility.DisplayDialog("Build with Addressables",
                   "Do you want to build a clean addressables before export?",
                   "Build with Addressables", "Skip"))
                {
                    BuildAddresssableAssetPreBuild();
                }
            }

            //Rule #3 - Force remove Logs
            if (!i_BuildPlayerOptions.options.HasFlag(BuildOptions.Development))
            {
                GameSettingsEditor.SetDebugLogs(false);
            }

            //Rule #4 - Force ASTC for Sprites on iOS
            ForceSpriteAtlasOptions();
            //Note - Not a fan of automatic setting up sprite configuration when building since we might want specific formats like uncompressed sometimes. In any case i'll leave the code here in case we want to use in the future
            //Note 2 - we already have default texture importer so we might not need to use it at all
            //ForceSpriteOptions();
        }

        static public void BuildAddresssableAssetPreBuild()
        {
            UnityEngine.Debug.Log("BuildAddresssableAssetPreBuild start");
            AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
            AddressableAssetSettings.BuildPlayerContent();
            UnityEngine.Debug.Log("BuildAddresssableAssetPreBuild done");
        }

        #region Force Sprites Options
        public static void ForceSpriteAtlasOptions()
        {
            string[] guids1 = AssetDatabase.FindAssets("t:SpriteAtlas", null);

            for (int i = 0; i < guids1.Length; i++)
            {
                SpriteAtlas spriteAtlas = (SpriteAtlas)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids1[i]), typeof(SpriteAtlas));

                ForceSpriteAtlasPlatform(spriteAtlas, BuildTarget.iOS.ToString(), TextureImporterFormat.ASTC_4x4);
                //This is deprecated but for some reason it's the one that changes the editor variables. Just in case, leaving it here
                ForceSpriteAtlasPlatform(spriteAtlas, "iPhone", TextureImporterFormat.ASTC_4x4);

                //Debug.LogError(spriteAtlas.name, spriteAtlas);
            }
        }
        private static void ForceSpriteAtlasPlatform(SpriteAtlas i_SpriteAtlas, string i_BuildTarget, TextureImporterFormat i_Format)
        {
            TextureImporterPlatformSettings settings = i_SpriteAtlas.GetPlatformSettings(i_BuildTarget);
            settings.overridden = true;
            settings.format = i_Format;
            i_SpriteAtlas.SetPlatformSettings(settings);
        }

        public static void ForceSpriteOptions()
        {
            string[] guids1 = AssetDatabase.FindAssets("t:Texture2D", null);

            for (int i = 0; i < guids1.Length; i++)
            {
                Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids1[i]), typeof(Texture2D));

                //getting error in some cases. Using Try to bypass that
                try
                {
                    TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guids1[i]));

                    if (textureImporter.textureType == TextureImporterType.Sprite)
                    {
                        ForceSpritePlatform(textureImporter, BuildTarget.iOS.ToString(), TextureImporterFormat.ASTC_4x4);
                        //This is deprecated but for some reason it's the one that changes the editor variables. Just in case, leaving it here
                        ForceSpritePlatform(textureImporter, "iPhone", TextureImporterFormat.ASTC_4x4);

                        //Debug.LogError(texture.name, texture);
                    }
                }
                catch { }
            }
        }

        private static void ForceSpritePlatform(TextureImporter i_TextureImporter, string i_BuildTarget, TextureImporterFormat i_Format)
        {
            TextureImporterPlatformSettings settings = i_TextureImporter.GetPlatformTextureSettings(i_BuildTarget);
            settings.overridden = true;
            settings.format = i_Format;
            i_TextureImporter.SetPlatformTextureSettings(settings);
        }

        #endregion
    }
}
*/