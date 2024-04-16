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
    public CSVHandler ResultsShower;

    void OnEnable()
    {
        if (Preview != null)
            Preview.SetActive(false);

        StartCoroutine(WaitForEscape());
    }

    public void ShowWhiskerList()
    {
        if (Preview != null)
            Preview.SetActive(true);
        ResultsShower.ShowCSVFile("test.csv");
        StartCoroutine(WaitForKeyPress());
    }

    public void ShowSimState()
    {
        if (Preview != null)
            Preview.SetActive(true);
        ResultsShower.ShowCSVFile(""); // Provide the appropriate file name here
        StartCoroutine(WaitForKeyPress());
    }

    public void ShowMonteCarloReport()
    {
        if (Preview != null)
            Preview.SetActive(true);
        ResultsShower.ShowCSVFile(""); // Provide the appropriate file name here
        StartCoroutine(WaitForKeyPress());
    }

    public void ShowBridgedWhiskersReport()
    {
        if (Preview != null)
            Preview.SetActive(true);
        ResultsShower.ShowCSVFile(""); // Provide the appropriate file name here
        StartCoroutine(WaitForKeyPress());
    }

    // Coroutine to wait for any key press to hide the image
    IEnumerator WaitForKeyPress()
    {
        // Wait until any key is pressed
        yield return new WaitUntil(() => Input.anyKeyDown);

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
