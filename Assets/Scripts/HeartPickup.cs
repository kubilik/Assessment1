using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 20;
    [SerializeField] private AudioSource pickupSound;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            if (playerHealth.GetCurrentHealth() < 100)
            {
                playerHealth.Heal(healAmount);
                if (pickupSound != null)
                {
                    pickupSound.Play();
                }
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Health allready full!");
            }
        }
    }
}
