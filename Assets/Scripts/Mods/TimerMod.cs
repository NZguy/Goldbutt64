using Assets.Scripts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Mods
{
    public class TimerMod : Mod
    {
        private const int STATE_START = 0;
        private const int STATE_DELAY_HOLD = 1;
        private const int STATE_INIT_RUNNING = 2;
        private const int STATE_RUNNING = 3;
        private const int STATE_RESTART = 4;
        public int state { get; private set; }
        private Cooldown _timer;

        /// <summary>
        /// ModSpecificModifier1: The timer length - How long to be updating the child mod.
        /// ModSpecificModifier2: Time before restarting the timer.
        /// ModSpecificModifier3: The delay before starting the timer
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="parentProjectile"></param>
        /// <param name="mod"></param>
        public TimerMod(List<AttributeEntity> attributes, Projectile parentProjectile, Mod mod = null) : base(attributes, parentProjectile)
        {
            if (mod != null)
                ChildMods.Add(mod);
        }

        protected override void ResetChild()
        {
            state = 0;
        }

        protected override void UpdateChild()
        {
            switch (state)
            {
                case STATE_START:
                    if (Cycles > 0)
                        // Refresh timer for Mod cooldown. 
                        _timer = new Cooldown(Attributes.GetAttributeValue(AttributeType.ModSpecificModifier2),0);
                    else
                        // Initial timer delay. ModSpecificModifier3 = how long before the child mods should begin for the first.
                        _timer = new Cooldown(Attributes.GetAttributeValue(AttributeType.ModSpecificModifier3),0);
                    _timer.StartCooldown();
                    state = STATE_DELAY_HOLD;
                    break;
                case STATE_DELAY_HOLD:
                    if (_timer.IsCool)
                    {
                        // Initial Delay / Cooldown delay is complete.
                        // Move on to updating the child mods.
                        state = STATE_INIT_RUNNING;
                        foreach (Mod mod in ChildMods)
                            mod.Reset();
                    }
                    break;
                case STATE_INIT_RUNNING:
                    state = STATE_RUNNING;
                    _timer = new Cooldown(Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1), 0);
                    _timer.StartCooldown();
                    break;
                case STATE_RUNNING:
                    // Child mods running until _timer completes
                    if(_timer.IsCool)
                    {
                        state = STATE_RESTART;
                    } else
                    {
                        foreach (Mod mod in ChildMods)
                        {   
                            mod.Update();
                        }
                    }
                    break;
                case STATE_RESTART:
                    CurrentIterationCount++;
                    state = STATE_START;
                    break;
                default:
                    state = STATE_START;
                    break;
            }
        }

        protected override Mod CloneModChild()
        {
            TimerMod newMod = new TimerMod(Attributes.GetAttributes(), ParentProjectile);
            return newMod;
        }
    }
}
