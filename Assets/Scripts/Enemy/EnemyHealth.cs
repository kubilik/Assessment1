using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;


    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource deathSound;

    private Animator anim;
    private EnemyMelee enemyMelee;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        enemyMelee = GetComponent<EnemyMelee>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        anim.SetBool("Hit", true);
        hitSound.Play();

        if (currentHealth <= 0)
        {
            enemyMelee.SetDeadBool();
            Die();
        }
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }


    void Die()
    {
        Debug.Log("Enemy died!");
        deathSound.Play();
        anim.SetBool("Dead", true);
        //Destroy(gameObject);
        // Ýstersen burada: Destroy(gameObject); veya respawn sistemi ekleyebilirsin
    }

}
