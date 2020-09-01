using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionCooldown : ActionBase
{
    public abstract void ActionOffCooldown();
    public abstract void ActionOnCooldown();

    protected Cooldown _cooldown;

    public ActionCooldown()
    {
        _cooldown = new Cooldown();

    }

    public override void Start()
    {
        _cooldown = new Cooldown();
    }

    public override void Update()
    {
        _cooldown.Update();
    }

    public override void Act()
    {

        if (_cooldown.IsCool)
        {
            _cooldown.StartCooldown();
            ActionOffCooldown();
        }
        else
        {
            ActionOnCooldown();
            Debug.Log($"{this.name} is currently on cooldown. Remaining Time:{_cooldown.RemainingTime} ");
        }
    }
}
