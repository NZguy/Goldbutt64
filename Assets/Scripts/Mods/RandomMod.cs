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
            switch (_random.Next(7)) {
                case 1:
                    _mod = new ReboundMod(attributes, parentProjectile);
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(2), 0));
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(2), 0));
                    break;

                case 2:
                    _mod = new SplitMod(attributes, parentProjectile.splitsInto, parentProjectile);
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(5), 0));
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(20), 0));
                    ((SplitMod)(_mod)).ChildMods.Add(new RandomMod(attributes, parentProjectile, _random));
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier3, _random.Next(5), 0));
                    break;

                case 3:
                    _mod = new TurnMod(attributes, parentProjectile);
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(360)-180, 0));
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(30), 0));
                    break;

                case 4:
                     _mod = new WaveMod(attributes, parentProjectile);
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(10), 0));
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(180), 0));
                    break;
                case 5:
                    _mod = new RandomMod(attributes, parentProjectile, _random);
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(10), 0));
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(180), 0));
                    break;
                case 6:
                    _mod = new TimerMod(attributes, parentProjectile, new RandomMod(attributes, ParentProjectile, _random));
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(5), 0));
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(5), 0));
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier3, _random.Next(5), 0));
                    break;

                default:
                    _mod = new TurnMod(attributes, parentProjectile);
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier1, _random.Next(360) - 180, 0));
                    _mod.Attributes.AddAttribute(new AttributeEntity(AttributeType.ModSpecificModifier2, _random.Next(30), 0));
                    break;
            }
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
    }
}
