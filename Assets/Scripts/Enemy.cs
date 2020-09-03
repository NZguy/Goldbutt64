using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    private bool canDupe;
    private float dupeCooldown;
    public float dupeCooldownMax;

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
        this.canDupe = false;
        this.dupeCooldown = dupeCooldownMax;

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

        if (!canDupe)
        {
            dupeCooldown -= Time.deltaTime;
            if(dupeCooldown <= 0)
            {
                canDupe = true;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other != null && other.collider.tag == "Player")
        {
            this.canMove = false;
            this.curMoveTimer = moveTimer;
        }
        else if (other != null && other.collider.tag == "Wall")
        {
            this.rb.velocity = this.rb.velocity * 1.8f;
            if (canDupe)
            {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                canDupe = false;
                dupeCooldown = dupeCooldownMax;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("BoundingBox") == 0) {
            this.Die();
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
