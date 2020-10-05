using Assets.Scripts.Attributes;
using Assets.Scripts.Mods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class ModFactory
{

    private static Random _random = new Random();

    public static Mod PickARandomMod(List<AttributeEntity> attributes = null, Projectile parentProjectile = null)
    {
        switch(_random.Next(8))
        {
            case 0:
                return GetParentAngleMod(attributes, parentProjectile);
            case 1:
                return GetReboundMod(attributes, parentProjectile);
            case 2:
                return GetSequenceMod(attributes, parentProjectile);
            case 3:
                return GetSplitMod(attributes, parentProjectile);
            case 4:
                return GetTimerMod(attributes, parentProjectile);
            case 5:
                return GetWaveMod(attributes, parentProjectile);
            case 6:
                return GetRandomMod(attributes, parentProjectile);
            default:
                return GetTurnMod(attributes, parentProjectile);
        }
    }
    
    public static RandomMod GetRandomMod(List<AttributeEntity> attributes = null, Projectile parentProjectile = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(10), 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(180), 0));
        }

        RandomMod _mod = new RandomMod(attributes, parentProjectile, _random);
        return _mod;
    }

    public static ParentAngleMod GetParentAngleMod(List<AttributeEntity> attributes = null, Projectile parentProjectile = null, Transform targetTransform = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(10), 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(180), 0));
        }

        // Temp solution - Revealed bug: ParentAngleMod should handle (or disable self) if targetTransform is null.
        // Target object may be removed / die which may cause targetTransform to be null.
        if (targetTransform == null)
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ParentAngleMod _mod = new ParentAngleMod(attributes, parentProjectile, targetTransform);
        return _mod;
    }

    public static ReboundMod GetReboundMod(List<AttributeEntity> attributes = null, Projectile parentProjectile = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(2) + 0.1f, 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(2) + 0.1f, 0));
        }

        ReboundMod _mod = new ReboundMod(attributes, parentProjectile);
        return _mod;
    }

    public static SequenceMod GetSequenceMod(List<AttributeEntity> attributes = null, Projectile parentProjectile = null, int numberOfMods = -1)
    {
        if (numberOfMods < 0)
            numberOfMods = _random.Next(10)+1;

        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
        }

        SequenceMod _mod = new SequenceMod(attributes, parentProjectile);
        for (int i = 0; i < numberOfMods; i++)
        {
            Mod timerMod = GetTimerMod();
            timerMod.ChildMods.Add(PickARandomMod());
            _mod.ChildMods.Add(timerMod);
        }

        return _mod;
    }

    public static SplitMod GetSplitMod(List<AttributeEntity> attributes = null, Projectile parentProjectile = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(5), 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(20)+5, 0));
        }
        SplitMod _mod = new SplitMod(attributes, null, parentProjectile);
        _mod.MaxCycles = 1;
        return _mod;
    }

    public static TimerMod GetTimerMod(List<AttributeEntity> attributes = null, Projectile parentProjectile = null)
    {

        if(attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(5)+1, _random.Next(5)));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(5)+1, _random.Next(5)));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier3, _random.Next(5)+1, _random.Next(5)));
        }

        TimerMod _mod = new TimerMod(attributes, parentProjectile);

        return _mod;
    }

    public static WaveMod GetWaveMod(List<AttributeEntity> attributes = null, Projectile parentProjectile = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(10), 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(180), 0));
        }
        WaveMod _mod = new WaveMod(attributes, parentProjectile);

        return _mod;
    }

    public static TurnMod GetTurnMod(List<AttributeEntity> attributes = null, Projectile parentProjectile = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(360) - 180, 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(30), 0));
        }
        TurnMod _mod = new TurnMod(attributes, parentProjectile);
        return _mod;
    }

}
