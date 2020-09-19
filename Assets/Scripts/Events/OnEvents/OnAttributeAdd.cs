using Assets.Scripts.Attributes;
using Assets.Scripts.Events.Base;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events.OnEvents
{
    class OnAttributeAdd : EventBase
    {
        public AttributeEntity Attribute { get; set; }

        public OnAttributeAdd(INotifier self, AttributeEntity attribute) : base(self)
        {
            Attribute = attribute;
        }
    }
}
