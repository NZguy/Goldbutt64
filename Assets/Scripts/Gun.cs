using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int damage = 10;
    public float range = 100f;

    public Camera shootCam;
    public ParticleSystem bulletTrail;
    public AudioSource musicBox;

    public bool attackOffCooldown;
    public float attackRate = 0.5f;

    public GameObject Projectile;

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

        GameObject bullet = Instantiate(Projectile, this.transform.position, this.transform.rotation) as GameObject;
        //bullet.GetComponent<Projectile>().dir = this.transform.eulerAngles;



        /*
        RaycastHit hit;
        if (Physics.Raycast(shootCam.transform.position, shootCam.transform.forward, out hit, range))
        {
            //Debug.Log(hit.transform.name);

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        */

        yield return new WaitForSeconds(attackRate);
        attackOffCooldown = true;
        
    }
}
