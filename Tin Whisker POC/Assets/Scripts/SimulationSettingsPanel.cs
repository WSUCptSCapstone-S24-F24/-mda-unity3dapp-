using UnityEngine;
using UnityEngine.UI;

public class SimulationSettingsPanel : MonoBehaviour
{
    public GameObject simulationSettingsPanel; // Reference to the Simulation Settings Panel
    public Button simulationSettingsButton;    // Reference to the button that opens the panel
    public Button closeSimulationSettingsButton; // Reference to the cloned button inside the panel
    public GameObject goToPartPanel;           // Reference to the Go To Part Panel
    public GameObject monteCarloSelectionPanel; // Reference to the Monte Carlo Selection buttons
    public GameObject basicControlsPanel;      // Reference to other basic control buttons (except Exit)
    public Button exitButton;                  // Reference to the Exit button, which should remain active

    private bool isPanelOpen = false; // Tracks whether the settings panel is open

    void Start()
    {
        // Ensure the settings panel is disabled by default
        simulationSettingsPanel.SetActive(false);

        // Add listeners to both buttons (outside and inside the panel) to toggle the settings panel
        simulationSettingsButton.onClick.AddListener(ToggleSimulationSettings);
        closeSimulationSettingsButton.onClick.AddListener(ToggleSimulationSettings); // New close button inside the panel

        // Ensure other UI elements are enabled by default
        EnableAllUIElements();
    }

    void Update()
    {
        // Check for the Escape key to close the settings panel
        if (isPanelOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSimulationSettings();
        }
    }

    // Toggles the simulation settings panel
    public void ToggleSimulationSettings()
    {
        isPanelOpen = !isPanelOpen;
        simulationSettingsPanel.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            // Disable all other UI except for the Exit button
            DisableOtherUI();
        }
        else
        {
            // Re-enable all other UI
            EnableAllUIElements();
        }
    }

    private void DisableOtherUI()
    {
        // Disable the Go To Part Panel and Monte Carlo Selection buttons
        goToPartPanel.SetActive(false);
        monteCarloSelectionPanel.SetActive(false);
        basicControlsPanel.SetActive(false); // Disable other controls, except the exit button
        exitButton.gameObject.SetActive(true); // Ensure the exit button remains active
    }

    private void EnableAllUIElements()
    {
        // Enable the Go To Part Panel and Monte Carlo Selection buttons when the panel is closed
        goToPartPanel.SetActive(true);
        monteCarloSelectionPanel.SetActive(true);
        basicControlsPanel.SetActive(true); // Re-enable other controls
    }
}
