using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using SimInfo;

public class SceneHandler : MonoBehaviour
{

    private Scene loadedScene;
    public string sceneName;
    public int sceneNum = 1;
    private bool isSceneLoaded = false;
    private bool argsParsed = false;
    public bool fileOpened = false;
    private int mySimNumber = -1;
    private WhiskerSim whiskerSim;
    public MonteCarloLauncher monteCarloLauncher;

    public TMP_InputField WhiskerDensityText;
    public TMP_InputField LengthSigmaText;
    public TMP_InputField LengthMuText;
    public TMP_InputField WidthSigmaText;
    public TMP_InputField WidthMuText;
    public TMP_InputField SpawnHeight;
    public TMP_InputField SpawnAreaSizeXText;
    public TMP_InputField SpawnAreaSizeYText;
    public TMP_InputField SpawnAreaSizeZText;
    public TMP_InputField SimDurationText;
    public TMP_InputField SimQuantityText;
    public SimState simState;
    public string objfilePath;
    public string mtlfilePath;


    private string rootJsonPath;
    private string myJsonPath;

    public void Start()
    {
        rootJsonPath = Application.persistentDataPath + "/SimState.JSON";
        ParseArgs();

        if (mySimNumber == 0)
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
            Debug.LogError("SimState not found");
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
        }
        else
        {
            // JSON folder doesn't exist, create SimState object with default constructor
            Debug.Log("root JSON not found\nSaving class to JSON");
            simState = new SimState();
            if (simState == null)
            {
                Debug.LogError("SimState class not found");
            }
            else
            {
                Debug.Log("SimState class found\nsimState: " + simState.ToString());
                Debug.Log("rootJsonPath: " + rootJsonPath);
            }
            simState.simNumber = 0;
            simState.SaveSimToJSON(rootJsonPath);
        }

        WhiskerDensityText.text = simState.whiskerDensity.ToString();
        LengthSigmaText.text = simState.LengthSigma.ToString();
        LengthMuText.text = simState.LengthMu.ToString();
        WidthSigmaText.text = simState.WidthSigma.ToString();
        WidthMuText.text = simState.WidthMu.ToString();
        SpawnHeight.text = simState.SpawnHeight.ToString();
        SpawnAreaSizeXText.text = simState.spawnAreaSizeX.ToString();
        SpawnAreaSizeYText.text = simState.spawnAreaSizeY.ToString();
        SpawnAreaSizeZText.text = simState.spawnAreaSizeZ.ToString();


        // Get the float value from the text field
        getSimInputs();
        simState.simNumber = 0;
        simState.SaveSimToJSON(rootJsonPath);
    }

    private void MonteCarloSimStart()
    {
        myJsonPath = Application.persistentDataPath + "/SimState" + mySimNumber + ".JSON";
        if (System.IO.File.Exists(rootJsonPath))
        {
            // JSON folder exists, read data from file and initialize SimState object
            string jsonString = System.IO.File.ReadAllText(rootJsonPath);
            simState = JsonUtility.FromJson<SimState>(jsonString);
            simState.simNumber = mySimNumber;
            simState.SaveSimToJSON(myJsonPath);

            if (simState.fileOpened)
            {
                objfilePath = simState.objfilePath;
                mtlfilePath = simState.mtlfilePath;
                fileOpened = simState.fileOpened;

                //Get File Browser object in scene by name and call load from file path
                GameObject fileBrowser = GameObject.Find("FileBrowser");
                fileBrowser.GetComponent<LoadFile>().LoadFromPath(objfilePath, mtlfilePath);
            }
            StartCoroutine(MonteCarloEndSimulationAfterDuration());
        }
        else
        {
            Debug.LogError("Root Sim JSON file does not exist");
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

        if (float.TryParse(SpawnHeight.text, out float result6))
            simState.SpawnHeight = result6;

        if (float.TryParse(SpawnAreaSizeXText.text, out float result7))
            simState.spawnAreaSizeX = result7;

        if (float.TryParse(SpawnAreaSizeYText.text, out float result8))
            simState.spawnAreaSizeY = result8;

        if (float.TryParse(SpawnAreaSizeZText.text, out float result9))
            simState.spawnAreaSizeZ = result9;

        if (float.TryParse(SimDurationText.text, out float result10))
            simState.simDuration = result10;

        if (int.TryParse(SimQuantityText.text, out int result11))
            monteCarloLauncher.numSimulations = result11;
    }

    public void LoadScene(int buildnum)
    {
        getSimInputs();
        if (fileOpened)
        {
            simState.objfilePath = objfilePath;
            simState.mtlfilePath = mtlfilePath;
            simState.fileOpened = fileOpened;
        }
        simState.SaveSimToJSON(myJsonPath);
        if (!isSceneLoaded)
        {
            StartCoroutine(LoadSceneAsync(buildnum));
        }
        else
        {
            ReloadScene(buildnum);
        }
        // RegularEndSimulationAfterDuration();
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadedScene.name, LoadSceneMode.Additive);
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
                    mySimNumber = simNumber;
                }
            }
        }
        if (mySimNumber == -1)
        {
            mySimNumber = 0;
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
        whiskerSim.SaveResults(mySimNumber);
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
            whiskerSim.SaveResults(mySimNumber);
            QuitApplication();
        }
        else
        {
            Debug.LogError("WireSim not found");
            GetResultsForward();
            QuitApplication();
        }
    }

    IEnumerator RegularEndSimulationAfterDuration()
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
            whiskerSim.SaveResults(mySimNumber);
        }
        else
        {
            Debug.LogError("WireSim not found");
            GetResultsForward();
            whiskerSim.SaveResults();
        }
        UnloadScene(sceneNum);
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