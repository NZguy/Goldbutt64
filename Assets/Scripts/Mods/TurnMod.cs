using Assets.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMod : Mod
{
    /// <summary>
    /// ModSpecificModifier1: The number of degrees to turn per second
    /// ModSpecificModifier2: Unused
    /// ModSpecificModifier3: Unused
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="parentProjectile"></param>
    public TurnMod (List<AttributeEntity> attributes, Projectile parentProjectile) : base(attributes, parentProjectile) {}

    protected override void ResetChild()
    {
        rb = ParentProjectile.GetComponent<Rigidbody>();
    }

    protected override void UpdateChild()
    {
        rb.velocity = Quaternion.AngleAxis(Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1) * Time.deltaTime, Vector3.up) * rb.velocity;
        CurrentIterationCount++;
    }

    protected override Mod CloneModChild()
    {
        TurnMod newMod = new TurnMod(Attributes.GetAttributes(), ParentProjectile);
        return newMod;
    }

    public override void CollisionTriggers(Collision collision)
    {
    }
}
