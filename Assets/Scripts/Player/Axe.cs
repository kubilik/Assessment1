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

    private void Update()
    {
        if (!isStuck)
        {
            transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isStuck && collision.CompareTag("Enemy"))
        {
            Debug.Log("Axe hit enemy for " + damage + " damage!");
            collision.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }

        if (!isStuck && collision.CompareTag("Ground"))
        {
            isStuck = true;
             
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            Debug.Log("Axe stuck in ground, can be picked up!");
        }
    }

}
