using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public int movementSpeed;
    public CharacterController characterController;

    private void Update()
    {
        Move();
    }
    void Move()
    {
        Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 walkDir = movementInput.x * transform.right + movementInput.z * transform.forward;
        characterController.Move(walkDir * movementSpeed * Time.deltaTime);
    }
}
