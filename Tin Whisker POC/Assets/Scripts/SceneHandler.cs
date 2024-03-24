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
            Debug.Log("Intitializing Simulation");
            InitializeSimulation();
        }
        else
        {
            MonteCarloSimStart();
        }
        if (simState == null)
        {
            Debug.LogError("SimState not found");
        }

        LoadScene(sceneNum);
    }

    private void InitializeSimulation()
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
        SpawnAreaSizeXText.text = "25";
        SpawnAreaSizeYText.text = "1";
        SpawnAreaSizeZText.text = "25";


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
            StartCoroutine(EndSimulationAfterDuration());

        }
        else
        {
            Debug.LogError("Root Sim JSON file does not exist");
        }
    }


    public void getSimInputs()
    {
        if (int.TryParse(WhiskerDensityText.text, out int result))
        {
            simState.whiskerDensity = result;
        }
        else
        {
            Debug.Log("Whisker Count is not a float");
        }

        if (float.TryParse(LengthSigmaText.text, out float result2))
        {
            simState.LengthSigma = result2;
        }
        else
        {
            Debug.Log("Length Sigma is not a float");
        }

        if (float.TryParse(LengthMuText.text, out float result3))
        {
            simState.LengthMu = result3;
        }
        else
        {
            Debug.Log("Length Mu is not a float");
        }

        if (float.TryParse(WidthSigmaText.text, out float result4))
        {
            simState.WidthSigma = result4;
        }
        else
        {
            Debug.Log("Width Sigma is not a float");
        }

        if (float.TryParse(WidthMuText.text, out float result5))
        {
            simState.WidthMu = result5;
        }
        else
        {
            Debug.Log("Width Mu is not a float");
        }

        if (float.TryParse(SpawnAreaSizeXText.text, out float result6))
        {
            simState.spawnAreaSizeX = result6;
        }
        else
        {
            Debug.Log("Spawn Area Size is not a float");
        }

        if (float.TryParse(SpawnAreaSizeYText.text, out float result7))
        {
            simState.spawnAreaSizeY = result7;
        }
        else
        {
            Debug.Log("Spawn Area Size is not a float");
        }

        if (float.TryParse(SpawnAreaSizeZText.text, out float result8))
        {
            simState.spawnAreaSizeZ = result8;
        }
        else
        {
            Debug.Log("Spawn Area Size is not a float");
        }

        if (float.TryParse(SimDurationText.text, out float result9))
        {
            simState.simDuration = result9;
        }
        else
        {
            Debug.Log("Sim Duration is not a float");
        }

        if (int.TryParse(SimQuantityText.text, out int result10))
        {
            monteCarloLauncher.numSimulations = result10;
        }
        else
        {
            Debug.Log("Sim Quantity is not a float");
        }
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
            //Get object with sim tag then get its whisker sim script
            whiskerSim = GameObject.FindGameObjectWithTag("Sim").GetComponent<WhiskerSim>();
        }

        //Get the results from the whisker sim script
        whiskerSim.SaveResults(mySimNumber);
    }

    IEnumerator EndSimulationAfterDuration()
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
            whiskerSim.QuitApplication();
        }
        else
        {
            Debug.LogError("Whisker Sim not found");
            GetResultsForward();
            whiskerSim.QuitApplication();
        }
    }

    /// <summary>
    /// Starts a whisker simulation
    /// </summary>
    /// <param name="duration"></param>
    public void StartWhiskerSimulation(int duration)
    {
        if (whiskerSim != null)
        {
            whiskerSim.RunSimulation(duration);
        }
        else
        {
            Debug.LogError("Whisker Sim not found");
        }
    }
}