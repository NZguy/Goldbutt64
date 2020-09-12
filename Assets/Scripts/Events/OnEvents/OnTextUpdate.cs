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
    /// <summary>
    /// An event for updating text elements.
    /// VariableName refers to the key used to identify appropriate text elements.
    /// NewValue is the new text.
    /// </summary>
    public class OnTextUpdate : EventBase
    {
        public string VariableName { get; private set; }
        public string NewValue { get; private set; }

        public OnTextUpdate(INotifier self, string variableName, string newValue) : base(self)
        {
            VariableName = variableName;
            NewValue = newValue;
        }
    }
}
