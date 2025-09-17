using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyID = "default"; // her anahtar�n ID'si olabilir (�r: "gold", "silver")

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddKey(keyID);
            Destroy(gameObject); // anahtar toplan�nca yok olur
        }
    }
}
