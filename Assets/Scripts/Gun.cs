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

        //sMod6.ChildMods.Add(sMod7);
        //sMod5.ChildMods.Add(sMod6);
        //sMod4.ChildMods.Add(sMod5);
        //sMod3.ChildMods.Add(sMod4);
        //sMod2.ChildMods.Add(sMod3);
        //sMod1.ChildMods.Add(sMod2);

        //List<AttributeEntity> temp = new List<AttributeEntity>();
        //temp.AddRange(GetAttributes());
        //temp.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, 1f, 0));
        //temp.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, .5f, 0));
        //sMod1.ChildMods.Add(new ReboundMod(temp, projectile));

        float timeModifier1 = 1f;


        SequenceMod sequenceMod = new SequenceMod(GetAttributes(), projectile);
        TimerMod delayTimer = new TimerMod(GetAttributes(), projectile);
        delayTimer.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, timeModifier1, 0));
        delayTimer.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, timeModifier1, 0));

        sequenceMod.ChildMods.Add(delayTimer);

        SplitMod sMod5 = new SplitMod(GetAttributes(), projectile.splitsInto, projectile);
        sMod5.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 5, 0));
        sMod5.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 15, 0));
        sequenceMod.ChildMods.Add(sMod5);


        TurnMod turnMod = new TurnMod(GetAttributes(), projectile);
        turnMod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 360, 0));
        TimerMod turnTimer = new TimerMod(GetAttributes(), projectile, turnMod);
        turnTimer.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, timeModifier1, 0));
        turnTimer.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, timeModifier1, 0));

        sequenceMod.ChildMods.Add(turnTimer);


        TimerMod delayTimer2 = new TimerMod(GetAttributes(), projectile);
        delayTimer2.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, timeModifier1, 0));
        delayTimer2.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, timeModifier1, 0));
        sequenceMod.ChildMods.Add(delayTimer2);


        TurnMod turnMod2 = new TurnMod(GetAttributes(), projectile);
        turnMod2.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, -360, 0));
        TimerMod turnTimer2 = new TimerMod(GetAttributes(), projectile, turnMod2);
        turnTimer2.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, timeModifier1, 0));
        turnTimer2.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, timeModifier1, 0));
        sequenceMod.ChildMods.Add(turnTimer2);


        List<Mod> mods = new List<Mod>();
        mods.Add(sequenceMod);
        //mods.Add(CreateTimerMod(projectile));

        //mods.Add(CreateWaveMod(projectile));
        //mods.Add(CreateTurnMod(projectile));
        //mods.Add(CreateReboundMod(projectile));
        //mods.Add(CreateWaveMod(projectile));
        //mods.Add(CreateTurnMod(projectile));
        //mods.Add(CreateSplit(projectile));
        //mods.Add(CreateRandomMod(projectile));
        //mods.Add(CreateRandomMod(projectile));
        //mods.Add(CreateRandomMod(projectile));
        //mods.Add(CreateRandomMod(projectile));


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

    Random random = new Random();
    private RandomMod CreateRandomMod(Projectile projectile)
    {
        RandomMod sMod7 = new RandomMod(GetAttributes(), projectile, random);
        sMod7.Attributes.CalculateFinalValues();
        return sMod7;
    }

    private TimerMod CreateTimerMod(Projectile projectile)
    {
        TimerMod sMod7 = new TimerMod(GetAttributes(), projectile, CreateTurnMod(projectile));
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1f, 0));
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 1, 0));
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier3, .25f, 0));
        sMod7.Attributes.CalculateFinalValues();
        return sMod7;
    }

    private ParentAngleMod CreateParentAngleMod(Projectile projectile)
    {
        ParentAngleMod sMod7 = new ParentAngleMod(GetAttributes(), projectile, this.transform);
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 5f, 0));
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 1, 0));
        sMod7.Attributes.CalculateFinalValues();
        return sMod7;
    }

    private ReboundMod CreateReboundMod(Projectile projectile)
    {
        ReboundMod sMod7 = new ReboundMod(GetAttributes(), projectile);
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1, 0));
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 1, 0));
        sMod7.Attributes.CalculateFinalValues();
        return sMod7;
    }

    private SplitMod CreateSplit(Projectile projectile)
    {
        SplitMod sMod7 = new SplitMod(GetAttributes(), projectile.splitsInto, projectile);
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 1, 0));
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 15, 0));
        sMod7.Attributes.CalculateFinalValues();
        return sMod7;
    }

    private TurnMod CreateTurnMod(Projectile projectile)
    {
        TurnMod sMod7 = new TurnMod(GetAttributes(), projectile);
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 200, 0));
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 15, 0));
        sMod7.Attributes.CalculateFinalValues();
        return sMod7;
    }

    private WaveMod CreateWaveMod(Projectile projectile)
    {
        WaveMod sMod7 = new WaveMod(GetAttributes(), projectile);
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, 5f, 0));
        sMod7.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, 180, 0));
        sMod7.Attributes.CalculateFinalValues();
        return sMod7;
    }
}
