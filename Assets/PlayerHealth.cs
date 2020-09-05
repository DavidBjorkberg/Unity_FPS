using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int curHealth; 
    void Start()
    {
        curHealth = maxHealth;
    }

    void Update()
    {
        
    }
    public void TakeDamage(int amount)
    {
        curHealth -= amount;
        GameManager.instance.UpdateHealthBar(maxHealth, curHealth);
        if(curHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {

    }
}
