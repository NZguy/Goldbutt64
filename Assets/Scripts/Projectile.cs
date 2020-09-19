using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // public enum Triggers { onHit, onColl, onFire, afterTime, afterBounces };
    // public Triggers triggers;
    private Cooldown cleanUpTimer;
    public GameObject splitsInto;
    // public Vector3 dir;
    public int damage = 0;
    public float speed = 100;
    private int bounces = 0;
    public int bounceLimit = 0;
    // true if hitting an agent counts as bounce
    public bool agentBounce = false;
    // public float waveFeq;
    // public float waveAmp;
    public int splits = 0;
    public bool splitOnColl = false;
    public bool splitOnFire = false;
    public SplitMod sMod;

    void Start()
    {
        cleanUpTimer = new Cooldown(15f, 15f);
        sMod = new SplitMod(splitsInto, this.gameObject);
        this.GetComponent<Rigidbody>().velocity = this.transform.forward * speed;
    }

    void Update()
    {
        // Forces the projectile to face it's move direction
        // May want to make this smoother later if we have none spherical bullets
        transform.LookAt(this.GetComponent<Rigidbody>().velocity + transform.position);


        if (cleanUpTimer.IsCool) Die();
        // if (bounces >= bounceLimit) Destroy(this.gameObject);
        if (bounces >= bounceLimit) Die();
        sMod.Update();

        //this.GetComponent<Rigidbody>().velocity = speed*(this.GetComponent<Rigidbody>().velocity.normalized);
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.GetComponent<Rigidbody>().velocity = speed * (this.GetComponent<Rigidbody>().velocity.normalized);

        Enemy enemy = collision.transform.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            if (agentBounce) bounces += 1;
        }
        else bounces += 1;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}