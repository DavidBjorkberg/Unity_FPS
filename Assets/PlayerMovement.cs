using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public int movementSpeed;
    public CharacterController characterController;

    private void Update()
    {
    }
    public void Move(float horizontalMove, float verticalMove)
    {
        Vector3 movementInput = new Vector3(horizontalMove, 0, verticalMove).normalized;
        Vector3 walkDir = movementInput.x * transform.right + movementInput.z * transform.forward;
        characterController.Move(walkDir * movementSpeed * Time.deltaTime);
    }
}
