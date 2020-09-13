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
                _percentValue = value / 100.0f;
            }
        }


        public AttributeType Type { get; set; }

        public AttributeEntity (AttributeType type)
        {
            Name = "Temp Item Name";
            Type = type;
        }

        // value change
        public void Update() {}
    }
}
