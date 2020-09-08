using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Attributes
{
    public class AttributeEntity : MonoBehaviour
    {
        public bool RequiresUpdating { get; private set; }
        public float FlatValue { get; private set; }
        public float PercentValue { get; private set; }
        public AttributeType Type { get; private set; }

        public AttributeEntity (AttributeType type)
        {
            Type = type;
        }

        // value change
        public void Update() {}
    }
}
