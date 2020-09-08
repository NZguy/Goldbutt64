using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeType
{
    MovementSpeed,
    AttackSpeed,
    Snakes,
    Lives
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
