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
    // MISC
    [TypeInfoAttribute(DisplayName = "Snakes", Description = "?")]
    Snakes,

    // Base Character Stats
    [TypeInfoAttribute(DisplayName = "Health", Description = "The Maximum base character health.", DefaultFlatValue = 100)]
    Health,
    [TypeInfoAttribute(DisplayName = "Health Regeneration", Description = "The rate at which a character passively regains health.")]
    HealthRegen,
    [TypeInfoAttribute(DisplayName = "Mana", Description = "The Maximum base character mana.", DefaultFlatValue = 100)]
    Mana,
    [TypeInfoAttribute(DisplayName = "Mana Regeneration", Description = "The rate at which a character passively regains mana.")]
    ManaRegen,
    [TypeInfoAttribute(DisplayName = "Movement Speed", Description = "The rate at which a character is able to move.", DefaultFlatValue = 10, DefaultPercentValue = 100)]
    MovementSpeed,
    [TypeInfoAttribute(DisplayName = "Attack Speed", Description = "The rate at which a character is able to attack.", DefaultFlatValue = 1, DefaultPercentValue = 100)]
    AttackSpeed,
    [TypeInfoAttribute(DisplayName = "Lives", Description = "The Lives a character has remaining.", DefaultFlatValue = 1)]
    Lives,
    [TypeInfoAttribute(DisplayName = "Size", Description = "Character Size Scale", DefaultFlatValue = 1)]
    Size,


    // Offensive Ability / Resistances
    [TypeInfoAttribute(DisplayName = "Snakes", Description = "?")]
    Piercing,
    [TypeInfoAttribute(DisplayName = "Snakes", Description = "?")]
    PiercingResistance,

    [TypeInfoAttribute(DisplayName = "Critical Hit Chance", Description = "The likelihood of getting a critical hit.")]
    CritChance,
}

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
