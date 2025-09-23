using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    public int damage = 1; // varsayýlan hasar

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("çekirge " + damage + " vurdu!");

            // Düþmana damage scripti varsa çaðýrabilirsin:
            // collision.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }
}
