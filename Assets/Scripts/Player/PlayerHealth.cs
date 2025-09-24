using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    private PlayerController playerController;

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent<int, int> onHealthChanged; // (currentHealth, maxHealth)


    void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthText.text = "Health: " + currentHealth.ToString();
        playerController.ChangeAnimHitTo(true);

        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
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
        playerController.ChangeAnimDeadTo(true);
        //Destroy(gameObject);
        // Ýstersen burada: Destroy(gameObject); veya respawn sistemi ekleyebilirsin
    }
}
