using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public RayGenerator ray;
    public BounceCamera bounceCamera;
    public GameObject gunHolder;
    public Gun startGunPrefab;
    internal Gun currentGun;
    internal Gun secondaryGun;
    internal Transform gunPoint;
    private Vector3 pointA; //The three corners of the ray
    private Vector3 pointB;
    private Vector3 pointC;
    private Enemy hitEnemy; //Stores the enemy the ray hits (if any), this way the ray doesn't have to be recalculated for the player to shoot.
    private float rayAim;
    private Vector3 BToLeftVector;
    void Start()
    {
        currentGun = InstantiateGun(startGunPrefab);
    }
    void Update()
    {
        AimRay();
        CalculateRay();

    }
    void CalculateRay()
    {
        pointA = gunPoint.position;
        pointC = Vector3.zero;

        if (Physics.Raycast(gunPoint.position, currentGun.transform.forward, out RaycastHit hit, 30, 1 << 10 | 1 << 11))
        {
            hit.collider.transform.root.TryGetComponent(out hitEnemy);
            pointB = hit.point;

            //If it doesn't hit an enemy, bounce
            if (hitEnemy == null)
            {
                Vector3 reflectVector = Vector3.Reflect(currentGun.transform.forward, hit.normal);
                Vector3 reflectAim = Vector3.Lerp(reflectVector, hit.normal, rayAim);
                BToLeftVector = -Vector3.Reflect(currentGun.transform.up, hit.normal);
                reflectAim.Normalize();
                BToLeftVector = Vector3.Cross(reflectAim, hit.normal);
                BToLeftVector.Normalize();
                if (Physics.Raycast(hit.point, reflectAim, out RaycastHit reflectHit, 30, 1 << 10 | 1 << 11))
                {
                    reflectHit.collider.transform.root.TryGetComponent(out hitEnemy);
                    pointC = reflectHit.point;
                }
                else
                {
                    pointC = hit.point + reflectAim * 10;
                }
            }
        }
        else
        {
            pointB = gunPoint.position + currentGun.transform.forward * 10;
        }
        ray.DrawMesh(pointA, pointB, pointC, BToLeftVector, hitEnemy != null);
        if (pointC != Vector3.zero)
        {
            bounceCamera.SetCamera(pointB, pointC);
        }
        else
        {
            bounceCamera.Deactivate();
        }
    }
    void AimRay()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            rayAim += 0.1f;
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            rayAim -= 0.1f;
        }
        rayAim = Mathf.Clamp(rayAim, 0, 0.9f);
    }
    public void Shoot()
    {
        currentGun.Shoot(pointA,pointB,pointC);
    }
    /// <summary>
    /// Swaps current and secondary weapon
    /// </summary>
    public void SwitchWeapon()
    {
        currentGun.gameObject.SetActive(false);
        secondaryGun.gameObject.SetActive(true);

        //Switch the GunInfo's and the corresponding gameobjects
        Gun tempGO = currentGun;

        currentGun = secondaryGun;
        secondaryGun = tempGO;

        gunPoint = currentGun.transform.Find("Gunpoint");
        ray.transform.parent = currentGun.transform; // Ray has to be in the guns local space
        ray.transform.localScale = Vector3.one;
        GameManager.instance.UpdateAmmoBar(currentGun.maxAmmo, currentGun.curAmmo);

    }

    public void RefillAmmo(Gun gunToRefill)
    {
        gunToRefill.curAmmo = gunToRefill.maxAmmo;
    }

    public void EquipGun(Gun gun)
    {
        if (secondaryGun == null)
        {
            secondaryGun = InstantiateGun(gun);
            SwitchWeapon();
        }
        else
        {
            //Destroy current gun and replace with new
            Destroy(currentGun);
            currentGun = InstantiateGun(gun);
        }
    }
    Gun InstantiateGun(Gun gun)
    {
        Gun instantiatedGun = Instantiate(gun, gunHolder.transform);
        instantiatedGun.transform.forward = gunHolder.transform.forward;
        instantiatedGun.Initialize();
        gunPoint = instantiatedGun.transform.Find("Gunpoint");
        ray.transform.parent = instantiatedGun.transform; // Ray has to be in the guns local space
        ray.transform.localScale = Vector3.one;

        return instantiatedGun;
    }
}
