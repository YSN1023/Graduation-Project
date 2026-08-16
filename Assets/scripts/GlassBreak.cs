using UnityEngine;

public class GlassBreak : MonoBehaviour
{
    public float breakImpulse = 8f; // impulse threshold, not velocity — retune from scratch by testing

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > breakImpulse)
        {
            Destroy(gameObject);
        }
    }
}