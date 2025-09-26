using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyID = "default";  

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddKey(keyID);
            Destroy(gameObject);  
        }
    }
}
