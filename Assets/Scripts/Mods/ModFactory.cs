using Assets.Scripts.Attributes;
using Assets.Scripts.Mods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Used for creating Mods
/// 
/// All GetMod methods have reasonable sample/test attributes. Pass in your own attributes list for more control over mod behaviour.
/// GetMod methods probably need to be more standardized. Currently, SequenceMod generates a new SequenceMod with some number of child mods. The person calling GetSequenceMod has no control over which mods are added.
///         Most GetMod methods just create a plain mod with default attributes if none are provided.
/// 
/// SampleMod provide a quick demonstration of potential uses.
/// 
/// 
/// </summary>
public static class ModFactory
{
    private static Random _random = new Random();

    #region Sample Mods

    public static Mod GetSampleMod (int sampleModIndex = -1)
    {
        if (sampleModIndex < 0 || sampleModIndex >= SampleMods.Length)
            sampleModIndex = _random.Next(SampleMods.Length);
        return SampleMods[sampleModIndex];
    }

    private static Mod[] SampleMods = new Mod[]
    {
        Sample_Explosion()
        //Sample_ShotgunSpinner(),
        //Sample_Spinner()
    };

    /// <summary>
    /// Causes projectile to explode on impact.
    /// </summary>
    /// <returns></returns>
    private static Mod Sample_Explosion()
    {
        //ExplosionMod exp = new ExplosionMod(new List<AttributeEntity>(), null);
        ExplosionMod exp = GetExplosionMod();

        return  exp;
    }

