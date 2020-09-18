using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GunHandler gunHandler;
    public KeyCode shootButton;
    public KeyCode pickUpButton;
    public KeyCode jumpButton;
    public KeyCode switchWeaponButton;
    public LayerMask weaponPickUpLayer;
    private FPSCamera playerCamera;
    void Start()
    {
        playerCamera = GetComponentInChildren<FPSCamera>();
    }
    void Update()
    {
        playerMovement.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(Input.GetKeyDown(shootButton))
        {
            gunHandler.Shoot();
        }
        if (Input.GetKeyDown(switchWeaponButton) && gunHandler.secondaryGun != null)
        {
            gunHandler.SwitchWeapon();
        }
        if(Input.GetKeyDown(jumpButton))
        {
            playerMovement.Jump();
        }
        PickUpCheck();
    }
    void PickUpCheck()
    {
        if (Physics.Raycast(transform.position, playerCamera.transform.forward,out RaycastHit hit, 2,weaponPickUpLayer))
        {
            GameManager.instance.ShowPickUpText();
            if(Input.GetKeyDown(pickUpButton))
            {
                hit.transform.GetComponent<WeaponPickup>().PickUpWeapon(gunHandler);
            }
        }
        else
        {
            GameManager.instance.HidePickUpText();
        }
    }
    public bool IsJumpButtonPressed()
    {
        return Input.GetKey(jumpButton);
    }
}
