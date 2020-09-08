﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour
{
    public float BaseCooldownTime;
    public float RemainingTime;

    public Cooldown()
    {
        BaseCooldownTime = 1; // temp
    }

    public void StartCooldown()
    {
        RemainingTime = BaseCooldownTime;
    }

    public void Update()
    {
        Debug.Log($"Remaining Time: {RemainingTime}");
        if (!IsCool)
            RemainingTime -= Time.deltaTime;
    }

    public bool IsCool
    {
        get
        {
            return RemainingTime <= 0;
        }
    }
}