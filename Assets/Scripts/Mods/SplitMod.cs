using Assets.Scripts;
using Assets.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mods
{
    public class SplitMod : Mod
    {

        private GameBase ParentGun;

        /// <summary>
        /// ModSpecificModifier1: Number of Times to split projectile
        /// ModSpecificModifier2: Angle between splits
        /// ModSpecificModifier3: Unused
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="split"></param>
        /// <param name="parentProjectile"></param>
        public SplitMod(List<AttributeEntity> attributes, GameObject split, Projectile parentProjectile) : base(attributes, parentProjectile)
        {
            SplitsInto = split;
        }

        protected override void UpdateChild()
        {
            for (int i = 1; i <= Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1); i++)
            {
                Quaternion newRotation = ParentProjectile.transform.rotation;
                newRotation *= Quaternion.Euler(0, Attributes.GetAttributeValue(AttributeType.ModSpecificModifier2) * i, 0);
                GameObject newProjectile = Object.Instantiate(SplitsInto, ParentProjectile.transform.position, newRotation);
                newProjectile.GetComponent<Projectile>().Init(Attributes.GetAttributes(), ChildMods);
                newProjectile.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Attributes.GetAttributeValue(AttributeType.ModSpecificModifier2) * i, Vector3.left) * newProjectile.GetComponent<Rigidbody>().velocity;
            }
            Cycles++;
        }

        protected override void ResetChild() { }

        public override Mod CloneMod()
        {
            SplitMod newMod = new SplitMod(Attributes.GetAttributes(), SplitsInto, ParentProjectile);
            foreach (Mod mod in ChildMods)
            {
                newMod.ChildMods.Add(mod.CloneMod());
            }
            return newMod;
        }
    }
}
