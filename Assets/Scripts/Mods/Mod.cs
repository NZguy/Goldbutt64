using Assets.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mod
{
    public GameObject splitsInto;
    public Projectile ParentProjectile;
    public AttributesManagerLite Attributes;

    public Mod(List<AttributeEntity> attributes, Projectile parentProjectile)
    {
        Attributes = new AttributesManagerLite();
        Attributes.AddAttribute(attributes);
        ParentProjectile = parentProjectile;
        Attributes.CalculateFinalValues();
    }


    // Update is called once per frame
    public abstract void Update();

    public abstract void Reset();
}
