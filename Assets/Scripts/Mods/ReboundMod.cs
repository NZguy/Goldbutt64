using Assets.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReboundMod : Mod
{
    private Cooldown flipTimer;
    private Cooldown delayTimer;
    private float accel = 2;
    private float delay = 1;
    Vector3 vel;
    Vector3 vel2;
    private float perc;
    private int state;
    private const int STATE_SETUP = 0;
    private const int STATE_START = 1;
    private const int STATE_SLOWING = 2;
    private const int STATE_REBOUND = 3;
    private const int STATE_SPEEDING = 4;
    private const int STATE_FINISH = 5;

    /// <summary>
    /// ModSpecificModifier1: The amount of time it takes to come to a stop.
    /// ModSpecificModifier2: The amount of time it takes to return to full speed.
    /// ModSpecificModifier3: Unused
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="parentProjectile"></param>
    public ReboundMod(List<AttributeEntity> attributes, Projectile parentProjectile) : base(attributes, parentProjectile)
    {
        flipTimer = new Cooldown(2 * Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1));
        delayTimer = new Cooldown(Attributes.GetAttributeValue(AttributeType.ModSpecificModifier2));

    }

    // Update is called once per frame

    protected override void UpdateChild()
    {
        switch (state)
        {
            case STATE_SETUP:
                delayTimer.StartCooldown();
                vel = new Vector3(ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity.x, ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity.y, ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity.z);
                vel2 = ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity;
                state = STATE_START;
                break;
            case STATE_START:
                if (delayTimer.IsCool)
                {
                    state = STATE_SLOWING;
                    flipTimer.StartCooldown();
                }
                break;
            case STATE_SLOWING:
                perc += Time.deltaTime;
                ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity = Vector3.Lerp(vel2, .1f * vel, perc / Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1));
                if (perc / Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1) > .9)
                {
                    state = STATE_REBOUND;
                }
                break;
            case STATE_REBOUND:
                ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity = -vel;
                perc = 0;
                state = STATE_SPEEDING;
                break;
            case STATE_SPEEDING:
                perc += Time.deltaTime;
                ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity = Vector3.Lerp(Vector3.zero, -vel, perc / (.5f * Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1)));
                break;
            case STATE_FINISH:
                Cycles++;
                break;
        }

    }

    protected override void ResetChild()
    {
        state = STATE_SETUP;
        flipTimer = new Cooldown(2 * Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1));
        delayTimer = new Cooldown(Attributes.GetAttributeValue(AttributeType.ModSpecificModifier2));
    }
}
