using Assets.Scripts;
using Assets.Scripts.Attributes;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : GameBase
{
    private Cooldown cleanUpTimer;
    public GameObject splitsInto;
    private int bounces = 0;

    // true if hitting an agent counts as bounce
    public bool agentBounce = true;
    public bool splitOnColl = false;
    public bool splitOnFire = false;
    public List<Mod> mods;

    private Vector3 oldvelocity;
    public void Init(List<AttributeEntity> attributes, List<Mod> mods)
    {
        foreach (Mod mod in mods)
        {
            mod.InitializeMods(this, splitsInto);
            mod.Reset();
        }

        // Adding attributes instead of sharing so that they're locked in. 
        // Once a projectile is created, the attached attributes shouldn't change.
        AddAttribute(attributes);
        UpdateAttributes();
        this.GetComponent<Rigidbody>().velocity = this.transform.forward * GetAttributeValue(AttributeType.ProjectileSpeed);
        this.GetComponent<Rigidbody>().mass = GetAttributeValue(AttributeType.ProjectileMass);
        this.mods = mods;
    }

    void Start()
    {
        cleanUpTimer = new Cooldown(15f, 15f);
        oldvelocity = Vector3.zero;
    }

    void Update()
    {
        // Forces the projectile to face it's move direction
        // May want to make this smoother later if we have none spherical bullets
        transform.LookAt(this.GetComponent<Rigidbody>().velocity + transform.position);
        for(int i = 0; i < mods.Count; i++)
        {
            mods[i].Update();
        }
        if (cleanUpTimer.IsCool) Die();
        if (bounces >= GetAttributeValue(AttributeType.ProjectileBounce)) Die();
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (Mod mod in mods)
            mod.OnCollision(collision);

        this.GetComponent<Rigidbody>().velocity = GetAttributeValue(AttributeType.ProjectileSpeed) * (this.GetComponent<Rigidbody>().velocity.normalized);

        Enemy enemy = collision.transform.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(GetAttributeValue(AttributeType.ProjectileDamage));
            if (agentBounce) bounces += 1;
        }
        else bounces += 1;
    }

    public override void Die()
    {
        foreach (Mod mod in mods)
            mod.OnProjectileDeath();
        base.Die();
    }
}