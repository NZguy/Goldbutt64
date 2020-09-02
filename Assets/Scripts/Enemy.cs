using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;
    public float speed = 2f;
    public GameObject player;
    private Rigidbody rb;

    public float moveTimer;
    private bool canMove;
    private float curMoveTimer;

    private void Start()
    {
        this.canMove = true;
        this.curMoveTimer = 0;
        this.rb = GetComponent<Rigidbody>();

        if(player == null)
        {
            this.player = GameObject.Find("Player");
        }
    }

    private void Update()
    {
        if (canMove)
        {
            Vector3 targetPos = Vector3.MoveTowards(transform.position, player.transform.position, 2 * Time.deltaTime);
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        }
        else
        {
            curMoveTimer -= Time.deltaTime;
            if(curMoveTimer <= 0)
            {
                canMove = true;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Player")
        {
            this.canMove = false;
            this.curMoveTimer = moveTimer;
        }
        else if (other.collider.tag == "Wall")
        {
            this.rb.velocity = this.rb.velocity * 1.8f;
        }
    }

    public void TakeDamage (int amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die ()
    {
        Destroy(gameObject);
    }
}
