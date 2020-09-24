using Assets.Scripts;
using Assets.Scripts.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttributesManagerLite
{
    public List<AttributeEntity> GetAttributes()
    {
        return attributes.ToList();
    }

    private HashSet<AttributeEntity> attributes = new HashSet<AttributeEntity>();
    private HashSet<AttributeEntity> attributesNeedingUpdates = new HashSet<AttributeEntity>();
    private Dictionary<AttributeType, float> PercentValues = new Dictionary<AttributeType, float>();
    private Dictionary<AttributeType, float> FlatValues = new Dictionary<AttributeType, float>();

    // Dictionary for shared Attribute Managers
    private Dictionary<GameBase, AttributeManager> SharedAttributes = new Dictionary<GameBase, AttributeManager>();

    // Stale if final values need updating to reflect changes in attributes.
    private bool IsStale;

    // Contains the final calculated values to be used by other objects.
    public Dictionary<AttributeType, float> FinalValues = new Dictionary<AttributeType, float>();


    #region Add Methods
    public void AddAttribute(AttributeEntity att)
    {
        if (attributes.Contains(att))
            Debug.Log($"AttributeManager already contains {att.ToString()}.");
        else
        {
            IsStale = true;
            attributes.Add(att);
            if (FinalValues.ContainsKey(att.Type))
            {
                PercentValues[att.Type] += att.PercentValue;
                FlatValues[att.Type] += att.FlatValue;
            }
            else
            {
                PercentValues.Add(att.Type, att.PercentValue);
                FlatValues.Add(att.Type, att.FlatValue);
                FinalValues.Add(att.Type, 0);
            }
        }
    }
    public void AddAttribute(List<AttributeEntity> atts)
    {
        foreach (AttributeEntity att in atts)
        {
            AddAttribute(att);
        }
    }
    #endregion

    #region Update Methods
    public void CalculateFinalValues()
    {
        foreach (AttributeType key in FlatValues.Keys)
        {
            FinalValues[key] = FlatValues[key] + (FlatValues[key] * PercentValues[key]);
        }
        IsStale = false;
    }
    #endregion

    public float GetAttributeValue(AttributeType type)
    {
        if (FinalValues.ContainsKey(type))
            return FinalValues[type];
        return AttributeEnumsExtended.GetTypeInfoFrom(type, type.GetType()).DefaultValue;
    }
}
