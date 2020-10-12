using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;

public class AnalyticsManager : MonoBehaviour
{
    public static void DidFinishLevel(int levelNumber) 
    {
        GA_Progression.NewEvent(GAProgressionStatus.Complete, "level", levelNumber.ToString(), new Dictionary <string, object>());
    }

}
