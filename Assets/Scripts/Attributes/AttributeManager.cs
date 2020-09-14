using Assets.Scripts.Events.Base;
using Assets.Scripts.Events.OnEvents;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Attributes
{

    public class AttributeManager : NotifierBase, ISubscriber
    {

        public List<AttributeEntity> GetAttributes()
        {
            return attributes.ToList();
        }


        private NotifierBase Parent;
        private HashSet<AttributeEntity> attributes = new HashSet<AttributeEntity>();
        private HashSet<AttributeEntity> attributesNeedingUpdates = new HashSet<AttributeEntity>();
        private Dictionary<AttributeType, float> PercentValues = new Dictionary<AttributeType, float>();
        private Dictionary<AttributeType, float> FlatValues = new Dictionary<AttributeType, float>();
        private bool IsStale;

        // Contains a copy of all attributes grouped by AttributeType. Created for debug use (and maybe for displaying stats later).
        private Dictionary<AttributeType, List<AttributeEntity>> GroupedAttributes = new Dictionary<AttributeType, List<AttributeEntity>>();

        // Contains the final calculated values to be used by other objects.
        public Dictionary<AttributeType, float> FinalValues = new Dictionary<AttributeType, float>();

        public void Update()
        {
            foreach (AttributeEntity att in attributesNeedingUpdates)
            {
                PercentValues[att.Type] -= att.PercentValue;
                FlatValues[att.Type] -= att.FlatValue;
                att.Update();
                PercentValues[att.Type] += att.PercentValue;
                FlatValues[att.Type] += att.FlatValue;
            }

            if (IsStale || attributesNeedingUpdates.Count > 0)
                CalculateFinalValues();
            Notify(new OnAttributeAdd(this, null));
        }

        #region Add Methods
        public void Add(AttributeEntity att)
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
                    GroupedAttributes[att.Type].Add(att);

                }
                else
                {
                    GroupedAttributes.Add(att.Type, new List<AttributeEntity>());
                    GroupedAttributes[att.Type].Add(att);

                    PercentValues.Add(att.Type, att.PercentValue);
                    FlatValues.Add(att.Type, att.FlatValue);
                    FinalValues.Add(att.Type, 0);
                }
            }
        }
        public void Add(List<AttributeEntity> atts)
        {
            foreach (AttributeEntity att in atts)
            {
                Add(att);
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

        #region Remove Methods
        public void Remove(AttributeEntity att)
        {
            if (attributes.Contains(att))
            {
                IsStale = true;
                attributes.Remove(att);
                PercentValues[att.Type] -= att.PercentValue;
                FlatValues[att.Type] -= att.FlatValue;
                GroupedAttributes[att.Type].Remove(att);

            }
            else
            {
                Debug.Log($"AttributeManager does not contain {att.ToString()}.");
            }
        }

        public void Remove(List<AttributeEntity> atts)
        {
            foreach (AttributeEntity att in atts)
            {
                Remove(att);
            }
        }
        #endregion

        public float GetValue(AttributeType type)
        {
            if (FinalValues.ContainsKey(type))
                return FinalValues[type];
            return 0;
        }

        #region Debug/Print methods
        private const int AttributeColumnWidth  = 20;
        private const int FlatColumnWidth       = 6;
        private const int PercentColumnWidth    = 10;
        private const int FinalColumnWidth      = 10;


        public string ToStringTableFinalValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{"Attribute", AttributeColumnWidth} | {"Final",FlatColumnWidth}");
            foreach (AttributeType key in FinalValues.Keys)
            {
                sb.Append($"\n{key.ToString(), AttributeColumnWidth} | {FinalValues[key], FinalColumnWidth}");
            }
            return sb.ToString();
        }

        public string ToStringTableAttributes()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{"Attribute",AttributeColumnWidth} | {"Flat",FlatColumnWidth} | {"Percent",PercentColumnWidth}");
            foreach (AttributeEntity att in attributes)
            {
                sb.Append($"\n{att.Type.ToString(),AttributeColumnWidth} | {att.FlatValue,FlatColumnWidth} | {FormatPercent(att.PercentValue),PercentColumnWidth}");
            }
            return sb.ToString();
        }

        public string ToStringTableCompleteRich()
        {
            StringBuilder sb = new StringBuilder();
            foreach (AttributeType key in FinalValues.Keys)
            {
                if (GroupedAttributes[key].Count > 0)
                {
                    sb.Append($"\n<u>{AttributeEnumsExtended.GetTypeInfoFrom(key, key.GetType()).DisplayName, -AttributeColumnWidth}   {"Flat",FlatColumnWidth}   {"Percent",PercentColumnWidth}   {"Final", FinalColumnWidth}</u>");
                    AddAttributesOfType(key, sb);
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }

        private void AddAttributesOfType(AttributeType type, StringBuilder sb)
        {
            foreach (AttributeEntity att in GroupedAttributes[type])
            {
                sb.Append($"\n{att.Name, -AttributeColumnWidth} | {att.FlatValue,FlatColumnWidth} | {FormatPercent(att.PercentValue),PercentColumnWidth} | {FinalValues[att.Type],FinalColumnWidth} ");
            }
        }

        private string FormatPercent(float percent)
        {
            if (percent >= 0)
                return "+" + (percent * 100.0f) + "%";
            else
                return (percent * 100.0f) + "%";
        }


        public bool OnNotify(IGameEvent gameEvent)
        {
            if (gameEvent is OnAttributeRemove)
            {
                OnAttributeRemove evnt = (OnAttributeRemove)gameEvent;
                Remove(evnt.Attribute);
            }
            else
            if (gameEvent is OnAttributeAdd)
            {
                OnAttributeAdd evnt = (OnAttributeAdd)gameEvent;
                Remove(evnt.Attribute);
            }
            return false;
        }
        #endregion
    }
}