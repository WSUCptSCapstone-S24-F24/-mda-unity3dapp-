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
    public int sceneNum;
    private bool isSceneLoaded = false;

    public TMP_InputField WhiskerCounteText;
    public TMP_InputField LengthSigmaText;
    public TMP_InputField LengthMuText;
    public TMP_InputField WidthSigmaText;
    public TMP_InputField WidthMuText;
    public TMP_InputField SpawnAreaSizeText;
    public SimState simState;

    string jsonPath;

    public void Start()
    {
        jsonPath = Application.persistentDataPath + "/SimState.json";

        if (System.IO.File.Exists(jsonPath))
        {
            // JSON folder exists, read data from file and initialize SimState object
            string jsonString = System.IO.File.ReadAllText(jsonPath);
            simState = JsonUtility.FromJson<SimState>(jsonString);
        }
        else
        {
            // JSON folder doesn't exist, create SimState object with default constructor
            simState = new SimState();
            simState.SaveSimToJSON(jsonPath);
        }

        WhiskerCounteText.text =simState.WhiskerCount.ToString();
        LengthSigmaText.text = simState.LengthSigma.ToString();
        LengthMuText.text = simState.LengthMu.ToString();
        WidthSigmaText.text = simState.WidthSigma.ToString();
        WidthMuText.text = simState.WidthMu.ToString();
        SpawnAreaSizeText.text = simState.spawnAreaSize.ToString();
        
        LoadScene(sceneNum);
        // Set the default value of the text field


        // Get the float value from the text field
        getSimInputs(); 

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

    public void LoadScene(int buildnum)
    {
        getSimInputs();
        StartCoroutine(simState.SaveSimToJSON(jsonPath));
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
}
