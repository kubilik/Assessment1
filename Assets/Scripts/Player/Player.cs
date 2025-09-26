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
    public bool isDead => health.GetCurrentHealth() <= 0;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        health = GetComponent<PlayerHealth>();
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
    }

    public void AddKey(string keyID)
    {
        keys.Add(keyID);
        Debug.Log("Key collected: " + keyID);
    }

     
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
