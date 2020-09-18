﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown
{
    public float BaseCooldownTime;
    public float RemainingTime;

    public Cooldown(float val, bool startCool)
    {
        if (startCool) RemainingTime = val;
        BaseCooldownTime = val; // temp
    }

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