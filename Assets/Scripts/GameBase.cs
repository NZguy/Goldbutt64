﻿using Assets.Scripts.Attributes;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    // Contains Attribute and Notification support
    public class GameBase : AttributeManager
    {

        public virtual void Die()
        {
            RemoveAllSubs(this);
            Destroy(gameObject);
        }
    }

}
