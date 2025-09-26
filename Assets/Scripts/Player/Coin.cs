using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;  

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddCoins(coinValue);
            Destroy(gameObject);  
        }
    }
}
