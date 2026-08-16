using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool isUnlocked = false;
    public float minimumMass = 5f;
    public SlidingDoor slidingDoor;
    private Renderer plateRenderer;

    void Start()
    {
        plateRenderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isUnlocked)
        {
            Debug.Log("Pressure plate is locked - press the button first");
            return;
        }

        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null && rb.mass >= minimumMass)
        {
            Debug.Log("Heavy object placed - opening door");

            // Turn plate green to show it is activated
            if (plateRenderer != null)
            {
                plateRenderer.material.color = Color.green;
            }

            // Open the door
            if (slidingDoor != null)
            {
                slidingDoor.OpenDoor();
            }
        }
        else
        {
            Debug.Log("Object is too light for the pressure plate");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null && rb.mass >= minimumMass)
        {
            // Turn plate back to red when object is removed
            if (plateRenderer != null)
            {
                plateRenderer.material.color = Color.red;
            }

            // Close the door if object is removed
            if (slidingDoor != null)
            {
                slidingDoor.CloseDoor();
            }
        }
    }
}