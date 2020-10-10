using Assets.Scripts;
using Assets.Scripts.Attributes;
using Assets.Scripts.Mods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Gun : GameBase
{
    public Camera shootCam;
    public ParticleSystem bulletTrail;
    public AudioSource musicBox;

    public bool attackOffCooldown;

    public GameObject Projectile;
    public List<Mod> AttachedMods;

    public void Init(GameBase owner)
    {
        AttachedMods = new List<Mod>();
        AttachedMods.Add(ModFactory.GetSampleMod());
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

        List<Mod> mods = new List<Mod>();
        foreach(Mod mod in AttachedMods)
        {
            mods.Add(mod.CloneMod());
        }

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
}