    /// <summary>
    /// Travels some distance then spins in a circle while shooting projectiles.
    /// </summary>
    /// <returns></returns>
    private static Mod Sample_ShotgunSpinner()
    {
        // First Stage: Projectile will travel forward until distanceTimer runs out of time.
        //      - Upon distanceTimer completing, it will be disabled and removed from parent.
        // Second Stage: turnMod will start turning the projectile.
        //      - Simultaneously, shotgunFireTimer will start counting down. Upon timer shotgun will execute and 
        //        spawn new projectiles that will straight out from the parent projectile (or at an offset angle).
        //      - This stage will be repeated until the projectile despawns.

        SequenceMod parent = new SequenceMod(new List<AttributeEntity>(), null);

        // This timer determines how far the projectile should travel before starting to circle and shotgun.
        // It does this by occupying the first SeqeuenceMod slot. No other Mods will by updated until this one completes a cycle.
        List<AttributeEntity> attributes = new List<AttributeEntity>();
        attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, (float)(_random.Next(3) * _random.NextDouble()) + 1, 0));
        TimerMod distanceTimer = GetTimerMod(attributes);
        distanceTimer.MaxCycles = 1;
        //^^ MaxCycles set to 1 so that as soon as the timer runs out it's removed from parent SequenceMod and no longer updated.


        // This TurnMod + TimerMod makes the projectile spin in a circle endlessly.
        // The projectile will turn for the duration of TimerMod timer then move to SplitMod for a single cycle before reseting.
        attributes = new List<AttributeEntity>();
        attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, 100, 0));
        TurnMod turny = GetTurnMod(attributes);

        attributes = new List<AttributeEntity>();
        attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, (float)(_random.Next(3) * _random.NextDouble()) + 0.05f, 0));
        attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, 0, 0));
        attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier3, 0, 0));
        TimerMod timer = GetTimerMod(attributes, turny);
        // ----------------------------------------------------------------------------------------------

        parent.ChildMods.Add(distanceTimer);
        parent.ChildMods.Add(timer);
        parent.ChildMods.Add(GetSplitMod());
        return parent;
    }
    
    /// <summary>
    /// Spins in randomly sized circles.
    /// 
    /// Alternative Solution:
    ///     Instead of using TimerMods we could have modified the IterationsPerCycle property.
    /// </summary>
    /// <returns></returns>
    private static Mod Sample_Spinner()
    {
        SequenceMod parent = new SequenceMod(new List<AttributeEntity>(), null);


        // Add 20 TimerMod/TurnMods to parent SequenceMod.
        // Each TimerMod has a TurnMod as a child that is enabled for a random about of time.
        // After that time expires, the next TimerMod/TurnMod pair are updated.
        for (int i = 0; i < 20; i++)
        {
            List<AttributeEntity> attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(500), 0));
            TurnMod turny = GetTurnMod(attributes);

            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, ((float)_random.NextDouble() + 0.5f), 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, 0, 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier3, 0, 0));
            TimerMod timer = GetTimerMod(attributes, turny);

            parent.ChildMods.Add(timer);
        }
        return parent;
    }

    #endregion

    /// <summary>
    /// Generates a Random mod
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="parentProjectile"></param>
    /// <returns>A Random mod</returns>
    public static Mod PickARandomMod(List<AttributeEntity> attributes = null, Projectile parentProjectile = null)
    {
        switch(_random.Next(9))
        {
            case 0:
                return GetParentAngleMod(attributes);
            case 1:
                return GetReboundMod(attributes);
            case 2:
                return GetSequenceMod(attributes);
            case 3:
                return GetSplitMod(attributes);
            case 4:
                return GetTimerMod(attributes);
            case 5:
                return GetWaveMod(attributes);
            case 6:
                return GetRandomMod(attributes);
            case 7:
                return GetExplosionMod(attributes);
            default:
                return GetTurnMod(attributes);
        }
    }

    public static ExplosionMod GetExplosionMod(List<AttributeEntity> attributes = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            // Damage
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(50), 0));
            // Radius
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(25), 0));
            // Force
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier3, _random.Next(150), 0));
        }

        ExplosionMod _mod = new ExplosionMod(attributes, null);
        return _mod;
    }
    
    public static RandomMod GetRandomMod(List<AttributeEntity> attributes = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(10), 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(180), 0));
        }

        RandomMod _mod = new RandomMod(attributes, null, _random);
        return _mod;
    }

    public static ParentAngleMod GetParentAngleMod(List<AttributeEntity> attributes = null, Transform targetTransform = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(10), 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(180), 0));
        }

        // Temp solution - Reveals bug: ParentAngleMod should handle (or disable self) if targetTransform is null.
        // Target object may be removed / die which may cause targetTransform to be null.
        if (targetTransform == null)
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ParentAngleMod _mod = new ParentAngleMod(attributes, null, targetTransform);
        return _mod;
    }

    public static ReboundMod GetReboundMod(List<AttributeEntity> attributes = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(2) + 0.1f, 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(2) + 0.1f, 0));
        }

        ReboundMod _mod = new ReboundMod(attributes, null);
        return _mod;
    }

    public static SequenceMod GetSequenceMod(List<AttributeEntity> attributes = null, int numberOfMods = -1)
    {
        if (numberOfMods < 0)
            numberOfMods = _random.Next(10)+1;

        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
        }

        SequenceMod _mod = new SequenceMod(attributes, null);
        for (int i = 0; i < numberOfMods; i++)
        {
            Mod timerMod = GetTimerMod();
            timerMod.ChildMods.Add(PickARandomMod());
            _mod.ChildMods.Add(timerMod);
        }

        return _mod;
    }

    /// <summary>
    /// Creates a new SplitMod
    /// 
    /// Note:
    ///     No default MaxCycle set. Adding this mod without some other limiting parent mod (like TimerMod) will result in new projectiles being created continuously. 
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="parentProjectile"></param>
    /// <returns></returns>
    public static SplitMod GetSplitMod(List<AttributeEntity> attributes = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(5) + 1, 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(20)+5, 0));
        }
        SplitMod _mod = new SplitMod(attributes, null, null);
        return _mod;
    }

    public static TimerMod GetTimerMod(List<AttributeEntity> attributes = null, Mod childMod = null)
    {

        if(attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(5)+1, _random.Next(5)));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(5)+1, _random.Next(5)));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier3, _random.Next(5)+1, _random.Next(5)));
        }

        TimerMod _mod = new TimerMod(attributes, null, childMod);

        return _mod;
    }

    public static WaveMod GetWaveMod(List<AttributeEntity> attributes = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(10), 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(180), 0));
        }
        WaveMod _mod = new WaveMod(attributes, null);

        return _mod;
    }

    public static TurnMod GetTurnMod(List<AttributeEntity> attributes = null)
    {
        if (attributes == null)
        {
            attributes = new List<AttributeEntity>();
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(360) - 180, 0));
            attributes.Add(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(30), 0));
        }
        TurnMod _mod = new TurnMod(attributes, null);
        return _mod;
    }

}
