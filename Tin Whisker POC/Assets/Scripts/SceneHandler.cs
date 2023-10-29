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

    public TMP_InputField WhiskerCounteText;
    public TMP_InputField LengthSigmaText;
    public TMP_InputField LengthMuText;
    public TMP_InputField WidthSigmaText;
    public TMP_InputField WidthMuText;
    public TMP_InputField SpawnAreaSizeText;
    public SimState simState;
    public string filePath;


    private string rootJsonPath;
    private string myJsonPath;

    public void Start()
    {
        rootJsonPath = Application.persistentDataPath + "/SimState.JSON";
        ParseArgs();

        if(mySimNumber == 0){
            Debug.Log("Root Sim Start");
            RootSimStart();
        }else{
            MonteCarloSimStart();
        }
        if(simState == null){
            Debug.LogError("SimState not found");
        }

        
        LoadScene(sceneNum);
    }

    private void RootSimStart(){
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
            if(simState == null){
                Debug.LogError("SimState class not found");
            }else{
                Debug.Log("SimState class found\nsimState: " + simState.ToString());
                Debug.Log("rootJsonPath: " + rootJsonPath);
            }
            simState.simNumber = 0;
            simState.SaveSimToJSON(rootJsonPath);
        }

        WhiskerCounteText.text =simState.WhiskerCount.ToString();
        LengthSigmaText.text = simState.LengthSigma.ToString();
        LengthMuText.text = simState.LengthMu.ToString();
        WidthSigmaText.text = simState.WidthSigma.ToString();
        WidthMuText.text = simState.WidthMu.ToString();
        SpawnAreaSizeText.text = simState.spawnAreaSize.ToString();

        // Get the float value from the text field
        getSimInputs();
        simState.simNumber = 0;
        simState.SaveSimToJSON(rootJsonPath);
    }

    private void MonteCarloSimStart(){
        myJsonPath = Application.persistentDataPath + "/SimState" + mySimNumber + ".JSON";
        if (System.IO.File.Exists(rootJsonPath))
        {
            // JSON folder exists, read data from file and initialize SimState object
            string jsonString = System.IO.File.ReadAllText(rootJsonPath);
            simState = JsonUtility.FromJson<SimState>(jsonString);
            simState.simNumber = mySimNumber;
            simState.SaveSimToJSON(myJsonPath);
        }else{
            Debug.LogError("Root Sim JSON file does not exist");
        }
    }


    public void getSimInputs(){
        if (int.TryParse(WhiskerCounteText.text, out int result))
        {
            simState.WhiskerCount = result;
        } else
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

        if (float.TryParse(SpawnAreaSizeText.text, out float result6))
        {
            simState.spawnAreaSize = result6;
        }
        else
        {
            Debug.Log("Spawn Area Size is not a float");
        }
    }

    public void UpdateModel(GameObject userModle)
    {
        simState.Model = userModle;
    }

    public void LoadScene(int buildnum)
    {
        getSimInputs();
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

    IEnumerator LoadSceneAsync(int buildnum )
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
        if(isSceneLoaded)
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

    public void MonteCarlosim(){
        getSimInputs();
        StartCoroutine(simState.SaveSimToJSONasync(rootJsonPath));
        LoadScene(2);
    }

    public void ParseArgs()
    {
        if (argsParsed) {
            return;
        }
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++) {
            if (args[i] == "-simNumber" && args.Length > i + 1) {
                int simNumber;
                if (int.TryParse(args[i + 1], out simNumber)) {
                    mySimNumber = simNumber;
                }
            }
        }
        if(mySimNumber == -1){
            mySimNumber = 0;
        }
        argsParsed = true;
    }
}
