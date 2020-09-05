using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGun : Gun
{
    public override void Shoot(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        if (curAmmo > 0 && timeSinceLastShot > fireRate)
        {
            //hitEnemy.TakeDamage(damage);
            curAmmo--;
            GameManager.instance.UpdateAmmoBar(maxAmmo, curAmmo);
            timeSinceLastShot = 0;
            if (Physics.Raycast(pointB, (pointC - pointB), out RaycastHit hit, 1 << 10 | 1 << 11))
            {
                if (hit.transform.root.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
    public override void EnemyShoot(PlayerHealth player)
    {
        if (curAmmo > 0 && timeSinceLastShot > fireRate)
        {
            muzzleFlash.Play();
            curAmmo--;
            player.TakeDamage(damage);
            timeSinceLastShot = 0;
        }
    }
}
