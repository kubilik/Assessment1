using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    public int damage = 1; 
    private EnemyMelee enemyMelee;
    private Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        enemyMelee = GetComponentInParent<EnemyMelee>();

        if (enemyMelee != null)
        {
            damage = Mathf.RoundToInt(enemyMelee.GetAttackDamage());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(damage);
        }
    }
}
