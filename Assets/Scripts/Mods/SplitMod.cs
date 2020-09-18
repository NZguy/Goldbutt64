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
        timer = new Cooldown(2f, true);
    }

    public void Update()
    {
        timer.Update();
        if (timer.IsCool)
        {
            GameObject newProj = GameObject.Instantiate(splitsInto, parObj.transform.position, Quaternion.identity);
            newProj.transform.Rotate(new Vector3(45,0,0));
            timer.StartCooldown();
        }
    }
}

