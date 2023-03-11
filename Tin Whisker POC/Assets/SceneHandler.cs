using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour
{
    
    private Scene loadedScene;
    public string sceneName;
    public int sceneNum;
    private bool isSceneLoaded = false;

    public TMP_InputField WhiskerCounteText;
    public TMP_InputField LengthSigmaText;
    public TMP_InputField LengthMuText;
    public float whiskerCount = 1000.0f;
    public float lengthSigma = 0.5f;
    public float lengthMu = 0.005f;

    public void Start()
    {
        
        LoadScene(sceneNum);
        // Set the default value of the text field
        WhiskerCounteText.text = "1000.0";
        LengthSigmaText.text = "0.5";
        LengthMuText.text = "0.005";
        
        // Get the float value from the text field
        if (float.TryParse(WhiskerCounteText.text, out float result))
        {
            whiskerCount = result;
        } else
        {
            Debug.Log("Whisker Count is not a float");
        }

        if (float.TryParse(LengthSigmaText.text, out float result2))
        {
            lengthSigma = result2;
        }
        else
        {
            Debug.Log("Length Sigma is not a float");
        }

        if (float.TryParse(LengthMuText.text, out float result3))
        {
            lengthMu = result3;
        }
        else
        {
            Debug.Log("Length Mu is not a float");
        }

    }


    public void LoadScene(int buildnum)
    {
        if (!isSceneLoaded)
        {
            StartCoroutine(LoadSceneAsync(buildnum));
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

    public void ReloadScene()
    {
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

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadedScene.name));
    }
}
