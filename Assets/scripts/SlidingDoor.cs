using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0f, 3f, 0f);
    public float slideSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;
    private bool isClosing = false;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = transform.position + openOffset;
    }

    void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                openPosition,
                slideSpeed * Time.deltaTime
            );

            if (transform.position == openPosition)
            {
                isOpening = false;
            }
        }

        if (isClosing)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                closedPosition,
                slideSpeed * Time.deltaTime
            );

            if (transform.position == closedPosition)
            {
                isClosing = false;
            }
        }
    }

    public void OpenDoor()
    {
        isOpening = true;
        isClosing = false;
        Debug.Log("Door opening");
    }

    public void CloseDoor()
    {
        isClosing = true;
        isOpening = false;
        Debug.Log("Door closing");
    }
}