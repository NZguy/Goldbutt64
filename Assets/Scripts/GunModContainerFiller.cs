using Assets.Scripts.Attributes;
using Assets.Scripts.Mods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Scripts {

    public class GunModContainerFiller : MonoBehaviour
    {
        [SerializeField]
        private GunModContainer[] GunModContainers;
        private Random random = new Random();
        // Start is called before the first frame update
        void Start()
        {
            GunModContainers = (GunModContainer[])FindObjectsOfType(typeof(GunModContainer));
            GenerateMods();
        }

        void GenerateMods ()
        {

            for(int i = 0; i < GunModContainers.Length; i++)
            {
                for (int numberOfMods = 0; numberOfMods < random.Next(5) + 2; numberOfMods++)
                {
                    GunModContainers[i].AttachedMods.Add(ModFactory.PickARandomMod());
                }
            }
        }

    }
}
