using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PewpewGun : Gun
{
    public override void Shoot(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        if (curAmmo > 0 && timeSinceLastShot > fireRate)
        {
           // hitEnemy.TakeDamage(damage);
            curAmmo--;
            UIHandler.instance.UpdateAmmoText(maxAmmo,curAmmo);
            timeSinceLastShot = 0;
        }
    }
}
