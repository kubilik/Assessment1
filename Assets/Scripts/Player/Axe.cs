using UnityEngine;

public class Axe : MonoBehaviour
{
    private int damage;
    private float spinSpeed;
    private bool isStuck = false;
    private Rigidbody2D rb;
    private PlayerController owner;

    public void Initialize(int dmg, float spin, PlayerController player)
    {
        damage = dmg;
        spinSpeed = spin;
        owner = player;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isStuck)
        {
            transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isStuck && collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("Axe hit enemy for " + damage + " damage!");
            // collision.collider.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }

        if (!isStuck && collision.collider.CompareTag("Ground"))
        {
            isStuck = true;

            // Hareketi tamamen dondur
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            Debug.Log("Axe stuck in ground, can be picked up!");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isStuck && collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            owner.AddAxe();
            Destroy(gameObject);
        }
    }
}
