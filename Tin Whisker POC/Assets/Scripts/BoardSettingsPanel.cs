using UnityEngine;
using UnityEngine.UI;

public class BoardSettingsPanel : MonoBehaviour
{
    public GameObject boardSettingsPanel; 
    public Button openBoardSettingsButton;   
    public Button closeBoardSettingsButton; 
    public GameObject goToPartPanel;         
    public GameObject monteCarloSelectionPanel; 
    public GameObject basicControlsPanel;   
    public Button exitButton;     

    private bool isPanelOpen = false; 

    void Start()
    {
        boardSettingsPanel.SetActive(false);
        openBoardSettingsButton.onClick.AddListener(ToggleBoardSettings);
        closeBoardSettingsButton.onClick.AddListener(ToggleBoardSettings); 
    }

    void Update()
    {
        if (isPanelOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleBoardSettings();
        }
    }

    public void ToggleBoardSettings()
    {
        isPanelOpen = !isPanelOpen;
        boardSettingsPanel.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            DisableOtherUI();
        }
        else
        {
            EnableAllUIElements();
        }
    }

    private void DisableOtherUI()
    {
        goToPartPanel.SetActive(false);
        monteCarloSelectionPanel.SetActive(false);
        basicControlsPanel.SetActive(false); 
        exitButton.gameObject.SetActive(true); 
    }

    private void EnableAllUIElements()
    {
        goToPartPanel.SetActive(true);
        monteCarloSelectionPanel.SetActive(true);
        basicControlsPanel.SetActive(true);
    }
}
