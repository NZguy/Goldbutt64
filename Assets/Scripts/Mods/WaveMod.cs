using Assets.Scripts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Mods
{
    public class WaveMod : Mod
    {

        private float startTime = 0;
        public bool ForceSinAngleOffset = true;

        /// <summary>
        /// ModSpecificModifier1: ? Frequency ? Amplitude ? 
        /// ModSpecificModifier2: ? Frequency ? Amplitude ?
        /// ModSpecificModifier3: Unused
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="parentProjectile"></param>
        public WaveMod(List<AttributeEntity> attributes, Projectile parentProjectile) : base(attributes, parentProjectile) { }

        protected override void ResetChild()
        {
            startTime =0;
            rb = ParentProjectile.GetComponent<Rigidbody>();
        }

        protected override void UpdateChild()
        {
            double rotationAmount = 0;
            if (ForceSinAngleOffset && startTime == 0)
            {
                startTime += Time.deltaTime;
                rotationAmount = -22.5f;
            } else
            {

                startTime += Time.deltaTime;
                rotationAmount = Math.Sin(startTime * Attributes.GetAttributeValue(AttributeType.ModSpecificModifier1))
                    * (Attributes.GetAttributeValue(AttributeType.ModSpecificModifier2) * Time.deltaTime);

            }
            rb.velocity = Quaternion.AngleAxis((float)rotationAmount, Vector3.up) * rb.velocity;
            CurrentIterationCount++;
        }

        protected override Mod CloneModChild()
        {
            WaveMod newMod = new WaveMod(Attributes.GetAttributes(), ParentProjectile);
            return newMod;
        }
    }

}
