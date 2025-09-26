using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    private PlayerController playerController;

    void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        healthSlider.maxValue = maxHealth;
    }

    private void Update()
    {
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;
        playerController.ChangeAnimHitTo(true);
        playerController.GetPlayerHitSound().Play();


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
        healthSlider.value = currentHealth;
    }

    void Die()
    {
        Debug.Log("Player died!");
        playerController.GetPlayerDeadSound().Play();
        playerController.ChangeAnimDeadTo(true);
        //Destroy(gameObject);
        // Ýstersen burada: Destroy(gameObject); veya respawn sistemi ekleyebilirsin
    }
}
