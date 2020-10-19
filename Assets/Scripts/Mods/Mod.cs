using Assets.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mod
{
    /// <summary>
    /// Currently only used by SplitMod. This is the GameObject that's created when SplitMod splits.
    /// </summary>
    private GameObject _splitsInto;
    public GameObject SplitsInto
    {
        get
        {
            return _splitsInto;
        }
        set
        {
            foreach (Mod mod in ChildMods)
                mod.SplitsInto = value;
            _splitsInto = value;
        }
    }

    /// <summary>
    /// This is the projectile to modify.
    /// </summary>
    private Projectile _parentProjectile;
    public Projectile ParentProjectile
    {
        get
        {
            return _parentProjectile;
        }
        set
        {
            foreach (Mod mod in ChildMods)
                mod.ParentProjectile = value;
            _parentProjectile = value;
        }
    }

    /// <summary>
    /// If disabled, this mod won't be updated.
    /// Additionally, other mods may choose to remove this mod from their child list as in the case of SequenceMod.
    /// </summary>
    private bool _isEnabled = true;
    public bool IsEnabled
    {
        get
        {
            return _isEnabled;
        }
        set
        {
            foreach (Mod mod in ChildMods)
                mod.IsEnabled = value;
            _isEnabled = value;
        }
    }

    /// <summary>
    /// Cycles: A single cycle is the however long it takes for a Mod to complete it's task once.
    /// Example (Assume defaults are unchanged): 
    ///     SplitMod: Splitting once is a cycle.
    ///     SequenceMod: Starting from the beginning of it ChildMods list, iterating through and waiting for each mod to complete a cycle, is one cycle.
    ///     TurnMod: A single update tick is one cycle. The degree to which this turns the projectile varies depending on how fast the game is running (unless unity uses a fixed update time).
    /// </summary>
    public int Cycles { get; private set; }
    
    /// <summary>
    /// MaxCycles:
    ///     The number of Cycles to perform before this mod disables itself.
    ///     If set to -1 this mod will never disable itself due to cycle count.
    /// </summary>
    public int MaxCycles = -1;
    
    /// <summary>
    /// IterationsPerCycle:
    /// Modifies the number of completed tasks to count as a whole cycle.
    /// Example: 
    ///     SplitMod: Changing IterationsPerCycle on SplitMod to 5 will mean this mod will Split 5 times before the Cycle count is incremented.
    /// </summary>
    public int IterationsPerCycle = 1;
    private int currentIterationCount = 1;
    protected int CurrentIterationCount
    {
        get
        {
            return currentIterationCount;
        }
        set
        {
            currentIterationCount = value;
            if (currentIterationCount >= IterationsPerCycle)
            {
                currentIterationCount -= IterationsPerCycle;
                Cycles++;
            }
        }
    }


    public AttributesManagerLite Attributes;
    protected Rigidbody rb;
    public List<Mod> ChildMods;

    /// <summary>
    /// Recursively updates/sets necessary variables.
    /// </summary>
    /// <param name="parentProjectile"></param>
    /// <param name="splitsInto"></param>
    public void InitializeMods(Projectile parentProjectile, GameObject splitsInto)
    {
        SplitsInto = splitsInto;
        ParentProjectile = parentProjectile;
        foreach (Mod childMod in ChildMods)
            childMod.InitializeMods(parentProjectile, splitsInto);
    }

    public Mod(List<AttributeEntity> attributes, Projectile parentProjectile)
    {
        ChildMods = new List<Mod>();
        Attributes = new AttributesManagerLite();
        Attributes.AddAttribute(attributes);
        ParentProjectile = parentProjectile;
        Attributes.CalculateFinalValues();
    }

    public void OnCollision(Collision collision)
    {
        foreach (Mod mod in ChildMods)
            mod.OnCollision(collision);
    }

    public void OnProjectileDeath()
    {
        IsEnabled = false;
        foreach (Mod mod in ChildMods)
            mod.OnProjectileDeath();
    }

    /// <summary>
    /// Updates the this mod and all child mods.
    /// 
    /// Please only override Update() if you have a good reason to.
    /// Example: Sequence mod overrides Update() since it need explicit control over which child mods get updated when.
    /// </summary>
    public virtual void Update()
    {
        if (MaxCycles > -1 && Cycles >= MaxCycles)
            IsEnabled = false;   

        if (IsEnabled)
        {
            foreach (Mod child in ChildMods)
                child.Update();
            UpdateChild();
        }
    }

    /// <summary>
    /// UpdateChild() performs the unique update behaviour per mod.
    /// Update() is used to ensure general overhead is taken care of.
    /// </summary>
    protected abstract void UpdateChild();

    /// <summary>
    /// Resets this mod and all child mods.
    /// 
    /// Please only override Reset() if you have a good reason to.
    /// </summary>
    public void Reset()
    {
        ResetChild();
        foreach (Mod mod in ChildMods)
            mod.Reset();
    }

    /// <summary>
    /// ResetChild() performs the unique reset behaviour per mod.
    /// Reset() is used to ensure general overhead is taken care of.
    /// </summary>
    protected abstract void ResetChild();

    /// <summary>
    /// Deep clones this mod and all child mods
    /// Please only override Reset() if you have a good reason to.
    /// </summary>
    public Mod CloneMod()
    {
        Mod clone = CloneModChild();
        clone.Attributes = Attributes;
        clone.MaxCycles = MaxCycles;
        foreach (Mod mod in ChildMods)
        {
            clone.ChildMods.Add(mod.CloneMod());
        }
        return clone;
    }

    /// <summary>
    /// ResetChild() performs the unique reset behaviour per mod.
    /// Reset() is used to ensure general overhead is taken care of.
    /// </summary>
    protected abstract Mod CloneModChild();


    public virtual string GetDescription(int currentIndent = 0)
    {
        string description = this.GetType().Name;
        currentIndent++;
        foreach (Mod mod in ChildMods)
        {
            description += "\n";
            for (int i = 0; i < currentIndent; i++)
            {
                description += "\t";
            }
            description += mod.GetDescription(currentIndent);
        }
        return description;
    }


    public abstract void CollisionTriggers(Collision collision);
}
