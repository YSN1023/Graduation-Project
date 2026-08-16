using UnityEngine;

public class ControlsHUD : MonoBehaviour
{
    [Header("References")]
    public GameObject mainControlsPanel;     // Shown/hidden with the toggle key
    public GameObject rotationControlsPanel; // Shown automatically while rotating a held object

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.T;

    void Start()
    {
        if (mainControlsPanel != null)
            mainControlsPanel.SetActive(true); // visible by default so new players see it

        if (rotationControlsPanel != null)
            rotationControlsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && mainControlsPanel != null)
        {
            mainControlsPanel.SetActive(!mainControlsPanel.activeSelf);
        }
    }

    // Called by PhysicsGun when entering rotation mode.
    // Also hides the main panel, so R always shows rotation controls instead,
    // even if the main panel happened to be open via T.
    public void ShowRotationHUD()
    {
        if (mainControlsPanel != null)
            mainControlsPanel.SetActive(false);

        if (rotationControlsPanel != null)
            rotationControlsPanel.SetActive(true);
    }

    // Called by PhysicsGun when exiting rotation mode - either by pressing R again,
    // or by dropping/throwing the object while mid-rotation.
    public void HideRotationHUD()
    {
        if (rotationControlsPanel != null)
            rotationControlsPanel.SetActive(false);
    }
}