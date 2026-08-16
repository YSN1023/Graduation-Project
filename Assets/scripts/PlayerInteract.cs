using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(
                new Vector3(Screen.width / 2,
                            Screen.height / 2,
                            0));

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                ButtonInteract button =
                    hit.collider.GetComponent<ButtonInteract>();

                if (button != null)
                {
                    button.PressButton();
                }
            }
        }
    }
}