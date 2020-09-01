using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera shootCam;
    public ParticleSystem bulletTrail;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Mouse0))
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot ()
    {
        bulletTrail.Play();

        RaycastHit hit;
         if (Physics.Raycast(shootCam.transform.position, shootCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
