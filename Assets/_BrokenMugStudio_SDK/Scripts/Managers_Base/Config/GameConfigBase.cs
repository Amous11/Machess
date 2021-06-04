
using BrokenMugStudioSDK.Animation;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
#if USING_ADDRESSABLES
using UnityEngine.AddressableAssets;
#endif


namespace BrokenMugStudioSDK
{
    public abstract class GameConfigBase : SingletonScriptableObject<GameConfig>
    {

    }

    [Serializable]
    public class DebugVariablesEditorBase
    {
        public bool IsDebugMode
        {
            get
            {
#if UNITY_EDITOR
                return m_EnableDebug;

#else
                return m_EnableDebug && GameSettings.Instance.BuildSettings.IsDeveloperMode;
#endif
            }
        }
        [SerializeField]
        private bool m_EnableDebug = true;
    }
    [Serializable]
    public class MenuVariablesEditorBase
    {
        public float TimeToShowNoThanks = 1f;
    }

    [Serializable]
    public class InputVariablesEditorBase
    {
        public Vector2 DragSensitivity = new Vector2(1, 1);
        public float InputThreshold = .1f;
    }

    [Serializable]
    public class LevelsVariablesEditorBase
    {

#if USING_ADDRESSABLES

        public const string k_LevelsPath = "Assets/_Project_Specific_Folder/Prefabs/Levels";
        public bool PreloadAllLevels = true;
        public bool KeepLevelsLoaded = true;

        public AssetReference[] PreloadAssets;
        public AssetReference[] Levels;
        public AssetReference CurrentLevel
        {
            get
            {
                if (Levels.Length > 0)
                {
                    return Levels[StorageManager.Instance.CurrentLevel % Levels.Length];
                }

                return null;
            }
        }
        public AssetReference PreviousLevel
        {
            get
            {
                if (Levels.Length > 0 && (StorageManager.Instance.CurrentLevel - 1) >= 0)
                {
                    return Levels[(StorageManager.Instance.CurrentLevel - 1) % Levels.Length];
                }

                return null;
            }
        }
#if UNITY_EDITOR
        [Button]
        public void LoadLevels()
        {
            string[] guids2 = AssetDatabase.FindAssets("Level t:Prefab", new[] { k_LevelsPath });

            Levels = new AssetReference[guids2.Length];
            for (int i = 0; i < guids2.Length; i++)
            {
                /*Debug.Log(AssetDatabase.GUIDToAssetPath(guids2[i]));
                string assetPath = AssetDatabase.GUIDToAssetPath(guids2[i]);

                UnityEngine.Object levelGameObjects = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
*/
                Levels[i] = new AssetReference(guids2[i]);
            }
        }
#endif
#elif !USING_ADDRESSABLES
        public const string k_LevelsPath = "Assets/_Project_Specific_Folder/Prefabs/Levels";
        public GameObject[] Levels;
        public int CurrentLevelIndex { get { return StorageManager.Instance.CurrentLevel % Levels.Length; } }
        public GameObject CurrentLevel
        {
            get
            {
                if(Levels.Length>0)
                {
                    return Levels[CurrentLevelIndex];
                }

                return null;
            }
        }

#if UNITY_EDITOR
        [Button]
        public void LoadLevels()
        {
            string[] guids2 = AssetDatabase.FindAssets("Level t:Prefab", new[] { k_LevelsPath });

            Levels = new GameObject[guids2.Length];
            for (int i = 0; i < guids2.Length; i++)
            {
                Debug.Log(AssetDatabase.GUIDToAssetPath(guids2[i]));
                string assetPath =  AssetDatabase.GUIDToAssetPath(guids2[i]);

                UnityEngine.Object levelGameObjects = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));

                Levels[i] = ((GameObject)levelGameObjects);
            }
        }
#endif
#endif
    }
    [Serializable]
    public class TweenVariablesEditorBase
    {


    }


    [Serializable]
    public class GamePlayVariablesEditorBase
    {
        public int LevelCompleteMoneyReward = 90;

    }

    [Serializable]
    public class HUDVariablesEditorBase
    {
        public CoinUIAnimData CoinUIAnimData;
        public float HideLevelCompleteDelay = .5f;
        public float ShowLevelCompleteDelay = 1f;
        public float ShowLevelFailedDelay = 1f;
        public ButtonJuice MainButtonJuice;
    }

    [Serializable]
    public class PlayerVariablesEditorBase
    {
    }

    [Serializable]
    public class CameraVariablesEditorBase
    {

    }
    [Serializable]
    public class ButtonJuice
    {
        [Title("OnDown")]
        public float DownDurration=.25f;
        public float ScaleDown = .85f;
        public Ease DownEase = Ease.InElastic;
        public bool DoVibrateDown = true;
        [Title("OnUp PunshScale")]
        public float UpShakeDurration = .25f;
        public float UpResetDurration = .1f;

        public float Strength = .5f;
        public int Vibro = 2;
        public float Elasticity = .1f;

        public bool DoVibrateUp = false;
        public Ease UpEase = Ease.InOutElastic;


    }
}



