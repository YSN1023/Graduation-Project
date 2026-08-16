using UnityEngine;

public class ButtonInteract : MonoBehaviour
{
    public bool isActivated = false;
    public PressurePlate pressurePlate;
    private Renderer buttonRenderer;

    void Start()
    {
        buttonRenderer = GetComponent<Renderer>();
    }

    public void PressButton()
    {
        isActivated = true;
        Debug.Log("Button Pressed - Pressure plate unlocked");

        // Turn button green to show it has been pressed
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = Color.green;
        }

        // Unlock the pressure plate
        if (pressurePlate != null)
        {
            pressurePlate.isUnlocked = true;
        }
    }
}