using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResultsHandler : MonoBehaviour
{
    public GameObject Preview;
    public GameObject MainMenu;
    public ResultsProcessor ResultsProcessor;
    private int lastSimNum;

    void OnEnable()
    {
        if (Preview != null)
            Preview.SetActive(false);
        GameObject MainController = GameObject.Find("MainController");
        if (MainController != null)
        {
            // Get the SceneHandler component and access sceneNum
            MainController handler = MainController.GetComponent<MainController>();
            if (handler != null)
                lastSimNum = handler.SimNumber - 1;
            else
                UnityEngine.Debug.LogError("SceneHandler component is not found on the SceneControl object!");
        }
        else
            UnityEngine.Debug.LogError("SceneControl GameObject is not found!");
        StartCoroutine(WaitForEscape());
    }

    public void ShowWhiskerList()
    {
        if (Preview != null)
            Preview.SetActive(true);
        ResultsProcessor.ShowCSVFile($"whiskers_log_{lastSimNum}.csv");

        StartCoroutine(WaitForKeyPress());
    }
    
    public void ShowMonteCarloReport()
    {
        if (Preview != null)
            Preview.SetActive(true);
        ResultsProcessor.ShowCSVFile($"montecarlo_log_{lastSimNum}.csv");
        StartCoroutine(WaitForKeyPress());
    }

    public void ShowBridgedWhiskersReport()
    {
        if (Preview != null)
            Preview.SetActive(true);
        ResultsProcessor.ShowCSVFile($"bridgedcomponents_log_{lastSimNum}.csv");
        StartCoroutine(WaitForKeyPress());
    }

    // Coroutine to wait for any key press to hide the image
    IEnumerator WaitForKeyPress()
    {
        // Wait until any key is pressed to exit
        yield return new WaitUntil(() => Input.anyKeyDown && !Input.GetMouseButtonDown(0));

        if (Preview != null)
            Preview.SetActive(false);
    }

    // Coroutine to wait for any key press to hide the content
    IEnumerator WaitForEscape()
    {
        // Wait until any key is pressed
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));
        SwitchToMain();
    }

    private void SwitchToMain()
    {
        GameObject resultsMenu = GameObject.Find("ResultsCanvas");
        if (MainMenu != null && resultsMenu != null)
        {
            MainMenu.SetActive(true);
            resultsMenu.SetActive(false);
        }
        else
        {
            UnityEngine.Debug.LogError("MainMenu or ResultsMenu not found in the scene!");
        }
    }
}
