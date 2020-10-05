using Assets.Scripts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Mods
{
    public class RandomMod : Mod
    {
        private Mod _mod;
        private Random _random;

        /// <summary>
        /// ModSpecificModifier1: Unused
        /// ModSpecificModifier2: Unused
        /// ModSpecificModifier3: Unused
        /// </summary>
        /// <remarks>
        /// ModSpecificModifiers will likely later be used to  modifier amplitude of attribute ranges per mod.
        /// </remarks>
        /// <param name="attributes"></param>
        /// <param name="parentProjectile"></param>
        public RandomMod(List<AttributeEntity> attributes, Projectile parentProjectile, Random random) : base(attributes, parentProjectile)
        {
            _random = random;
            AssignMod(attributes, parentProjectile);
        }

        private void AssignMod(List<AttributeEntity> attributes, Projectile parentProjectile)
        {
            _mod = ModFactory.PickARandomMod();
            _mod.Attributes.CalculateFinalValues();
            ChildMods.Add(_mod);
        }

        protected override void ResetChild()
        {
        }

        protected override void UpdateChild()
        {
            Cycles++;
        }

        public virtual string GetDescription(int currentIndent = 0)
        {
            return this.GetType().Name;
        }

        public override Mod CloneMod()
        {
            RandomMod newMod = new RandomMod(Attributes.GetAttributes(), ParentProjectile, _random);
            return newMod;
        }
    }
}
