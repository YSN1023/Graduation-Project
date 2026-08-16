using UnityEngine;

public class PhysicsGun : MonoBehaviour
{
    [Header("Physics Gun Settings")]
    public float pickupRange = 5f;
    public float holdDistance = 3f;
    public float holdStrength = 50f;
    public float throwForce = 20f;

    private Rigidbody heldObject;
    private Transform playerCamera;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        HandlePickup();
        HandleThrow();

        if (heldObject != null)
        {
            HoldObject();
        }
    }

    void HandlePickup()
    {
        // Left click to pick up
        if (Input.GetMouseButtonDown(0))
        {
            // If already holding something, drop it
            if (heldObject != null)
            {
                DropObject();
                return;
            }

            // Shoot a ray from the camera forward
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, pickupRange))
            {
                // Check if the object has a Rigidbody
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    heldObject = rb;
                    heldObject.useGravity = false;
                    heldObject.linearDamping = 10f;
                }
            }
        }
    }

    void HoldObject()
    {
        // Calculate where the object should be
        Vector3 targetPosition = playerCamera.position + playerCamera.forward * holdDistance;

        // Move the object smoothly toward that position
        Vector3 directionToTarget = targetPosition - heldObject.position;
        heldObject.linearVelocity = directionToTarget * holdStrength * Time.deltaTime * 10f;

        // Stop the object from spinning while held
        heldObject.angularVelocity = Vector3.zero;
    }

    void HandleThrow()
    {
        // Right click to throw
        if (Input.GetMouseButtonDown(1) && heldObject != null)
        {
            heldObject.useGravity = true;
            heldObject.linearDamping = 0f;
            heldObject.AddForce(playerCamera.forward * throwForce, ForceMode.Impulse);
            heldObject = null;
        }
    }

    void DropObject()
    {
        heldObject.useGravity = true;
        heldObject.linearDamping = 0f;
        heldObject = null;
    }
}