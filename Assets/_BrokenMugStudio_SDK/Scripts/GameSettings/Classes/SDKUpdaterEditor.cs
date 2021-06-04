using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    [Serializable]
    public class SDKUpdaterEditor
    {
#if UNITY_EDITOR
        public string SDKPath = "D:/BrokenMugStudio/000_UnityProjects/00_BMS_SDK_UNOPENED/";
        public string SafetyPathBase = "D:/BrokenMugStudio/000_SafetyPath/";
        public string DateTimeModifier { get { return DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"); } }
        [SerializeField, InlineButton(nameof(SetPathToThisProject), "SetPath")]
        public string DestPath = string.Empty;
        
        public SDKManagerScripts[] ManagerScripts = new SDKManagerScripts[]
            {
                new SDKManagerScripts("ADs Manager","Ads"),
                new SDKManagerScripts("Analytics Manager","Analytics"),
                new SDKManagerScripts("GameConfigBase","Config"),
                new SDKManagerScripts("Game Manager","Game"),
                new SDKManagerScripts("Haptic Manager","Haptic"),
                new SDKManagerScripts("Input Manager","Input"),
                new SDKManagerScripts("Loading Manager","Loading"),
                new SDKManagerScripts("Pool Manager","Pool"),
                new SDKManagerScripts("Sound Manager","Sound"),
                new SDKManagerScripts("SplashScreen Manager","SplashScreen"),
                new SDKManagerScripts("Storage Manager","Storage"),
                new SDKManagerScripts("UI&HUD Manager","UI"),
                new SDKManagerScripts("Webtool Manager","Webtool")

            };
        public void SetPathToThisProject()
        {
            DestPath = Application.dataPath.Replace("Assets", "");
        }

        [Button]
        public void CopySDKFolder()
        {
            EditorApplication.LockReloadAssemblies();

            //IsLockedReloadAssemblies = true;

            EditorUtility.DisplayProgressBar("Copying Folders", "Please hold...", .8f);

            try
            {
                string fullSourcePath = $"{SDKPath}/Assets/_BrokenMugStudio_SDK";
                string fullDestPath = $"{DestPath}/Assets/_BrokenMugStudio_SDK";
                if (Directory.Exists(fullSourcePath))
                {
                    if (Directory.Exists(fullDestPath))
                    {
                        Directory.Delete(fullDestPath, true);
                    }

                    Utils.CopyFolder(fullSourcePath, fullDestPath, i_Overwrite: true);
                }

                Debug.LogError($"Copied SDK Folder Done.");

                EditorApplication.UnlockReloadAssemblies();
                //IsLockedReloadAssemblies = false;

/*                i_Callback.InvokeSafe();
*/            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message} Stack: {ex.StackTrace}");
            }

            EditorApplication.UnlockReloadAssemblies();
            //IsLockedReloadAssemblies = false;

            EditorUtility.ClearProgressBar();

            AssetDatabase.Refresh();
        }
        [Button]
        public void CopyGAFolder()
        {
            CopyGAResources();
            EditorApplication.LockReloadAssemblies();

            //IsLockedReloadAssemblies = true;

            EditorUtility.DisplayProgressBar("Copying Folders", "Please hold...", .8f);

            try
            {
                string fullSourcePath = $"{SDKPath}/Assets/GameAnalytics";
                string fullDestPath = $"{DestPath}/Assets/GameAnalytics";
                if (Directory.Exists(fullSourcePath))
                {
                    if (Directory.Exists(fullDestPath))
                    {
                        Directory.Delete(fullDestPath, true);
                    }

                    Utils.CopyFolder(fullSourcePath, fullDestPath, i_Overwrite: true);
                }

                Debug.LogError($"Copied GameAnalytics Folder Done.");

                EditorApplication.UnlockReloadAssemblies();
                //IsLockedReloadAssemblies = false;

                /*                i_Callback.InvokeSafe();
                */
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message} Stack: {ex.StackTrace}");
            }

            EditorApplication.UnlockReloadAssemblies();
            //IsLockedReloadAssemblies = false;

            EditorUtility.ClearProgressBar();

            AssetDatabase.Refresh();
            
        }
        [Button]
        public void CopyMaxSDKFolder()
        {
            EditorApplication.LockReloadAssemblies();
            CopyGAResources();

            //IsLockedReloadAssemblies = true;

            EditorUtility.DisplayProgressBar("Copying Folders", "Please hold...", .8f);

            try
            {
                string fullSourcePath = $"{SDKPath}/Assets/MaxSdk";
                string fullDestPath = $"{DestPath}/Assets/MaxSdk";
                if (Directory.Exists(fullSourcePath))
                {
                    if (Directory.Exists(fullDestPath))
                    {
                        Directory.Delete(fullDestPath, true);
                    }

                    Utils.CopyFolder(fullSourcePath, fullDestPath, i_Overwrite: true);
                }

                Debug.LogError($"Copied MaxSdk Folder Done.");

                EditorApplication.UnlockReloadAssemblies();
                //IsLockedReloadAssemblies = false;

                /*                i_Callback.InvokeSafe();
                */
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message} Stack: {ex.StackTrace}");
            }

            EditorApplication.UnlockReloadAssemblies();
            //IsLockedReloadAssemblies = false;

            EditorUtility.ClearProgressBar();

            AssetDatabase.Refresh();

        }

        public void CopyGAResources()
        {
            try
            {
                string fullSourcePath = $"{SDKPath}/Assets/Resources/GameAnalytics";
                string fullDestPath = $"{DestPath}/Assets/Resources/GameAnalytics";
                if (Directory.Exists(fullSourcePath))
                {
                    if (Directory.Exists(fullDestPath))
                    {
                        Directory.Delete(fullDestPath, true);
                    }

                    Utils.CopyFolder(fullSourcePath, fullDestPath, i_Overwrite: true);
                }

                Debug.LogError($"Copied GameAnalytics Resources Folder Done.");

                //IsLockedReloadAssemblies = false;

                /*                i_Callback.InvokeSafe();
                */
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message} Stack: {ex.StackTrace}");
            }

            
        }
        [Button]
        public void CopyManagersFolder()
        {
            EditorApplication.LockReloadAssemblies();

            //IsLockedReloadAssemblies = true;

            EditorUtility.DisplayProgressBar("Copying Folders", "Please hold...", .8f);

            try
            {
                string fullSourcePath = $"{SDKPath}/Assets/_BrokenMugStudio_SDK/Scripts/Managers_Base";
                string fullDestPath = $"{DestPath}/Assets/_BrokenMugStudio_SDK/Scripts/Managers_Base";
                if (Directory.Exists(fullSourcePath))
                {
                    if (Directory.Exists(fullDestPath))
                    {
                        Directory.Delete(fullDestPath, true);
                    }

                    Utils.CopyFolder(fullSourcePath, fullDestPath, i_Overwrite: true);
                }

                Debug.LogError($"Copied SDK Folder Done.");

                EditorApplication.UnlockReloadAssemblies();
                //IsLockedReloadAssemblies = false;

                /*                i_Callback.InvokeSafe();
                */
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message} Stack: {ex.StackTrace}");
            }

            EditorApplication.UnlockReloadAssemblies();
            //IsLockedReloadAssemblies = false;

            EditorUtility.ClearProgressBar();

            AssetDatabase.Refresh();
        }
        [Button]
        public void UpdateGameSettingsFolders()
        {
            EditorApplication.LockReloadAssemblies();

            //IsLockedReloadAssemblies = true;

            EditorUtility.DisplayProgressBar("Copying Folders", "Please hold...", .8f);

            try
            {
                string fullSourcePath = $"{SDKPath}/Assets/_BrokenMugStudio_SDK/Scripts/GameSettings";
                string fullDestPath = $"{DestPath}/Assets/_BrokenMugStudio_SDK/Scripts/GameSettings";
                //string safetyDestPath = $"{SafetyPathBase}/Assets/_BrokenMugStudio_SDK/Scripts/Managers_Base/" + DateTime.Now.ToString() + "/" + i_Folder;

                if (Directory.Exists(fullSourcePath))
                {

                    if (Directory.Exists(fullDestPath))
                    {
                        Directory.Delete(fullDestPath, true);
                    }

                    Utils.CopyFolder(fullSourcePath, fullDestPath, i_Overwrite: true);
                }

                Debug.LogError($"Copied SDK Folder Done.");

                EditorApplication.UnlockReloadAssemblies();
                //IsLockedReloadAssemblies = false;

                /*                i_Callback.InvokeSafe();
                */
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message} Stack: {ex.StackTrace}");
            }

            EditorApplication.UnlockReloadAssemblies();
            //IsLockedReloadAssemblies = false;

            EditorUtility.ClearProgressBar();

            AssetDatabase.Refresh();
        }
        public void UpdateManagerScriptsFolders(string i_Folder)
        {
            EditorApplication.LockReloadAssemblies();

            //IsLockedReloadAssemblies = true;

            EditorUtility.DisplayProgressBar("Copying Folders", "Please hold...", .8f);

            try
            {
                string fullSourcePath = $"{SDKPath}/Assets/_BrokenMugStudio_SDK/Scripts/Managers_Base/"+ i_Folder;
                string fullDestPath = $"{DestPath}/Assets/_BrokenMugStudio_SDK/Scripts/Managers_Base/" + i_Folder;
                string safetyDestPath = $"{SafetyPathBase}/Assets/_BrokenMugStudio_SDK/Scripts/Managers_Base/"+ DateTimeModifier + "/" + i_Folder;

                if (Directory.Exists(fullSourcePath))
                {
                    Utils.CopyFolder(fullDestPath, safetyDestPath, i_Overwrite: true);

                    if (Directory.Exists(fullDestPath))
                    {
                        Directory.Delete(fullDestPath, true);
                    }

                    Utils.CopyFolder(fullSourcePath, fullDestPath, i_Overwrite: true);
                }

                Debug.LogError($"Copied SDK Folder Done.");

                EditorApplication.UnlockReloadAssemblies();
                //IsLockedReloadAssemblies = false;

                /*                i_Callback.InvokeSafe();
                */
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message} Stack: {ex.StackTrace}");
            }

            EditorApplication.UnlockReloadAssemblies();
            //IsLockedReloadAssemblies = false;

            EditorUtility.ClearProgressBar();

            AssetDatabase.Refresh();
        }
        public void ExportManagerScriptsFolders(string i_Folder)
        {
            EditorApplication.LockReloadAssemblies();

            //IsLockedReloadAssemblies = true;

            EditorUtility.DisplayProgressBar("Copying Folders", "Please hold...", .8f);

            try
            {
                string fullSourcePath = $"{SDKPath}/Assets/_BrokenMugStudio_SDK/Scripts/Managers_Base/" + i_Folder;
                string fullDestPath = $"{DestPath}/Assets/_BrokenMugStudio_SDK/Scripts/Managers_Base/" + i_Folder;
                string safetyDestPath = $"{SafetyPathBase}/Assets/_BrokenMugStudio_SDK/Scripts/Managers_Base/" + DateTime.Now.ToString() + "/" + i_Folder;

                if (Directory.Exists(fullSourcePath))
                {
                    Utils.CopyFolder(fullSourcePath, safetyDestPath, i_Overwrite: true);

                    if (Directory.Exists(fullSourcePath))
                    {
                        Directory.Delete(fullSourcePath, true);
                    }

                    Utils.CopyFolder(fullDestPath, fullSourcePath, i_Overwrite: true);
                }

                Debug.LogError($"Exported " + i_Folder+" Folder Done.");

                EditorApplication.UnlockReloadAssemblies();
                //IsLockedReloadAssemblies = false;

                /*                i_Callback.InvokeSafe();
                */
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message} Stack: {ex.StackTrace}");
            }

            EditorApplication.UnlockReloadAssemblies();
            //IsLockedReloadAssemblies = false;

            EditorUtility.ClearProgressBar();

            AssetDatabase.Refresh();
        }
        [Serializable]
        public class SDKManagerScripts
        {
            public SDKManagerScripts (string i_Name,string i_Folder)
            {
                ManagerName = i_Name;
                Folder = i_Folder;
            }
            [ReadOnly]
            public string ManagerName;
            [ReadOnly]
            public string Folder;
            [Button("Update")]
            public void UpdateScript()
            {
                GameSettings.Instance.SDKUpdater.UpdateManagerScriptsFolders(Folder);
            }
            [Button("Export")]
            public void ExportScript()
            {
                GameSettings.Instance.SDKUpdater.ExportManagerScriptsFolders(Folder);
            }

        }
#endif

    }
}
