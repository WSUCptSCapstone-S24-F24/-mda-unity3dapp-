using System.Collections;
using System.Collections.Generic;
using SimInfo;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    // private Scene loadedScene;
    public GameObject SimulationObject;
    public int SimNumber = 0;
    public SimState simState;
    public GameObject ResultsCanvas;


    public TMP_InputField WhiskerDensityText;
    public TMP_InputField LengthSigmaText;
    public TMP_InputField LengthMuText;
    public TMP_InputField WidthSigmaText;
    public TMP_InputField WidthMuText;
    public TMP_InputField SpawnAreaSizeXText;
    public TMP_InputField SpawnAreaSizeYText;
    public TMP_InputField SpawnAreaSizeZText;
    public TMP_InputField SpawnPositionXText;
    public TMP_InputField SpawnPositionYText;
    public TMP_InputField SpawnPositionZText;
    public TMP_InputField SimDurationText;
    public TMP_InputField SimQuantityText;
    public Button endSimEarlyButton;

    public bool PCBloaded = false;
    public string objfilePath;
    public string mtlfilePath;

    private string rootJsonPath;
    private string myJsonPath;

    private PopupManager popupManager;
    private WhiskerSim whiskerSim;
    private MonteCarloSim monteCarloSim;

    public void Start()
    {
        rootJsonPath = Application.persistentDataPath + "/SimState.JSON";
        popupManager = FindObjectOfType<PopupManager>();
        endSimEarlyButton.gameObject.SetActive(false);

        ParameterSetup();
    }

    public void ShowDebugMessage(string message)
    {
        if (popupManager != null)
        {
            PopupManagerSingleton.Instance.ShowPopup(message);
        }
        else
        {
            Debug.LogError("PopupManager is not assigned. Cannot show popup.");
        }
    }

    private void ParameterSetup()
    {
        myJsonPath = rootJsonPath;
        if (System.IO.File.Exists(rootJsonPath))
        {
            // JSON folder exists, read data from file and initialize SimState object
            string jsonString = System.IO.File.ReadAllText(rootJsonPath);
            simState = JsonUtility.FromJson<SimState>(jsonString);
            simState.simNumber = SimNumber;
        }
        else
        {
            // JSON folder doesn't exist, create SimState object with default constructor
            Debug.Log("root JSON not found\nSaving class to JSON");
            simState = new SimState();
            simState.simNumber = SimNumber;
            if (simState == null)
            {
                ShowDebugMessage("No sim state found");
            }
            else
            {
                Debug.Log("Sim state found");
                Debug.Log("rootJsonPath: " + rootJsonPath);
            }
            simState.SaveSimToJSON(rootJsonPath);
        }

        WhiskerDensityText.text = simState.whiskerDensity.ToString();

        LengthSigmaText.text = simState.LengthSigma.ToString();
        LengthMuText.text = simState.LengthMu.ToString();

        WidthSigmaText.text = simState.WidthSigma.ToString();
        WidthMuText.text = simState.WidthMu.ToString();

        SpawnAreaSizeXText.text = simState.spawnAreaSizeX.ToString();
        SpawnAreaSizeYText.text = simState.spawnAreaSizeY.ToString();
        SpawnAreaSizeZText.text = simState.spawnAreaSizeZ.ToString();

        SpawnPositionXText.text = simState.spawnPositionX.ToString();
        SpawnPositionYText.text = simState.spawnPositionY.ToString();
        SpawnPositionZText.text = simState.spawnPositionZ.ToString();


        // Get the float value from the text field
        GetSimInputs();
        simState.SaveSimToJSON(rootJsonPath);
        SetUpSpawnBox();
    }

    private void SetUpSpawnBox()
    {
        GameObject spawnBoxObject = GameObject.Find("WhiskerSpawnBox");
        if (spawnBoxObject != null)
        {
            SpawnBoxController spawnBoxController = spawnBoxObject.GetComponent<SpawnBoxController>();
            if (spawnBoxController != null)
            {
                // Call UpdateCubeProperties on the SpawnBoxController instance
                spawnBoxController.UpdateCubeProperties();
            }
            else
            {
                Debug.LogError("SpawnBoxController component not found on GameObject 'WhiskerSpawnBox'.");
            }
        }
        else
        {
            Debug.LogError("GameObject 'WhiskerSpawnBox' not found in the scene.");
        }
    }

    public void GetSimInputs()
    {
        if (int.TryParse(WhiskerDensityText.text, out int result))
            simState.whiskerDensity = result;

        if (float.TryParse(LengthSigmaText.text, out float result2))
            simState.LengthSigma = result2;

        if (float.TryParse(LengthMuText.text, out float result3))
            simState.LengthMu = result3;

        if (float.TryParse(WidthSigmaText.text, out float result4))
            simState.WidthSigma = result4;

        if (float.TryParse(WidthMuText.text, out float result5))
            simState.WidthMu = result5;

        if (float.TryParse(SpawnAreaSizeXText.text, out float result6))
            simState.spawnAreaSizeX = result6;

        if (float.TryParse(SpawnAreaSizeYText.text, out float result7))
            simState.spawnAreaSizeY = result7;

        if (float.TryParse(SpawnAreaSizeZText.text, out float result8))
            simState.spawnAreaSizeZ = result8;

        if (float.TryParse(SpawnPositionXText.text, out float result9))
            simState.spawnPositionX = result9;

        if (float.TryParse(SpawnPositionYText.text, out float result10))
            simState.spawnPositionY = result10;

        if (float.TryParse(SpawnPositionZText.text, out float result11))
            simState.spawnPositionZ = result11;

        if (float.TryParse(SimDurationText.text, out float result12))
            simState.simDuration = result12;

        if (int.TryParse(SimQuantityText.text, out int result13))
            monteCarloSim.numSimulations = result13;
    }

    public void RunSimulation()
    {
        if (PCBloaded)
        {
            ShowDebugMessage("Simulation starting. ");
            simState.simNumber = SimNumber;
            Debug.Log("Sim num: " + SimNumber);
            GetSimInputs();

            if (PCBloaded)
            {
                // TODO: Show object file and mtl file path in results so user knows which PCB was used
                simState.objfilePath = objfilePath;
                simState.mtlfilePath = mtlfilePath;
            }

            // TODO: Make all but end sim button be non-interactable
            GameObject.Find("RunSimButton").GetComponent<Button>().interactable = false;
            endSimEarlyButton.gameObject.SetActive(true);

            simState.SaveSimToJSON(myJsonPath);

            if (SimulationObject) {
                whiskerSim = SimulationObject.GetComponent<WhiskerSim>();
                whiskerSim.StartSim(SimNumber, simState.simDuration);
                StartCoroutine(EndOfSimActions());
                SimNumber++;
            }
            else
                Debug.LogError("No Simulation Object found");

        }
        else
        {
            ShowDebugMessage("No loaded PCB");
        }
    }

    IEnumerator EndOfSimActions() {
        yield return new WaitUntil(() => whiskerSim.IsSimulationEnded);

        ShowDebugMessage("Simulation ended.");
        GameObject.Find("RunSimButton").GetComponent<Button>().interactable = true;
        endSimEarlyButton.gameObject.SetActive(false);
    }

    public void EndSimulationEarly()
    {
        ShowDebugMessage("User interupt. ");
        whiskerSim.EndSimulationEarly();
    }

    public void SwitchToResults()
    {
        if (ResultsCanvas != null)
            ResultsCanvas.SetActive(true);
        GameObject.Find("MainCanvas").SetActive(false);
    }

    public void QuitApplication()
    {
        Debug.Log("Quitting application");
        Debug.Log("Sim number: " + simState.simNumber);
        // Quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Application.Quit();
    }
}
