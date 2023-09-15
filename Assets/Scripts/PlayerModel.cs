using UnityEngine;

public class PlayerModel
{
    public int currentHealth;
    
    public PlayerModel(int health)
    {
        currentHealth = health;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
    }
    
}