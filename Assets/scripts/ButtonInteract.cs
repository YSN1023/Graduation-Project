using UnityEngine;

public class ButtonInteract : MonoBehaviour
{
    public bool isActivated = false;
    public PressurePlate pressurePlate;
    public LiftingGlassWall glassWall;
    private Renderer buttonRenderer;

    void Start()
    {
        buttonRenderer = GetComponent<Renderer>();

        if (pressurePlate != null)
        {
            pressurePlate.isUnlocked = true;
        }
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

        if (pressurePlate != null)
        {
            pressurePlate.isUnlocked = true;
        }

        if (glassWall != null)
        {
            glassWall.Lift();
        }
    }
}