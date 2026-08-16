using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 1.1f;
    public bool canMove = true;
    [Header("Mouse Settings")]
    public float mouseSensitivity = 2f;

    private Rigidbody rb;
    private Camera playerCamera;
    private float verticalRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetMovementLocked(bool locked)
    {
        canMove = !locked;
    }

    void Update()
    {
        if (canMove)
        {
            HandleMouseLook();
            HandleJump();
            CheckGrounded();
        }
    }

    void FixedUpdate()
    {
        if (canMove)
            HandleMovement();
        else
            StopHorizontalMovement();
    }

    void HandleMouseLook()
    {
        // Horizontal rotation - rotates the player body
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseX, 0);

        // Vertical rotation - rotates only the camera
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection = moveDirection.normalized * moveSpeed;

        // Keep existing Y velocity so gravity works normally
        moveDirection.y = rb.linearVelocity.y;
        rb.linearVelocity = moveDirection;
    }

    void StopHorizontalMovement()
    {
        // Called while movement is locked (e.g. rotation mode), so residual
        // momentum from the moment R was pressed doesn't keep carrying the
        // player sideways — Y velocity is preserved so gravity still applies.
        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(0f, v.y, 0f);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void CheckGrounded()
    {
        // Shoots a ray downward to check if player is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }
}
