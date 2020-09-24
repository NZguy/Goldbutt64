using Assets.Scripts;
using Assets.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : GameBase
{
    public Camera shootCam;
    public ParticleSystem bulletTrail;
    public AudioSource musicBox;

    public bool attackOffCooldown;

    public GameObject Projectile;

    public void Init(GameBase owner)
    {
        ShareAttributes(owner);
        UpdateAttributes();
    }

    private void Start()
    {
        attackOffCooldown = true;
        //AddAttribute(new AttributeEntity(AttributeType.ProjectileDamage, 10, 0));
        //AddAttribute(new AttributeEntity(AttributeType.ProjectileBounce, 1, 0));
        //AddAttribute(new AttributeEntity(AttributeType.ProjectileSpeed, 10, 0));
        //AddAttribute(new AttributeEntity(AttributeType.RateOfFire, 1, 0));
        //AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1, 0));
        //AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 15, 0));
        //AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier3, 0, 0));
        //UpdateAttributes();
    }

    public void Attack(bool isClick)
    {
        if (attackOffCooldown)
            StartCoroutine(AttackLoop());
        else if (isClick)
            musicBox.GetComponent<MusicBox>().playCoolSound();
    }

    public IEnumerator AttackLoop()
    {
        attackOffCooldown = false;
        musicBox.GetComponent<MusicBox>().playShootSound();
        
        //bulletTrail.Play();

        GameObject bullet = Instantiate(Projectile, this.transform.position, this.transform.rotation) as GameObject;
        Projectile projectile = bullet.GetComponent<Projectile>();

        // Example mods - Nest SplitMods to demonstrate passing mods to child projectiles.
        // So child projectiles no longer recieve the same mods as their parent.
        SplitMod sMod1 = new SplitMod(GetAttributes(), projectile.splitsInto, projectile);
        sMod1.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1, 0));
        sMod1.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 90, 0));
        sMod1.Attributes.CalculateFinalValues();

        SplitMod sMod2 = new SplitMod(GetAttributes(), projectile.splitsInto, projectile);
        sMod2.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1, 0));
        sMod2.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, -90, 0));
        sMod2.Attributes.CalculateFinalValues();

        SplitMod sMod3 = new SplitMod(GetAttributes(), projectile.splitsInto, projectile);
        sMod3.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1, 0));
        sMod3.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 90, 0));
        sMod3.Attributes.CalculateFinalValues();

        SplitMod sMod4 = new SplitMod(GetAttributes(), projectile.splitsInto, projectile);
        sMod4.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1, 0));
        sMod4.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, -90, 0));
        sMod4.Attributes.CalculateFinalValues();

        SplitMod sMod5 = new SplitMod(GetAttributes(), projectile.splitsInto, projectile);
        sMod5.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1, 0));
        sMod5.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 90, 0));
        sMod5.Attributes.CalculateFinalValues();

        SplitMod sMod6 = new SplitMod(GetAttributes(), projectile.splitsInto, projectile);
        sMod6.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1, 0));
        sMod6.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, -90, 0));
        sMod6.Attributes.CalculateFinalValues();

        SplitMod sMod7 = new SplitMod(GetAttributes(), projectile.splitsInto, projectile);
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1, 0));
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 90, 0));
        sMod7.Attributes.CalculateFinalValues();

        sMod6.ChildMods.Add(sMod7);
        sMod5.ChildMods.Add(sMod6);
        sMod4.ChildMods.Add(sMod5);
        sMod3.ChildMods.Add(sMod4);
        sMod2.ChildMods.Add(sMod3);
        sMod1.ChildMods.Add(sMod2);

        List<AttributeEntity> temp = new List<AttributeEntity>();
        temp.AddRange(GetAttributes());
        temp.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, 1f, 0));
        temp.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, .5f, 0));
        sMod1.ChildMods.Add(new ReboundMod(temp, projectile));

        List<Mod> mods = new List<Mod>();
        mods.Add(sMod1);
        projectile.Init(GetAttributes(), mods);

        if (GetAttributeValue(AttributeType.RateOfFire) <= 0)
        {
            attackOffCooldown = false;
            yield return new WaitForSeconds(0.0001f);
        }
        else
        {
            yield return new WaitForSeconds(1f / GetAttributeValue(AttributeType.RateOfFire));
            attackOffCooldown = true;
        }

        
    }

    private SplitMod CreateSplit(Projectile projectile)
    {
        SplitMod sMod7 = new SplitMod(GetAttributes(), projectile.splitsInto, projectile);
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1, 0));
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 15, 0));
        sMod7.Attributes.CalculateFinalValues();
        return sMod7;
    }
}
