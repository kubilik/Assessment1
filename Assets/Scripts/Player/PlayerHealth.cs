using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent<int, int> onHealthChanged; // (currentHealth, maxHealth)

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthText.text = "Health: " + currentHealth.ToString();

        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthText.text = "Health: " + currentHealth.ToString();

        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Die()
    {
        Debug.Log("Player died!");
        onDeath?.Invoke();
        // Ýstersen burada: Destroy(gameObject); veya respawn sistemi ekleyebilirsin
    }
}
