using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Gun pickUpGun;
    public float rotateSpeed;
    private GameObject gunModel;
    void Start()
    {
        gunModel = Instantiate(pickUpGun, transform.position, Quaternion.identity, transform).gameObject;
    }
    void Update()
    {
        RotateModel();
    }
    /// <summary>
    /// Chooses whether to equip the gun or refill existing gun
    /// </summary>
    /// <param name="gunHandler"></param>
    public void PickUpWeapon(GunHandler gunHandler)
    {
        if (gunHandler.currentGun.GetType() == pickUpGun.GetType())
        {
            gunHandler.RefillAmmo(gunHandler.currentGun);
            GameManager.instance.UpdateAmmoBar(gunHandler.currentGun.maxAmmo, gunHandler.currentGun.curAmmo);

            Destroy(gameObject);
        }
        else if (gunHandler.secondaryGun != null)
        {
            if (gunHandler.secondaryGun.GetType() == pickUpGun.GetType())
            {
                gunHandler.RefillAmmo(gunHandler.secondaryGun);
                Destroy(gameObject);
            }
            else
            {
                Gun temp = gunHandler.currentGun; //If secondary slot isn't empty and current weapon and secondary weapon isn't the same type as pickUpGun
                gunHandler.EquipGun(pickUpGun);
                PlaceWeapon(temp);
            }
        }
        else
        {
            gunHandler.EquipGun(pickUpGun); //If secondary slot is empty and current weapon isn't the same type as pickUpGun
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Places gun in the weapon pick up
    /// </summary>
    /// <param name="gunToPlace"></param>
    void PlaceWeapon(Gun gunToPlace)
    {
        pickUpGun = gunToPlace;
        Destroy(gunModel);
        gunModel = Instantiate(pickUpGun, transform.position, Quaternion.identity, transform).gameObject;
    }
    void RotateModel()
    {
        gunModel.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
