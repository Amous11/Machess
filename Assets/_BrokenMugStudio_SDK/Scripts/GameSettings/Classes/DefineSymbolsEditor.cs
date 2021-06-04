using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace BrokenMugStudioSDK
{
    [Serializable]
    public class DefineSymbolsEditor 
    {
        public static readonly string[] DefaultDefineSymbols = { "ODIN_INSPECTOR", "ODIN_INSPECTOR_3", "MOREMOUNTAINS_NICEVIBRATIONS_RUMBLE", "MOREMOUNTAINS_NICEVIBRATIONS" };
        public bool UseAddressables = true;
        public bool UseAdjust = false;
        public string[] ExtraDefineSymobls;
        //Constants
#if UNITY_EDITOR
        [Button]
        public void UpdateDefineSymbols()
        {
            List<string> defines = new List<string>();
            defines.AddRange(DefaultDefineSymbols);
            if(UseAddressables)
            {
                defines.Add(Constants.k_UsingAddressablesDefine);
            }
            if (UseAdjust)
            {
                defines.Add(Constants.k_UsingAdjustDefine);
            }
            if(ExtraDefineSymobls!=null)
            {
                defines.AddRange(ExtraDefineSymobls);
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android| BuildTargetGroup.Standalone, defines.ToArray());
        }
#endif

    }
}
