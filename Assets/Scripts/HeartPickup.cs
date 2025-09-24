using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 20; // Kaç can dolduracak

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Çarpan objede PlayerHealth var mý kontrol et
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // Eðer caný full deðilse doldur
            if (playerHealth.GetCurrentHealth() < 100) // maxHealth deðerini scriptinden alabilirsin
            {
                playerHealth.Heal(healAmount);
                Destroy(gameObject); // Kalp objesini yok et
            }
            else
            {
                // Can zaten full, hiçbir þey yapma
                Debug.Log("Can zaten full!");
            }
        }
    }
}
