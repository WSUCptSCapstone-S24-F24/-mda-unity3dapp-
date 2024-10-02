using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimInfo;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    // private Scene loadedScene;
    public GameObject WhiskerSimulationObject;
    // ****************** TEST ******************
    public GameObject MonteCarloSimulationObject;
    // ****************** TEST ******************
    public int SimNumber = 0;
    public SimState simState;
    private WhiskerSim whiskerSim;
    private MonteCarloSim monteCarloSim;


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

    public TMP_InputField VibrationSpeedText;
    public TMP_InputField VibrationAmplitudeText;
    public TMP_InputField ShockIntensityText;
    public TMP_InputField ShockDurationText;
    public Shocker Shocker;
    public OpenVibration OpenVibration;
    public Vibration Vibration;
    public Shock Shock;

    private PopupManager popupManager;

    public void Start()
    {
        rootJsonPath = Application.persistentDataPath + "/SimState.JSON";
        popupManager = FindObjectOfType<PopupManager>();
        endSimEarlyButton.gameObject.SetActive(false);
        whiskerSim = WhiskerSimulationObject.GetComponent<WhiskerSim>();
        monteCarloSim = MonteCarloSimulationObject.GetComponent<MonteCarloSim>();

        ParameterSetup();
    }

    private void ResetBoardPosition()
    {
        GameObject board = GameObject.FindWithTag("Board");
        if (board != null)
        {
            Vector3 originalPosition = board.transform.position;
            originalPosition.y = -13.7f; // This is the height set in LoadFile script
            board.transform.position = originalPosition;
            Debug.Log("Board position reset to: " + originalPosition);
        }
        else
        {
            Debug.LogError("Cannot reset board position. Board not found.");
        }
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

        VibrationSpeedText.text = simState.vibrationSpeed.ToString();
        VibrationAmplitudeText.text = simState.vibrationAmplitude.ToString();
        ShockIntensityText.text = simState.ShockIntensity.ToString();
        ShockDurationText.text = simState.ShockDuration.ToString();


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

        if (float.TryParse(VibrationSpeedText.text, out float result14))
            simState.vibrationSpeed = result14;

        if (float.TryParse(VibrationAmplitudeText.text, out float result15))
            simState.vibrationAmplitude = result15;

        if (float.TryParse(ShockIntensityText.text, out float result16))
            simState.ShockIntensity = result16;

        if (float.TryParse(ShockDurationText.text, out float result17))
            simState.ShockDuration = result17;
    }

    public void RunSimulation()
    {
        if (PCBloaded)
        {
            ShowDebugMessage("Simulation starting. ");
            simState.simNumber = SimNumber;
            Debug.Log("Sim num: " + SimNumber);
            GetSimInputs();

            simState.objfilePath = objfilePath;
            simState.mtlfilePath = mtlfilePath;

            // Check if whiskerSim is null
            if (whiskerSim == null)
            {
                Debug.LogError("whiskerSim is null");
            }

            // Check if simState is null
            if (simState == null)
            {
                Debug.LogError("simState is null");
            }

            GameObject runSimButton = GameObject.Find("RunSimButton");
            if (runSimButton == null)
            {
                Debug.LogError("RunSimButton not found.");
            }
            else
            {
                runSimButton.GetComponent<Button>().interactable = false;
            }

            if (Shocker.shocking)
            {
                Debug.Log("Starting shock...");
                StartCoroutine(FindObjectOfType<Shock>().InitializeShock());
            }

            if (OpenVibration.vibrate)
            {
                Debug.Log("Starting vibration...");
                StartCoroutine(FindObjectOfType<Vibration>().InitializeVibration());
            }

            endSimEarlyButton.gameObject.SetActive(true);
            simState.SaveSimToJSON(myJsonPath);

            whiskerSim.RunSim(ref SimNumber, simState.simDuration);  // Line 248
            StartCoroutine(EndOfSimActions());
        }
        else
        {
            ShowDebugMessage("No loaded PCB");
        }
    }

    IEnumerator EndOfSimActions() {
        yield return new WaitUntil(() => whiskerSim.NumberSimsRunning == 0);

        ShowDebugMessage("Simulation ended.");
        GameObject.Find("RunSimButton").GetComponent<Button>().interactable = true;
        endSimEarlyButton.gameObject.SetActive(false);
    }

    public void EndSimulationEarly()
    {
        ShowDebugMessage("User interupt. ");
        whiskerSim.EndSimulationEarly();
    }

    public void RunMonteCarloSimulation() {
        if (PCBloaded)
        {
            ShowDebugMessage("Simulation starting. ");
            simState.simNumber = SimNumber;
            Debug.Log("Sim num: " + SimNumber);
            GetSimInputs();

            // TODO: Show object file and mtl file path in results so user knows which PCB was used
            simState.objfilePath = objfilePath;
            simState.mtlfilePath = mtlfilePath;

            // TODO: Make all but end sim button be non-interactable
            GameObject.Find("Run Monte Carlo").GetComponent<Button>().interactable = false;
            GameObject.Find("RunSimButton").GetComponent<Button>().interactable = false;

            simState.SaveSimToJSON(myJsonPath);

            monteCarloSim.RunMonteCarloSim(whiskerSim, ref SimNumber, simState.simDuration);
            StartCoroutine(EndOfMonteCarloSimActions());  // TODO: Change to end of monte carlo sim actions
        }
        else
        {
            ShowDebugMessage("No loaded PCB");
        }
    }

    IEnumerator EndOfMonteCarloSimActions() {
        yield return new WaitUntil(() => monteCarloSim.IsSimulationEnded);

        ShowDebugMessage("Monte Carlo simulation ended.");
        GameObject.Find("Run Monte Carlo").GetComponent<Button>().interactable = true;
        GameObject.Find("RunSimButton").GetComponent<Button>().interactable = true;
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
