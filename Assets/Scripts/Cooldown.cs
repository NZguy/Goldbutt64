﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown
{
    public float BaseCooldownTime;
    private float CompletionTime;
    private float StartTime;

    public float RemainingTime {
        get
        {
            return CompletionTime - Time.realtimeSinceStartup;
        }
    }


    //private float time;

    public Cooldown(float val, float delayStart = 0)
    {
        StartTime = Time.realtimeSinceStartup;
        CompletionTime = StartTime + delayStart;
        BaseCooldownTime = val; // temp
    }

    public Cooldown()
    {
        BaseCooldownTime = 1; // temp
    }

    public void StartCooldown()
    {
        StartTime = Time.realtimeSinceStartup;
        CompletionTime = StartTime + BaseCooldownTime;
    }
    
    public bool IsCool
    {
        get
        {
            return RemainingTime <= 0;
        }
    }



    #region
    /// <summary>
    /// 
    /// </summary>
    /// <returns>The max value between the seconds remaining until cool or 0</returns>
    public float SecondsRemaining()
    {
        return Mathf.Max(CompletionTime, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>The completion percent for this cooldown object</returns>
    public float PercentComplete()
    {
        return Mathf.Min((Time.realtimeSinceStartup / CompletionTime) * 100, 100);
    }
    #endregion

}