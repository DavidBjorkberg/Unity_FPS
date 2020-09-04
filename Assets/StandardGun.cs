using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGun : Gun
{
    public ParticleSystem hitEffect;
    public override void Shoot(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        if (curAmmo > 0 && timeSinceLastShot > fireRate)
        {
            //hitEnemy.TakeDamage(damage);
            curAmmo--;
            UIHandler.instance.UpdateAmmoText(maxAmmo, curAmmo);
            timeSinceLastShot = 0;
            if (Physics.Raycast(pointB, (pointC - pointB), out RaycastHit hit))
            {
               GameObject instantiatedPS = Instantiate(hitEffect, pointC, Quaternion.identity).gameObject;
                instantiatedPS.transform.up = hit.normal;

            }
        }
    }
}
