using UnityEngine;

public class PhysicsGun : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public PlayerMovement playerMovement;
    public ControlsHUD controlsHUD;

    [Header("Pickup Settings")]
    public float pickupRange = 5f;
    public LayerMask grabbableMask;

    [Header("Hold Settings")]
    public float holdDistance = 3f;
    public float minHoldDistance = 1.5f;
    public float maxHoldDistance = 6f;
    public float scrollSensitivity = 2f;
    public float followStrength = 200f; // Spring acceleration (before mass scaling)
    public float followDamping = 25f;   // Prevents overshooting/wobble
    public float minEffectiveMass = 0.3f; // Floor so very light props don't fling wildly
    public float maxEffectiveMass = 8f;   // Ceiling so very heavy props aren't unmovable

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f;

    [Header("Throw Settings")]
    public float throwForce = 15f;     // Mass-dependent impulse force

    private Rigidbody heldBody;
    private bool isRotating = false;

    void Update()
    {
        if (heldBody == null)
        {
            if (Input.GetMouseButtonDown(0))
                TryPickup();
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isRotating = !isRotating;
            playerMovement.SetMovementLocked(isRotating);

            if (isRotating)
            {
                heldBody.angularVelocity = Vector3.zero;
                if (controlsHUD != null) controlsHUD.ShowRotationHUD();
            }
            else
            {
                if (controlsHUD != null) controlsHUD.HideRotationHUD();
            }
        }

        UpdateHoldDistance();

        if (Input.GetMouseButtonDown(0))
            DropObject();

        if (Input.GetMouseButtonDown(1))
            ThrowObject();
    }

    void FixedUpdate()
    {
        if (heldBody == null) return;

        if (isRotating)
            RotateHeldObject();
        else
            HoldObject();
    }

    void TryPickup()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, grabbableMask))
        {
            heldBody = hit.rigidbody;
            if (heldBody != null)
            {
                heldBody.useGravity = false;
                heldBody.angularVelocity = Vector3.zero;
                heldBody.constraints = RigidbodyConstraints.None;
                isRotating = false;
            }
        }
    }

    void HoldObject()
    {
        Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward * holdDistance;
        Vector3 toTarget = targetPosition - heldBody.position;

        // ForceMode.Acceleration ignores the rigidbody's mass entirely, so every object -
        // light or heavy - was tracking the crosshair at exactly the same rate. Dividing by
        // a clamped "effective mass" here restores the weight difference (light props snap
        // in fast, heavy ones lag behind) without letting a very light prop go unstable or
        // a very heavy one become unmovable.
        float effectiveMass = Mathf.Clamp(heldBody.mass, minEffectiveMass, maxEffectiveMass);

        Vector3 springAccel = (toTarget * followStrength) / effectiveMass;
        Vector3 dampingAccel = (-heldBody.linearVelocity * followDamping) / effectiveMass;

        heldBody.AddForce(springAccel + dampingAccel, ForceMode.Acceleration);
        heldBody.angularVelocity = Vector3.zero;
    }

    void RotateHeldObject()
    {
        float xRot = 0f, yRot = 0f, zRot = 0f;

        if (Input.GetKey(KeyCode.A)) xRot -= rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) xRot += rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q)) yRot += rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) yRot -= rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W)) zRot -= rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) zRot += rotationSpeed * Time.deltaTime;

        heldBody.linearVelocity = Vector3.zero;
        heldBody.angularVelocity = Vector3.zero;

        Quaternion worldDelta = Quaternion.AngleAxis(xRot, Vector3.right)
                               * Quaternion.AngleAxis(yRot, Vector3.up)
                               * Quaternion.AngleAxis(zRot, Vector3.forward);

        heldBody.MoveRotation(worldDelta * heldBody.rotation);
    }

    void UpdateHoldDistance()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        holdDistance = Mathf.Clamp(
            holdDistance + scroll * scrollSensitivity,
            minHoldDistance,
            maxHoldDistance
        );
    }

    void DropObject()
    {
        ReleaseHeldObject();
    }

    void ThrowObject()
    {
        if (heldBody == null) return;
        Rigidbody thrown = heldBody;
        ReleaseHeldObject();

        // ForceMode.Impulse applies momentum: light objects fly far, heavy objects drop closer
        thrown.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);
    }

    void ReleaseHeldObject()
    {
        if (heldBody != null)
        {
            heldBody.useGravity = true;
            // No rotation freeze here - locking rotation permanently meant anything
            // dropped mid-tilt got stuck balanced in that exact pose forever, since it
            // could fall but never rotate to a stable resting angle. Angular Damping on
            // the Rigidbody (bump it up a bit) resists unwanted toppling once something
            // settles, without permanently locking rotation.
            heldBody.constraints = RigidbodyConstraints.None;
        }

        if (isRotating)
        {
            isRotating = false;
            playerMovement.SetMovementLocked(false);
            if (controlsHUD != null) controlsHUD.HideRotationHUD();
        }

        heldBody = null;
    }
}