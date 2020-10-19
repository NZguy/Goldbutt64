using Assets.Scripts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Mods
{
    public class SequenceMod : Mod
    {
        /// <summary>
        /// ModSpecificModifier1: Unused 
        /// ModSpecificModifier2: Unused
        /// ModSpecificModifier3: Unused
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="parentProjectile"></param>
        public SequenceMod(List<AttributeEntity> attributes, Projectile parentProjectile) : base(attributes, parentProjectile)
        {

        }

        protected override void ResetChild()
        {
            state = 0;
            CurrentModIndex = 0;
        }

        public override void Update()
        {
            if (MaxCycles > -1 && Cycles > MaxCycles)
                IsEnabled = false;

            if (IsEnabled)
                UpdateChild();
        }

        private const int STATE_START = 0;
        private const int STATE_START_TO_UPDATE = 1;
        private const int STATE_UPDATE = 2;
        private const int STATE_UPDATE_TO_FINISH = 3;
        private const int STATE_FINISH = 4;
        private int state = 0;
        private int CurrentModIndex = 0;
        private int CurrentChildModStartCycle = 0;
        protected override void UpdateChild()
        {
            //if (ChildMods[CurrentModIndex] is TimerMod timermod)
            //    Debug.Log($"Current Cycle: {Cycles}/{MaxCycles}, IsEnabled={IsEnabled}, IsModEnabled={ChildMods[CurrentModIndex].IsEnabled}, CurrentMod: {ChildMods[CurrentModIndex].GetType().Name}, CurrentIndex: {CurrentModIndex}, ModCycles: {ChildMods[CurrentModIndex].Cycles}/{ChildMods[CurrentModIndex].MaxCycles}, CurrentState: {state}, TimerMod State: {timermod.state}");
            //else
            //    Debug.Log($"Current Cycle: {Cycles}/{MaxCycles}, IsEnabled={IsEnabled}, IsModEnabled={ChildMods[CurrentModIndex].IsEnabled}, CurrentMod: {ChildMods[CurrentModIndex].GetType().Name}, CurrentIndex: {CurrentModIndex}, ModCycles: {ChildMods[CurrentModIndex].Cycles}/{ChildMods[CurrentModIndex].MaxCycles}, CurrentState: {state}");
            switch (state)
            {
                case STATE_START:
                    state++;

                    break;
                case STATE_START_TO_UPDATE:
                    state++;
                    CurrentChildModStartCycle = ChildMods[CurrentModIndex].Cycles;
                    break;
                case STATE_UPDATE:
                    ChildMods[CurrentModIndex].Update();
                    if (!ChildMods[CurrentModIndex].IsEnabled)
                        state = STATE_FINISH;
                    // Check if current child mod has had enough update tick to complete a cycle.
                    // If so, move on to the next state.
                    if (ChildMods[CurrentModIndex].Cycles > CurrentChildModStartCycle)
                        state++;
                    break;
                case STATE_UPDATE_TO_FINISH:
                    state++;

                    break;
                case STATE_FINISH:
                    if (!ChildMods[CurrentModIndex].IsEnabled)
                        ChildMods.Remove(ChildMods[CurrentModIndex]);
                    else
                        CurrentModIndex++;
                    
                    // All child mods have completed 1 cycle.
                    if (CurrentModIndex >= ChildMods.Count)
                    {
                        if (ChildMods.Count <= 0)
                        {
                            this.IsEnabled = false;
                            return;
                        }


                        CurrentModIndex = 0;
                        // If Cycles exceeds MaxCycles, this mod will be automatically disabled (see Update() in Mod.cs) 
                        CurrentIterationCount++;
                        foreach (Mod mod in ChildMods)
                            mod.Reset();
                    }
                    // There are still child mods waiting to perform their cycle.
                    // Restart states and update next child mod.
                        state = STATE_START;
                    break;
                default:
                    Debug.Log("Bad state in sequence mod");
                    state = STATE_START;
                    break;
            }

        }

        protected override Mod CloneModChild()
        {
            SequenceMod newMod = new SequenceMod(Attributes.GetAttributes(), ParentProjectile);
            return newMod;
        }

        public override void CollisionTriggers(Collision collision)
        {
        }
    }
}
