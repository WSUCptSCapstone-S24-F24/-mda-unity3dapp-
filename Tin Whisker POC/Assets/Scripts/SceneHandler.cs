using System.Collections;
using System.Collections.Generic;
using SimInfo;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    private Scene loadedScene;
    public string sceneName;
    public int sceneNum = 1;
    private bool isSceneLoaded = false;
    private bool isSceneRunning = false;
    private bool argsParsed = false;
    public bool fileOpened = false;
    public int SimNumber = 0;    
    private WhiskerSim whiskerSim;
    public GameObject ResultsCanvas;
    public MonteCarloLauncher monteCarloLauncher;

    public Button startButton;
    public Button endButton;
    private Coroutine regularEndSimCoroutine;
    private PopupManager popupManager;

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
    public SimState simState;
    public string objfilePath;
    public string mtlfilePath;

    public TMP_InputField VibrationSpeedText;
    public TMP_InputField VibrationAmplitudeText;
    public TMP_InputField ShockIntensityText;
    public TMP_InputField ShockDurationText;
    public Shocker Shocker;
    public OpenVibration OpenVibration;
    public Vibration Vibration;
    public Shock Shock;

    private string rootJsonPath;
    private string myJsonPath;

    public void Start()
    {
        rootJsonPath = Application.persistentDataPath + "/SimState.JSON";
        ParseArgs();
        startButton = GameObject.Find("Start_Button").GetComponent<Button>();
        endButton = GameObject.Find("End_Button").GetComponent<Button>();
        popupManager = FindObjectOfType<PopupManager>();
        endButton.gameObject.SetActive(false);

        if (SimNumber == 0)
        {
            Debug.Log("Root Sim Start");
            RootSimSetup();
        }
        else
        {
            MonteCarloSimStart();
        }
        if (simState == null)
        {
            ShowDebugMessage("SimState not found");
        }
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

    private void RootSimSetup()
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
                ShowDebugMessage("SimState class not found");
            }
            else
            {
                Debug.Log("SimState class found\nsimState: " + simState.ToString());
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
        getSimInputs();
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

    private void MonteCarloSimStart()
    {
        myJsonPath = Application.persistentDataPath + "/SimState.JSON";
        if (System.IO.File.Exists(rootJsonPath))
        {
            // JSON folder exists, read data from file and initialize SimState object
            string jsonString = System.IO.File.ReadAllText(rootJsonPath);
            simState = JsonUtility.FromJson<SimState>(jsonString);
            simState.simNumber = SimNumber;
            simState.SaveSimToJSON(myJsonPath);

            // if (simState.fileOpened)
            // {
                // objfilePath = simState.objfilePath;
                // mtlfilePath = simState.mtlfilePath;
                // fileOpened = simState.fileOpened;
                // Get File Browser object in scene by name and call load from file path
                // GameObject fileBrowser = GameObject.Find("FileBrowser");
                // fileBrowser.GetComponent<LoadFile>().LoadFromPath(objfilePath, mtlfilePath);
            // }
        //     StartCoroutine(MonteCarloEndSimulationAfterDuration());
        // }
        // else
        // {
        //     ShowDebugMessage("Root Sim JSON file does not exist");
        }
    }

    public void getSimInputs()
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
            monteCarloLauncher.numSimulations = result13;

        if (float.TryParse(VibrationSpeedText.text, out float result14))
            simState.vibrationSpeed = result14;

        if (float.TryParse(VibrationAmplitudeText.text, out float result15))
            simState.vibrationAmplitude = result15;

        if (float.TryParse(ShockIntensityText.text, out float result16))
            simState.ShockIntensity = result16;

        if (float.TryParse(ShockDurationText.text, out float result17))
            simState.ShockDuration = result17;
    }

    public void LoadScene(int buildnum)
    {
        if (isSceneRunning)
        {
            ShowDebugMessage("Simulation is already running.");
            return; // Exit if the simulation is already running
        }

        if (fileOpened)
        {
            simState.simNumber = SimNumber;
            Debug.Log("Sim num: " + SimNumber);
            getSimInputs();
            if (fileOpened)
            {
                simState.objfilePath = objfilePath;
                simState.mtlfilePath = mtlfilePath;
                simState.fileOpened = fileOpened;
            }

            isSceneRunning = true;
            startButton.interactable = false;
            endButton.gameObject.SetActive(true);


            simState.SaveSimToJSON(myJsonPath);
            if (!isSceneLoaded)
            {
                StartCoroutine(LoadSceneAsync(buildnum));
            }
            else
            {
                ReloadScene(buildnum);
            }

            if (Shocker.shocking == true)
            {
                Debug.Log("Starting shock...");
                StartCoroutine(FindObjectOfType<Shock>().InitializeShock());
            }

            if (OpenVibration.vibrate == true)
            {
                Debug.Log("Starting vibration...");
                StartCoroutine(FindObjectOfType<Vibration>().InitializeVibration());
            }

            regularEndSimCoroutine = StartCoroutine(RegularEndSimulationAfterDuration());

            SimNumber++;
        }
        else
        {
            ShowDebugMessage("No loaded PCB");
        }
    }

    IEnumerator LoadSceneAsync(int buildnum)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildnum, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadedScene = SceneManager.GetSceneByBuildIndex(buildnum);
        isSceneLoaded = true;
    }

    public void UnloadScene(int buildnum)
    {
        if (isSceneLoaded)
        {
            ResetBoardPosition();
            StartCoroutine(UnloadSceneAsync(buildnum));
        }
        else
        {
            Debug.LogWarning("Scene is not loaded.");
        }
    }

    IEnumerator UnloadSceneAsync(int buildnum)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(buildnum);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }
        isSceneLoaded = false;
    }

    public void ReloadScene(int buildnum)
    {
        loadedScene = SceneManager.GetSceneByBuildIndex(buildnum);
        if (isSceneLoaded)
        {
            SceneManager.UnloadSceneAsync(loadedScene);

            StartCoroutine(ReloadSceneAsync());
        }
    }

    IEnumerator ReloadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(
            loadedScene.name,
            LoadSceneMode.Additive
        );
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        loadedScene = SceneManager.GetSceneByBuildIndex(1);

        SceneManager.SetActiveScene(loadedScene);
    }

    public void MonteCarlosim()
    {
        getSimInputs();
        LoadScene(2);
        StartCoroutine(simState.SaveSimToJSONasync(rootJsonPath));
    }

    public void ParseArgs()
    {
        if (argsParsed)
        {
            return;
        }
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-simNumber" && args.Length > i + 1)
            {
                int simNumber;
                if (int.TryParse(args[i + 1], out simNumber))
                {
                    SimNumber = simNumber;
                }
            }
        }
        if (SimNumber == -1)
        {
            SimNumber = 0;
        }
        argsParsed = true;
    }

    public void GetResultsForward()
    {
        if (!whiskerSim)
        {
            //Get object with sim tag then get its wire sim script
            whiskerSim = GameObject.FindGameObjectWithTag("Sim").GetComponent<WhiskerSim>();
        }

        //Get the results from the wire sim script
        whiskerSim.SaveResults(SimNumber);
    }

    public void SwitchToResults()
    {
        if (ResultsCanvas != null)
            ResultsCanvas.SetActive(true);
        GameObject.Find("MainCanvas").SetActive(false);
    }

    IEnumerator MonteCarloEndSimulationAfterDuration()
    {
        float simulationDuration;
        if (simState != null && simState.simDuration > 0)
        {
            simulationDuration = simState.simDuration;
        }
        else
        {
            simulationDuration = 10f;
        }
        yield return new WaitForSeconds(simulationDuration);
        if (whiskerSim != null)
        {
            whiskerSim.SaveResults(SimNumber);
            QuitApplication();
        }
        else
        {
            ShowDebugMessage("WireSim not found");
            GetResultsForward();
            QuitApplication();
        }
    }

    IEnumerator RegularEndSimulationAfterDuration()
    {
        ShowDebugMessage("Simulation starting.");
        // Check if simState and its duration are set, otherwise use a default value
        float simulationDuration =
            (simState != null && simState.simDuration > 0) ? simState.simDuration : 10f;

        // Wait for the specified simulation duration
        yield return new WaitForSeconds(simulationDuration);

        // Ensure whiskerSim is available; find it if not already referenced
        if (whiskerSim == null)
        {
            whiskerSim = FindObjectOfType<WhiskerSim>();
        }

        if (whiskerSim != null)
        {
            // Assuming SaveResults and ClearCylinders are operations that can complete immediately
            whiskerSim.SaveResults();
            whiskerSim.ClearCylinders();

            // Optional: wait a frame to ensure operations have completed if they involve Unity's messaging system
            yield return null;
        }
        else
        {
            ShowDebugMessage("WhiskerSim not found. Unable to save results or clear cylinders.");
        }

        // Proceed to call cleanup for all WhiskerCollider instances
        foreach (WhiskerCollider whiskerCollider in FindObjectsOfType<WhiskerCollider>())
        {
            whiskerCollider.Cleanup();
        }

        // Finally, unload the scene
        ShowDebugMessage("Simulation ended.");
        UnloadScene(sceneNum);
        isSceneRunning = false;
        startButton.interactable = true;
        endButton.gameObject.SetActive(false);
    }

    // This callback method will be called by WhiskerSim when the simulation ends and it's time to unload the scene
    private void OnSimulationEnded()
    {
        // Call the Cleanup on all WhiskerCollider instances before unloading the scene
        foreach (WhiskerCollider whiskerCollider in FindObjectsOfType<WhiskerCollider>())
        {
            whiskerCollider.Cleanup();
        }

        // Now you can unload the scene

        UnloadScene(sceneNum);
    }

    public void EndSimulationEarly()
    {
        Debug.Log("User ended simulation.");
        ShowDebugMessage("User ended simulation.");
        // Stop the coroutine that is waiting for the simulation to end
        StopCoroutine(regularEndSimCoroutine);

        if (whiskerSim == null)
        {
            whiskerSim = FindObjectOfType<WhiskerSim>();
        }

        // Perform any necessary cleanup, similar to what would happen at the end of the simulation
        whiskerSim.SaveResults();
        whiskerSim.ClearCylinders();

        // Reactivate the Start button, hide the Exit button
        startButton.interactable = true;
        endButton.gameObject.SetActive(false);

        // Set the simulation state to not running
        isSceneRunning = false;

        // Unload the scene or reset the simulation as needed
        UnloadScene(sceneNum);
        ShowDebugMessage("Simulation ended.");
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
