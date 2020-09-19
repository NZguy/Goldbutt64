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
    [TypeInfoAttribute(DisplayName = "Movement Speed", Description = "The rate at which a character is able to move.", DefaultFlatValue = 10, DefaultPercentValue = 100)]
    MovementSpeed,

    [TypeInfoAttribute(DisplayName = "Attack Speed", Description = "The rate at which a character is able to attack.", DefaultFlatValue = 1, DefaultPercentValue = 100)]
    AttackSpeed,

    [TypeInfoAttribute(DisplayName = "Projectile Lifespan", Description = "The maximum time in seconds a projectile will exist for.", DefaultFlatValue = 10, DefaultPercentValue = 100)]
    ProjectileLifeSpan,
    
    [TypeInfoAttribute(DisplayName = "Snakes", Description = "?")]
    Snakes,

    [TypeInfoAttribute(DisplayName = "Size", Description = "?", DefaultFlatValue = 1, DefaultPercentValue = 100)]
    Size,
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
    public float DefaultFlatValue;
    public float DefaultPercentValue;
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
