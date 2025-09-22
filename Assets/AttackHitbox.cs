using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 1; // varsayýlan hasar

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit for " + damage + " damage!");

            // Düþmana damage scripti varsa çaðýrabilirsin:
            // collision.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }
}
