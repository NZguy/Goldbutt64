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

    public ReboundMod(List<AttributeEntity> attributes, Projectile parentProjectile) : base(attributes, parentProjectile)
    {
        flipTimer = new Cooldown(2 * Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1));
        delayTimer = new Cooldown(Attributes.GetAttributeValue(AttributeType.ModSpecificModifier2));

    }

    // Update is called once per frame

    public override void Update()
    {
        switch (state)
        {
            case STATE_SETUP:
                Debug.Log("STATE_SETUP");
                delayTimer.StartCooldown();
                vel = new Vector3(ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity.x, ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity.y, ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity.z);
                vel2 = ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity;
                state = STATE_START;
                break;
            case STATE_START:
                Debug.Log("STATE_START");
                if (delayTimer.IsCool)
                {
                    state = STATE_SLOWING;
                    flipTimer.StartCooldown();
                }
                break;
            case STATE_SLOWING:
                Debug.Log("STATE_SLOWING");
                perc += Time.deltaTime;
                ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity = Vector3.Lerp(vel2, .1f * vel, perc / Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1));
                if (perc / Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1) > .9)
                {
                    state = STATE_REBOUND;
                }
                break;
            case STATE_REBOUND:
                Debug.Log("STATE_REBOUND");
                ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity = -vel;
                perc = 0;
                state = STATE_SPEEDING;
                break;
            case STATE_SPEEDING:
                Debug.Log("STATE_SPEEDING");
                perc += Time.deltaTime;
                ParentProjectile.gameObject.GetComponent<Rigidbody>().velocity = Vector3.Lerp(Vector3.zero, -vel, perc / (.5f * Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1)));
                break;
            case STATE_FINISH:
                Debug.Log("STATE_FINISH");
                break;
        }

    }

    public override void Reset()
    {
        state = STATE_SETUP;
        flipTimer = new Cooldown(2 * Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1));
        delayTimer = new Cooldown(Attributes.GetAttributeValue(AttributeType.ModSpecificModifier2));
    }
}






//Debug.Log(vel);
//Debug.Log(-vel);
//Debug.Log(vel2);
//Debug.Log(-vel2);
/*
if (!hasStarted)
{
    StartTimer();
    hasStarted = true;
}

if (flipTimer.IsCool && !hasRebound)
{
    Debug.Log("Rebound");
    proj.GetComponent<Rigidbody>().velocity = -vel;
    hasRebound = true;
    //speedTimer.StartCooldown();
}
else if (slowTimer.IsCool && !hasRebound)
{
    Debug.Log("Slow");
    perc += Time.deltaTime;

    //proj.GetComponent<Rigidbody>().velocity = Vector3.SmoothDamp(vel, Vector3.zero, ref vel2, 2f);
    //proj.GetComponent<Rigidbody>().velocity = Vector3.Lerp(vel2, .1f*vel, 1f*Time.deltaTime);
    proj.GetComponent<Rigidbody>().velocity = Vector3.Lerp(vel2, .1f * vel, perc/1f);
    isSlow = true;
}
else if (hasRebound)
{
    Debug.Log("Fast");

    //proj.GetComponent<Rigidbody>().velocity = Vector3.SmoothDamp(vel, -vel, ref vel2, 2f);
    proj.GetComponent<Rigidbody>().velocity = Vector3.Lerp(Vector3.zero, -vel, 2f * Time.deltaTime);
    //proj.GetComponent<Rigidbody>().velocity = vel;
    isFast = true;
}
*/
