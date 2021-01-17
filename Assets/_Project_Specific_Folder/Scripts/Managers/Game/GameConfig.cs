using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


}

[Serializable]
public class DebugVariablesEditor: DebugVariablesEditorBase
{

}

[Serializable]
public class InputVariablesEditor: InputVariablesEditorBase
{
}

[Serializable]
public class LevelsVariablesEditor: LevelsVariablesEditorBase
{
}
[Serializable]
public class TweenVariablesEditor: TweenVariablesEditorBase
{


}


[Serializable]
public class GamePlayVariablesEditor: GamePlayVariablesEditorBase
{


}

[Serializable]
public class HUDVariablesEditor: HUDVariablesEditorBase
{
}

[Serializable]
public class PlayerVariablesEditor: PlayerVariablesEditorBase
{
}

[Serializable]
public class CameraVariablesEditor: CameraVariablesEditorBase
{

}
