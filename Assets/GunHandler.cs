using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GunHandler : MonoBehaviour
{
    public RayGenerator ray;
    public BounceCamera bounceCamera;
    public GameObject gunHolder;
    public Gun startGunPrefab;
    public Image crosshair;
    internal Gun currentGun;
    internal Gun secondaryGun;
    internal Transform gunPoint;
    internal bool isRayActive;
    private Vector3 pointA; //The three corners of the ray
    private Vector3 pointB;
    private Vector3 pointC;
    private Collider pointBCollider; //Used for bazooka
    private Enemy hitEnemy; //Stores the enemy the ray hits (if any), this way the ray doesn't have to be recalculated for the player to shoot.
    private float rayAim;
    private Vector3 BToCLeftVector;
    void Start()
    {
        currentGun = InstantiateGun(startGunPrefab);
    }
    void Update()
    {
        if (isRayActive)
        {
            AimRay();
            CalculateRay();
            DrawRay();
            SetBounceCamera();
        }
        else
        {
            if (IsEnemyInCrosshair())
            {
                crosshair.color = Color.red;
            }
            else
            {
                crosshair.color = Color.white;
            }
        }

    }
    void CalculateRay()
    {
        pointA = gunPoint.position;
        pointB = Vector3.zero;
        pointC = Vector3.zero;

        RaycastHit pointBHit = CalculatePointB();

        //If pointB was found and an enemy wasnt hit, bounce
        if (pointB != Vector3.zero && hitEnemy == null)
        {
            CalculatePointC(pointBHit);
        }

    }
    bool IsEnemyInCrosshair()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit))
        {
            if (hit.transform.root.TryGetComponent(out Enemy hitEnemy))
            {
                return true;
            }
        }
        return false;
    }
    void DrawRay()
    {
        if (pointB != Vector3.zero)
        {
            ray.DrawMesh(pointA, pointB, pointC, BToCLeftVector, hitEnemy != null);
        }
        else
        {
            ray.ResetMesh();
        }
    }
    void SetBounceCamera()
    {
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
    RaycastHit CalculatePointB()
    {
        if (Physics.Raycast(gunPoint.position, currentGun.transform.forward, out RaycastHit hit, 30, 1 << 10 | 1 << 11))
        {
            hit.collider.transform.root.TryGetComponent(out hitEnemy);
            pointB = hit.point;
            pointBCollider = hit.collider;
        }
        return hit;
    }
    void CalculatePointC(RaycastHit pointBHit)
    {
        Vector3 reflectVector = Vector3.Reflect(currentGun.transform.forward, pointBHit.normal);
        Vector3 reflectAim = Vector3.Lerp(reflectVector, pointBHit.normal, rayAim).normalized;

        if (Physics.Raycast(pointBHit.point, reflectAim, out RaycastHit reflectHit, 30, 1 << 10 | 1 << 11))
        {
            reflectHit.collider.transform.root.TryGetComponent(out hitEnemy);
            pointC = reflectHit.point;
        }
        else
        {
            pointC = pointBHit.point + reflectAim * 10;
        }
        //Left vector of the B->C ray
        BToCLeftVector = Vector3.Cross(reflectAim, pointBHit.normal).normalized;
    }
    public void Shoot()
    {
        if (isRayActive)
        {
            currentGun.Shoot(pointA, pointB, pointC, pointBCollider);
        }
        else
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            currentGun.Shoot(ray);
        }
    }
    /// <summary>
    /// Swaps current and secondary weapon
    /// </summary>
    public void SwitchWeapon()
    {
        currentGun.gameObject.SetActive(false);
        secondaryGun.gameObject.SetActive(true);


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
            Destroy(currentGun);
            currentGun = InstantiateGun(gun);
        }
    }
    public void ActivateRay()
    {
        isRayActive = true;
        crosshair.gameObject.SetActive(false);
    }
    public void DeactivateRay()
    {
        crosshair.gameObject.SetActive(true);
        isRayActive = false;
        ray.ResetMesh();
        bounceCamera.Deactivate();
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
