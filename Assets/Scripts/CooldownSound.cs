using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownSound : ActionCooldown
{
    public AudioSource sound;

    public override void Start()
    {
        _cooldown = new Cooldown();
    }

    public CooldownSound() : base()
    {
        _cooldown.BaseCooldownTime = 0.23f;
    }

    public override void ActionOffCooldown()
    {
        sound.Play();
    }

    public override void ActionOnCooldown()
    {
        
    }
}
