using DG.Tweening;
using Hawkeen.Extentions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigBase : Singleton<GameConfig>
{
   
}

[Serializable]
public class DebugVariablesEditorBase
{
   
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
}
[Serializable]
public class TweenVariablesEditorBase
{
    

}


[Serializable]
public class GamePlayVariablesEditorBase 
{
 

}

[Serializable]
public class HUDVariablesEditorBase 
{
}

[Serializable]
public class PlayerVariablesEditorBase 
{
}

[Serializable]
public class CameraVariablesEditorBase
{

}



