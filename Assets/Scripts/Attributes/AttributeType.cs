using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;


public enum AttributeType
{
    [TypeInfoAttribute(DisplayName = "Movement Speed", Description = "The rate at which a character is able to move.", DefaultValue = 10)]
    MovementSpeed,

    [TypeInfoAttribute(DisplayName = "RateOfFire", Description = "Shots per second", DefaultValue = 1)]
    RateOfFire,

    
    [TypeInfoAttribute(DisplayName = "Snakes", Description = "?")]
    Snakes,

    [TypeInfoAttribute(DisplayName = "Size", Description = "?", DefaultValue = 1)]
    Size,

    [TypeInfoAttribute(DisplayName = "Projectile Lifespan", Description = "The maximum time in seconds a projectile will exist for.", DefaultValue = 10)]
    ProjectileLifeSpan,

    [TypeInfoAttribute(DisplayName = "ProjectileDamage", Description = "Base damage per projectile.")]
    ProjectileDamage,

    [TypeInfoAttribute(DisplayName = "ProjectileSpeed", Description = "Base projectile movement speed.", DefaultValue = 100)]
    ProjectileSpeed,

    [TypeInfoAttribute(DisplayName = "ProjectileBounce", Description = "?", DefaultValue = 0)]
    ProjectileBounce,

    [TypeInfoAttribute(DisplayName = "ProjectileMass", Description = "?", DefaultValue = 0.05f)]
    ProjectileMass,

    [TypeInfoAttribute(DisplayName = "ModSpecificModifier1", Description = "?")]
    ModSpecificModifier1,

    [TypeInfoAttribute(DisplayName = "ModSpecificModifier2", Description = "?")]
    ModSpecificModifier2,

    [TypeInfoAttribute(DisplayName = "ModSpecificModifier3", Description = "?")]
    ModSpecificModifier3,
}
//Health,
//HealthRegen,
//Mana,
//ManaRegen,
//Lives,
//Size,
//Piercing,
//PiercingResistance,
//CritChance,

public sealed class TypeInfoAttribute : Attribute // (C#/.NET Attribute - not our Attribute)
{
    public string DisplayName;
    public string Description;
    public float DefaultValue;
    public bool IsSecret = false;
}


public enum AttributeBehavior
{
    Fixed,
    Permanent,
    Gradient,
    Chained,
    Timed
}

public enum AttributeCalculationType
{
    Flat,
    Percentage
}

public static class AttributeEnumsExtended
{
    public static TypeInfoAttribute GetTypeInfoFrom(Enum enumValue, Type enumType)
    {
        return enumType.GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<TypeInfoAttribute>();
    }

    private static int _countAttributeTypes = -1;
    public static int AttributeTypesCount
    {
        get
        {
            if (_countAttributeTypes < 0)
                _countAttributeTypes = Enum.GetValues(typeof(AttributeType)).Length;
            return _countAttributeTypes;
        }
    }

    private static int _countAttributeBehavior = -1;
    public static int AttributeBehaviorCount
    {
        get
        {
            if (_countAttributeBehavior < 0)
                _countAttributeBehavior = Enum.GetValues(typeof(AttributeBehavior)).Length;
            return _countAttributeBehavior;
        }
    }
}
