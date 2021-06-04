using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
namespace BrokenMugStudioSDK
{
    [Serializable]
    public class BuildSettingsEditor
    {
        public bool IsDeveloperMode = true;
#if UNITY_EDITOR

        public string BuildAndroidPath { get => EditorPrefs.GetString(Constants.k_BuildAndroidEditorPrefKey, Constants.k_DefaultBuildPath); set => EditorPrefs.SetString(Constants.k_BuildAndroidEditorPrefKey, value); }
        public bool IsAppBundle = false;
        public string BuildAndroidProductName { get { return $"{PlayerSettings.productName}_v{GameSettings.Instance.General.BuildVersion}_{GameSettings.Instance.General.BuildNumber}_{PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android)}_{(PlayerSettings.Android.targetArchitectures == AndroidArchitecture.All ? "ARMv7_ARM64" : PlayerSettings.Android.targetArchitectures.ToString())}.{(IsAppBundle ? "aab" : "apk")}"; } }
        public string BuildAndroidFolderPath { get { return $"{BuildAndroidPath}{PlayerSettings.productName}_v{GameSettings.Instance.General.BuildVersion}_{GameSettings.Instance.General.BuildNumber}_Android/"; } }
        public BuildOptions BuildOptionAndroid = BuildOptions.AutoRunPlayer | BuildOptions.ShowBuiltPlayer;
        public BuildPlayerOptions BuildPlayerOptions;
        [ShowInInspector]
        public string ProductName { get { return $"{BuildAndroidProductName}"; } }
        [ShowInInspector]
        public string BuildAndroidFullPathIncludingFolder { get { return $"{BuildAndroidFolderPath}{BuildAndroidProductName}"; } }

        [Button]
        public void SwitchToMono()
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        }
        [Button]
        public void SwitchToIL2CPP()
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
        }
        [Button]
        public void Save()
        {
            EditorSceneManager.SaveOpenScenes();
            if (!Directory.Exists(BuildAndroidFolderPath))
            {
                Directory.CreateDirectory(BuildAndroidFolderPath);
            }
            EditorUserBuildSettings.buildAppBundle = IsAppBundle;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            EditorUserBuildSettings.installInBuildFolder = true;
            EditorUserBuildSettings.SetBuildLocation(BuildTarget.Android, BuildAndroidFolderPath);
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
            buildPlayerOptions.locationPathName = BuildAndroidFullPathIncludingFolder;

            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptionAndroid;

            //ProjectCustomBuildProcessor.CustomBuildProcessor(buildPlayerOptions);
        }
#if UNITY_EDITOR
        [BoxGroup("APK"), InfoBox("Make sure to select a key store", nameof(SignAPK)), OnValueChanged(nameof(SetSignAPK)), ShowInInspector]
        public bool SignAPK { get => EditorPrefs.GetBool(Constants.k_SignAPKEditorPrefKey, false); set => EditorPrefs.SetBool(Constants.k_SignAPKEditorPrefKey, value); }
        [BoxGroup("APK"), PropertyOrder(2)] public string KeyStoreName = "BMS";
        [BoxGroup("APK"), PropertyOrder(3)] public string KeyStorePass = "BMS!123";
        [BoxGroup("APK"), PropertyOrder(4)] public string KeyaliasName = "bms_key";
        [BoxGroup("APK"), PropertyOrder(5)] public string KeyaliasPass = "BMS!123";
#endif
        public void SetSignAPK()
        {
#if UNITY_EDITOR
            if (SignAPK)
            {
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = $"{Constants.k_KeyStorePath}/{KeyStoreName}.keystore";
                PlayerSettings.Android.keystorePass = KeyStorePass;
                PlayerSettings.Android.keyaliasName = KeyaliasName;
                PlayerSettings.Android.keyaliasPass = KeyaliasPass;
            }
            else
            {
                PlayerSettings.Android.useCustomKeystore = false;
                PlayerSettings.Android.keystoreName = string.Empty;
                PlayerSettings.Android.keystorePass = string.Empty;
                PlayerSettings.Android.keyaliasName = string.Empty;
                PlayerSettings.Android.keyaliasPass = string.Empty;
            }
#endif
        }
        [Button]
        public void BuildGameAndroid()
        {
            Save();
            SetSignAPK();
            // Get filename.
            BuildPlayerOptions = new BuildPlayerOptions();
            BuildPlayerOptions.scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
            BuildPlayerOptions.locationPathName = BuildAndroidFullPathIncludingFolder;

            BuildPlayerOptions.target = BuildTarget.Android;
            BuildPlayerOptions.options = BuildOptionAndroid;
            // Build player.
            //BuildPipeline.BuildPlayer(levels, BuildAndroidFullPathIncludingFolder, BuildTarget.Android, BuildOptionAndroid);
            BuildPipeline.BuildPlayer(BuildPlayerOptions);

            // Copy a file from the project folder to the build folder, alongside the built game.
            //FileUtil.CopyFileOrDirectory("Assets/Templates/Readme.txt", path + "Readme.txt");

            // Run the game (Process class from System.Diagnostics).
            Process proc = new Process();
            proc.StartInfo.FileName = BuildAndroidFullPathIncludingFolder;
            proc.Start();
        }
#endif

    }
}

