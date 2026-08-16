using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public float minimumMass = 8f;
    public SlidingDoor slidingDoor;
    public bool isUnlocked = true;
    private Renderer plateRenderer;
    private bool isPressed = false;

    void Start()
    {
        plateRenderer = GetComponent<Renderer>();
        if (plateRenderer != null)
        {
            plateRenderer.material.color = Color.green;
        }
    }

    void Update()
    {
        if (!isUnlocked)
        {
            return;
        }

        if (slidingDoor == null)
        {
            return;
        }

        if (isPressed)
        {
            slidingDoor.OpenDoor();
        }
        else
        {
            slidingDoor.CloseDoor();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isUnlocked)
        {
            return;
        }

        if (collision.rigidbody == null || collision.rigidbody.mass < minimumMass)
        {
            return;
        }

        isPressed = true;

        if (plateRenderer != null)
        {
            plateRenderer.material.color = Color.green;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (!isUnlocked)
        {
            return;
        }

        if (collision.rigidbody == null || collision.rigidbody.mass < minimumMass)
        {
            return;
        }

        isPressed = false;

        if (plateRenderer != null)
        {
            plateRenderer.material.color = Color.green;
        }
    }
}