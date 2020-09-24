using Assets.Scripts;
using Assets.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SplitMod : Mod
{
    // ModSpecificModifier1 == Number of Times to split projectile
    // ModSpecificModifier2 == Angle between splits
    // ModSpecificModifier3 == Unused
   
    public Cooldown timer;
    private GameBase ParentGun;
    public List<Mod> ChildMods;

    public SplitMod (List<AttributeEntity> attributes,  GameObject split, Projectile parentProjectile) : base(attributes, parentProjectile)
    {
        ChildMods = new List<Mod>();
        splitsInto = split;
        timer = new Cooldown(10f, 2f);
    }

    public override void Update()
    {
        if (timer.IsCool)
        {

            for (int i = 1; i <= Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1); i++)
            {
                Quaternion newRotation = ParentProjectile.transform.rotation;
                newRotation *= Quaternion.Euler(0, Attributes.GetAttributeValue(AttributeType.ModSpecificModifier2) * i, 0);
                GameObject newProjectile = GameObject.Instantiate(splitsInto, ParentProjectile.transform.position, newRotation);
                newProjectile.GetComponent<Projectile>().Init(Attributes.GetAttributes(), ChildMods);
            }
            timer.StartCooldown();
        }
    }

    public override void Reset ()
    {
        timer = new Cooldown(10f, 2f);
    }
}

