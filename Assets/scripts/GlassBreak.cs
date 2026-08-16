using UnityEngine;

public class GlassBreak : MonoBehaviour
{
    public float breakForce = 8f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > breakForce)
        {
            Destroy(gameObject);
        }
    }
}