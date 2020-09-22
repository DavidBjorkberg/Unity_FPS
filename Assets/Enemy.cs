using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    EnemyColorController colorController;
    EnemyStateHandler stateHandler;
    void Awake()
    {
        colorController = GetComponent<EnemyColorController>();
        stateHandler = GetComponent<EnemyStateHandler>();
    }
    public void TakeDamage(int amount,Vector3 shotDirection,float shotStrength)
    {
        health -= amount;
        if(health <= 0)
        {
            stateHandler.SwitchToDeadState();
            DisableEnemy();
            SetBodyPartsVelocity(shotDirection, shotStrength);
            //Die();
        }
    }
    void SetBodyPartsVelocity(Vector3 dir, float strength)
    {
        for (int i = 0; i < colorController.modelComponents.Length; i++)
        {
            colorController.modelComponents[i].GetComponent<Rigidbody>().velocity = dir * strength * 2;
            colorController.modelComponents[i].GetComponent<Rigidbody>().useGravity = true;
            colorController.modelComponents[i].GetComponent<Collider>().enabled = true;
        }
    }
    void DisableEnemy()
    {
        GetComponent<EnemyGunHandler>().currentGun.gameObject.SetActive(false);
        transform.Find("Hitbox").gameObject.SetActive(false);

    }
    void Die()
    {
        Destroy(gameObject);
    }
}
