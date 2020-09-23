using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PewpewGun : Gun
{
    public override void Shoot(Vector3 pointA, Vector3 pointB, Vector3 pointC,Collider pointBCollider)
    {
        if (curAmmo > 0 && timeSinceLastShot > fireRate)
        {
           // hitEnemy.TakeDamage(damage);
            curAmmo--;
            GameManager.instance.UpdateAmmoBar(maxAmmo,curAmmo);
            timeSinceLastShot = 0;
        }
    }
    public override void Shoot(Ray ray)
    {
        if (curAmmo > 0 && timeSinceLastShot > fireRate)
        {
            curAmmo--;
            GameManager.instance.UpdateAmmoBar(maxAmmo, curAmmo);
            timeSinceLastShot = 0;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out Enemy hitEnemy))
                {
                    hitEnemy.TakeDamage(damage, ray.direction, 1);
                }
            }
        }
    }
    public override void EnemyShoot(PlayerHealth player)
    {
        if (curAmmo > 0 && timeSinceLastShot > fireRate)
        {
            curAmmo--;
            player.TakeDamage(damage);
            timeSinceLastShot = 0;
        }
    }

}
