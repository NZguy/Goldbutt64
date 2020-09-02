using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera shootCam;
    public ParticleSystem bulletTrail;
    public AudioSource musicBox;

    public bool attackOffCooldown;
    public float attackRate = 0.5f;

    private void Start()
    {
        attackOffCooldown = true;
    }

    public void Attack(bool isClick)
    {
        if (attackOffCooldown)
            StartCoroutine(AttackLoop());
        else if (isClick)
            musicBox.GetComponent<MusicBox>().playCoolSound();
    }

    public IEnumerator AttackLoop()
    {
        attackOffCooldown = false;
        musicBox.GetComponent<MusicBox>().playShootSound();
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

        yield return new WaitForSeconds(attackRate);
        attackOffCooldown = true;
    }
}
