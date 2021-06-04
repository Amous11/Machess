#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    public class BrokenMugStudioSDKHotkeys 
    {
        [MenuItem(Constants.k_SDK_Title+"/Select GameConfig #%t")]
        public static void SelectGameConfg()
        {
            Selection.activeObject = GameConfig.Instance;
        }
        [MenuItem(Constants.k_SDK_Title + "/Select GameSettings #%g")]
        public static void SelectGameSettings()
        {
            Selection.activeObject = GameSettings.Instance;
        }
    }
    
}
#endif

