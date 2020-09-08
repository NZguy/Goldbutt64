using Assets.Scripts.Events.Base;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Events.OnEvents
{
    class OnStopMoving : EventBase
    {
        public MonoBehaviour MovingObject { get; private set; }

        public OnStopMoving(INotifier self, MonoBehaviour movingObject) : base(self)
        {
            MovingObject = movingObject;
        }
    }
}
