using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    public PlayerController controller { get; private set; }
    public PlayerHealth health { get; private set; }
    public int coins { get; private set; }

    private HashSet<string> keys = new HashSet<string>();

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // Ýlerde inputlarý buradan yönlendirebilirsin
        // controller.Move(...);
        // controller.Jump();
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }

    public void Heal(int amount)
    {
        health.Heal(amount);
    }
    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Coins: " + coins);
        // buradan UI güncellemesi veya ses efekti çaðýrabilirsin
    }

    public void AddKey(string keyID)
    {
        keys.Add(keyID);
        Debug.Log("Key collected: " + keyID);
    }

    //idleri anahtarlaeýn üzerinde olucak if in içine yazman için var burasý 
    public bool HasKey(string keyID)
    {
        return keys.Contains(keyID);
    }

    public void RemoveKey(string keyID)
    {
        if (keys.Contains(keyID))
        {
            keys.Remove(keyID);
            Debug.Log("Key used: " + keyID);
        }
    }
}
