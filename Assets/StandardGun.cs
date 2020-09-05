using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGun : Gun
{
    public override void Shoot(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        if (curAmmo > 0 && timeSinceLastShot > fireRate)
        {
            curAmmo--;
            GameManager.instance.UpdateAmmoBar(maxAmmo, curAmmo);
            timeSinceLastShot = 0;
            Enemy hitEnemy = HitEnemyCheck(pointA, pointB, pointC);
            if(hitEnemy != null)
            {
                hitEnemy.TakeDamage(damage);
            }
        }
    }
    Enemy HitEnemyCheck(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Enemy returnEnemy = null;
        if (Physics.Raycast(pointA, (pointB - pointA), out RaycastHit hit, 1 << 10 | 1 << 11))
        {
            hit.transform.root.TryGetComponent(out returnEnemy);
        }
        else if (Physics.Raycast(pointB, (pointC - pointB), out hit, 1 << 10 | 1 << 11))
        {
            hit.transform.root.TryGetComponent(out returnEnemy);
        }
        return returnEnemy;
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
