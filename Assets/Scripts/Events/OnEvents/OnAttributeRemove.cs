using Assets.Scripts.Attributes;
using Assets.Scripts.Events.Base;
using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events.OnEvents
{
    public class OnAttributeRemove : EventBase
    {
        public AttributeEntity Attribute { get; set; }
        public OnAttributeRemove(INotifier self, AttributeEntity attribute) : base(self)
        {
            Attribute = attribute;
        }
    }
}
