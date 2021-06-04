using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class AnalyticsEditor 
{
    public bool EnableAdjust = true;
    [ShowIf(nameof(EnableAdjust))]
    public string AdjustToken = "YOUR_ANDROID_APP_TOKEN_HERE";
    public bool LogFacebookEvents = true;
    public bool LogGameAnalyticsEvents = true;
}
