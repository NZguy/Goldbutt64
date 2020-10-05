using Assets.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mod
{

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

    public AttributesManagerLite Attributes;
    protected Rigidbody rb;
    public List<Mod> ChildMods;
    public int Cycles { get; protected set; }
    public int MaxCycles = -1;


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

    public virtual void Update()
    {
        if (MaxCycles > -1 && Cycles > MaxCycles)
            IsEnabled = false;   

        if (IsEnabled)
        {
            foreach (Mod mod in ChildMods)
                mod.Update();
            UpdateChild();
        }
    }
    protected abstract void UpdateChild();

    public void Reset()
    {
        ResetChild();
        foreach (Mod mod in ChildMods)
            mod.Reset();
    }
    protected abstract void ResetChild();

    public virtual string GetDescription(int currentIndent = 0)
    {
        string description = this.GetType().Name;
        currentIndent++;
        foreach (Mod mod in ChildMods)
        {
            description += "\n";
            for(int i = 0; i < currentIndent; i++)
            {
                description += "\t";
            }
            description += mod.GetDescription(currentIndent);
        }
        return description;
    }

    public abstract Mod CloneMod();

}
