using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunHandler : MonoBehaviour
{
    public GameObject gunHolder;
    public Gun startGunPrefab;
    internal Gun currentGun;
    internal Transform gunPoint;
    private PlayerDetection playerDetection;
    void Start()
    {
        playerDetection = GetComponent<PlayerDetection>();
        currentGun = InstantiateGun(startGunPrefab);
    }

    void Update()
    {
        if(playerDetection.IsPlayerInView())
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        currentGun.EnemyShoot(GameManager.instance.player.GetComponent<PlayerHealth>());
    }
    Gun InstantiateGun(Gun gun)
    {
        Gun instantiatedGun = Instantiate(gun, gunHolder.transform);
        instantiatedGun.transform.forward = gunHolder.transform.forward;
        instantiatedGun.Initialize();
        gunPoint = instantiatedGun.transform.Find("Gunpoint");

        return instantiatedGun;
    }
}
