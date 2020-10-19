using Assets.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: clean up effects after use, why does it go flying if it hits the man?


public class ExplosionMod : Mod
{
    public Cooldown delay = new Cooldown(.1f);
    bool hasExploded = false;
    public float radius;
    public float force;
    public float damage;
    public ParticleSystem explosionEffect;

    public ExplosionMod(List<AttributeEntity> attributes, Projectile parentProjectile) : base(attributes, parentProjectile)
    {
        damage = Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1);
        radius = Attributes.GetAttributeValue(AttributeType.ModSpecificModifier2);
        force = Attributes.GetAttributeValue(AttributeType.ModSpecificModifier3);
    }


    protected override Mod CloneModChild()
    {
        return new ExplosionMod(Attributes.GetAttributes(), ParentProjectile);
    }

    protected override void ResetChild()
    {
    }

    protected override void UpdateChild()
    {

    }

    public override void CollisionTriggers(Collision collision)
    {
        if (delay.IsCool)
        {
            Explode();
            delay.StartCooldown();
        }
    }

    void Explode()
    {
        explosionEffect = Resources.Load<ParticleSystem>("TestExplosion");
        //Debug.Log(Resources.Load<GameObject>("TestCube"));
        Object.Instantiate(explosionEffect, ParentProjectile.transform.position, ParentProjectile.transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(ParentProjectile.transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, ParentProjectile.transform.position, radius);
            }

            Enemy enemy = nearbyObject.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
