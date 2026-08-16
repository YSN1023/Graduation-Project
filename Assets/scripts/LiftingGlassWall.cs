using UnityEngine;

public class LiftingGlassWall : MonoBehaviour
{
    public Vector3 liftOffset = new Vector3(0f, 3f, 0f);
    public float liftSpeed = 2f;
    public bool isLifted = false;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + liftOffset;
    }

    void Update()
    {
        if (isLifted)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, liftSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, liftSpeed * Time.deltaTime);
        }
    }

    public void Lift()
    {
        isLifted = true;
        Debug.Log("Glass wall lifting");
    }

    public void Lower()
    {
        isLifted = false;
        Debug.Log("Glass wall lowering");
    }
}
