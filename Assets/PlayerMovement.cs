using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public int movementSpeed;
    public float jumpForce;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private float fallMultiplier = 2;
    private float lowJumpMultiplier = 1.5f;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }
    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !playerInput.IsJumpButtonPressed())
        {
            rb.velocity += Vector3.up * Physics.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }
    }
    public void Move(float horizontalMove, float verticalMove)
    {
        Vector3 movementInput = new Vector3(horizontalMove, 0, verticalMove).normalized;
        Vector3 walkDir = (movementInput.x * transform.right + movementInput.z * transform.forward).normalized;
        if (!HitWall(walkDir))
        {
            transform.position += (walkDir * movementSpeed * Time.deltaTime);
        }
    }
    public void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = Vector3.up * jumpForce;
        }
    }
    bool IsGrounded()
    {
       return Physics.SphereCast(transform.position, 0.5f, Vector3.down, out RaycastHit hit, 2, 1 << 10);
    }
    bool HitWall(Vector3 walkDir)
    {
        if (Physics.Raycast(transform.position, walkDir, out RaycastHit hit, 1))
        {
            if (hit.collider.TryGetComponent(out Obstacle obstacle))
            {
                return true;
            }
        }
        return false;
    }
}
