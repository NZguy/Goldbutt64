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

    public class OnStartMoving : EventBase
    {
        public MonoBehaviour MovingObject { get; private set; }

        public OnStartMoving(INotifier self, MonoBehaviour movingObject) : base(self)
        {
            MovingObject = movingObject;
        }
    }
}
