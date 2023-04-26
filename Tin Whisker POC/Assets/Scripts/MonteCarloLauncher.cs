using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;

public class MonteCarloLauncher : MonoBehaviour
{
    public int numSimulations = 2;
    public string unityAppPath = "C:/Users/Gage Unruh/Tin Whisker POC/Builds/Beta/Tin Whisker POC.exe";

    void Start()
    {
        Button launchButton = GetComponent<Button>();
        launchButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        unityAppPath = Application.persistentDataPath + "/Builds/Tin Whisker POC.exe";
        for (int i = 1; i <= numSimulations; i++)
        {
            Process process = new Process();
            process.StartInfo.FileName = unityAppPath;
            process.StartInfo.Arguments = "-batchmode -nographics -simNumber " + i;
            //process.StartInfo.Arguments = "-simNumber " + i;
            process.StartInfo.UseShellExecute = false;
            //process.StartInfo.CreateNoWindow = true;
            process.Start();
        }
    }
}
