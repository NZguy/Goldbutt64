using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 dir;
    public int damage;
    public float speed;
    private int bounces = 0;
    public int bounceLimit;
    // true if hitting an agent counts as bounce
    public bool agentBounce;
   // private Vector3 aim;
    //public GameObject Gun;

    // Start is called before the first frame update
    void Start()
    {
        //aim = new Vector3(Gun.transform.position.x, Gun.transform.position.y, Gun.transform.position.z);
        this.GetComponent<Rigidbody>().velocity = this.transform.TransformDirection(Vector3.down*speed);
    }
    // Update is called once per frame
    void Update()
    {
        if (bounces > bounceLimit) Destroy(this.gameObject);

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
}
