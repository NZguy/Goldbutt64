using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SplitMod
{
    private Cooldown timer;
    public GameObject splitsInto;
    public GameObject parObj;

    public SplitMod (GameObject split, GameObject par)
    {
        parObj = par;
        splitsInto = split;
        timer = new Cooldown(2f, 2f);
    }

    public void Update()
    {
        if (timer.IsCool)
        {
            Quaternion newRotation = parObj.transform.rotation;
            newRotation *= Quaternion.Euler(0, 15, 0);
            GameObject newProj = GameObject.Instantiate(splitsInto, parObj.transform.position, newRotation);

            newRotation = parObj.transform.rotation;
            newRotation *= Quaternion.Euler(0, -15, 0);
            GameObject newProj2 = GameObject.Instantiate(splitsInto, parObj.transform.position, newRotation);

            timer.StartCooldown();
        }
    }
}

