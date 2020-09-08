using Assets.Scripts.Events.Base;
using Assets.Scripts.Events.OnEvents;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Attributes
{

    public class AttributeManager : NotifierBase, ISubscriber
    {
        private INotifier Parent;
        private HashSet<AttributeEntity> attributes;
        private HashSet<AttributeEntity> attributesNeedingUpdates;
        private Dictionary<AttributeType, float> PercentValues;
        private Dictionary<AttributeType, float> FlatValues;
        private bool IsStale;

        public Dictionary<AttributeType, float> FinalValues { get; private set; }

        public AttributeManager (INotifier parent)
        {
            Parent = parent;
            attributes = new HashSet<AttributeEntity>();
            attributesNeedingUpdates = new HashSet<AttributeEntity>();
            PercentValues = new Dictionary<AttributeType, float>();
            FlatValues = new Dictionary<AttributeType, float>();
            FinalValues = new Dictionary<AttributeType, float>();
        }

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
                }
                else
                {
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
            foreach (AttributeType key in FinalValues.Keys)
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
        private const int AttributeColumnWidth  = -7;
        private const int FlatColumnWidth       = -4;
        private const int PercentColumnWidth    = -4;
        private const int FinalColumnWidth      = -5;

        public string ToStringTable()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{"Attribute", AttributeColumnWidth} | {"Flat",FlatColumnWidth} | {"Percent",PercentColumnWidth} | {"Final",FinalColumnWidth} ");
            foreach (AttributeEntity att in attributesNeedingUpdates)
            {
                sb.Append($"\n{att.Type.ToString(),AttributeColumnWidth} | {FlatValues[att.Type],FlatColumnWidth} | {PercentValues[att.Type],PercentColumnWidth} | {FinalValues[att.Type],FinalColumnWidth} ");
            }
            return sb.ToString();
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