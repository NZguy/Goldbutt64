using Assets.Scripts.Attributes;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events.Base
{
    public class EventBase : IGameEvent, IComparable
    {
        public INotifier EventCreater { get; private set; }
        public bool IsStale { get; set; }
        public Action OnTrigger;
        private INotifier self;

        public EventBase(INotifier self)
        {
            EventCreater = self;
        }
        

        public virtual int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
