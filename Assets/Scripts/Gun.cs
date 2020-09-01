using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : ActionCooldown
{
    public float damage = 10f;
    public float range = 100f;

    public Camera shootCam;
    public ParticleSystem bulletTrail;
    public CooldownSound cooldownSound;
    public AudioSource FireSound;

    public override void Update()
    {
        base.Update();
        cooldownSound.Update();
    }

    public override void ActionOffCooldown()
    {
        FireSound.Play();
        Shoot();
    }

    public override void ActionOnCooldown()
    {
        cooldownSound.Act();
        // play a wacky sound maybe -- perhaps another action with it's own cooldown and another ActionOnCooldown method??
    }

    void Shoot ()
    {
        bulletTrail.Play();

        RaycastHit hit;
         if (Physics.Raycast(shootCam.transform.position, shootCam.transform.forward, out hit, range))
        {
            //Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
