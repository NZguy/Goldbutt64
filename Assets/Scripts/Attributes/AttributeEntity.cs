using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Attributes
{
    public class AttributeEntity
    {
        public string Name { get; set; }
        public bool RequiresUpdating { get; set; }

        public float FlatValue { get; set; }

        private float _percentValue;
        public float PercentValue
        {
            get
            {
                return _percentValue;
            }

            set
            {
                if (value > 0)
                    _percentValue = value / 100.0f;
                else
                    _percentValue = value;
            }
        }


        public AttributeType Type { get; set; }

        public AttributeEntity (AttributeType type)
        {
            Name = "Temp Item Name";
            Type = type;
        }

        public AttributeEntity(AttributeType type, float flat, float percent)
        {
            string name = AttributeEnumsExtended.GetTypeInfoFrom(type, type.GetType()).DisplayName;
            Name = string.IsNullOrEmpty(name) ? "Default Name" : name;
            Type = type;
            FlatValue = flat;
            PercentValue = percent;
        }

        // value change
        public void Update() {}
    }
}
