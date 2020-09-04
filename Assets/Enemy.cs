using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int curHealth;
    private void Start()
    {
      //  curHealth = enemyInfo.Health;
    }
    public void TakeDamage(int amount)
    {
        curHealth -= amount;
        if(curHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
