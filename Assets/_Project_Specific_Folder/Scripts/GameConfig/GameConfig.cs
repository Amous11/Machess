using BrokenMugStudioSDK.Shop;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrokenMugStudioSDK
{
    [CreateAssetMenu(fileName = "BrokenMugStudio/GameConfig")]

    public class GameConfig : GameConfigBase
    {
        public DebugVariablesEditor Debug = new DebugVariablesEditor();
        public InputVariablesEditor Input = new InputVariablesEditor();
        public GamePlayVariablesEditor GamePlay = new GamePlayVariablesEditor();
        public LevelsVariablesEditor Levels = new LevelsVariablesEditor();
        public HUDVariablesEditor HUD = new HUDVariablesEditor();
        public PlayerVariablesEditor Player = new PlayerVariablesEditor();
        public CameraVariablesEditor Camera = new CameraVariablesEditor();
        public TweenVariablesEditor Tweens = new TweenVariablesEditor();
        public MenuVariablesEditor Menus = new MenuVariablesEditor();
        public ShopVariablesEditor Shop = new ShopVariablesEditor();

    }

    [Serializable]
    public class DebugVariablesEditor : DebugVariablesEditorBase
    {

    }
    [Serializable]
    public class MenuVariablesEditor : MenuVariablesEditorBase
    {

    }

    [Serializable]
    public class InputVariablesEditor : InputVariablesEditorBase
    {
    }
    [Serializable]
    public class LevelsVariablesEditor : LevelsVariablesEditorBase
    {
        
    }
    [Serializable]
    public class TweenVariablesEditor : TweenVariablesEditorBase
    {
       
    }

    [Serializable]
    public class GamePlayVariablesEditor : GamePlayVariablesEditorBase
    {
        public int BonusPerKillMoney = 10;
        public DiceSettings DiceSettings;
        public PieceData[] PieceSettings;
        public PieceData GetPieceData(ePieceTypes i_Type)
        {
            for(int i=0;i< PieceSettings.Length;i++)
            {
                if(PieceSettings[i].Type== i_Type)
                {
                    return PieceSettings[i];
                }
            }
            return null;
        }
    }
    [Serializable]
    public class PieceData
    {
        public ePieceTypes Type;
        public Vector3Int[] Moves;
    }
    [Serializable]
    public class DiceSettings
    {
        public float ThrowVelocity=5;
        public float ThrowAngularVelocity = 20;
    }
    [Serializable]
    public class HUDVariablesEditor : HUDVariablesEditorBase
    {
        

    }

    [Serializable]
    public class PlayerVariablesEditor : PlayerVariablesEditorBase
    {
    }

    [Serializable]
    public class CameraVariablesEditor : CameraVariablesEditorBase
    {
        public CameraMouvementsData CameraMouvements;
        public CameraZoomData ZoomData;
        public CameraPositionsData CameraPositions;
    }
   

   
}