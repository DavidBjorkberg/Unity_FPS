using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public int maxAmmo;
    internal int curAmmo;
    protected float timeSinceLastShot;

    public abstract void Shoot(Vector3 pointA, Vector3 pointB, Vector3 pointC);

    public void Initialize()
    {
        timeSinceLastShot = fireRate;
        curAmmo = maxAmmo;
    }
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }
}
