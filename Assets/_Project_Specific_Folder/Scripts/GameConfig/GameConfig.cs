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