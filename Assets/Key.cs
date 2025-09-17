using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyID = "default"; // her anahtarýn ID'si olabilir (ör: "gold", "silver")

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddKey(keyID);
            Destroy(gameObject); // anahtar toplanýnca yok olur
        }
    }
}
