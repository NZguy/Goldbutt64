using Assets.Scripts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Mods
{
    /// <summary>
    /// Changes the angle of the projectile by the change in rotation of the parented transform.
    /// </summary>
    public class ParentAngleMod : Mod
    {
        private Vector3 PreviousParentAngle;
        private Transform ParentTransform;

        /// <summary>
        /// ModSpecificModifier1: Scales the rotation difference of parented tranform. 
        ///                         Larger numbers reduces the effect of this mod.
        ///                         Smaller numbers, less than 1, may be broken.
        /// ModSpecificModifier2: Unused
        /// ModSpecificModifier3: Unused
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="parentProjectile"></param>
        /// <param name="parentTransform">The transform of the parent to watch</param>
        /// <param name="hardOverride">Sets the velocity angle to that of the parent if true. Otherwise, rotates velocity by the rotation of the parent.</param>
        public ParentAngleMod(List<AttributeEntity> attributes, Projectile parentProjectile, Transform parentTransform) : base(attributes, parentProjectile) {
            ParentTransform = parentTransform;
        }

        public override Mod CloneMod()
        {
            ParentAngleMod newMod = new ParentAngleMod(Attributes.GetAttributes(), ParentProjectile, ParentTransform);
            foreach(Mod mod in ChildMods)
            {
                newMod.ChildMods.Add(mod.CloneMod());
            }
            return newMod;
        }

        protected override void ResetChild()
        {
            PreviousParentAngle = ParentTransform.rotation.eulerAngles;
            rb = ParentProjectile.GetComponent<Rigidbody>();
        }

        protected override void UpdateChild()
        {
            rb.velocity = Quaternion.AngleAxis((ParentTransform.rotation.eulerAngles.y - PreviousParentAngle.y) * Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1), Vector3.up) * rb.velocity;
            PreviousParentAngle = ParentTransform.rotation.eulerAngles;
        }

    }
}
