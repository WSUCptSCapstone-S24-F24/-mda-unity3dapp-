using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;

public class MonteCarloLauncher : MonoBehaviour
{
    public int numSimulations = 2;
    private string unityAppPath = "not found";

    void Start()
    {
        Button launchButton = GetComponent<Button>();

        //Parse the command line argument -filepath
        string[] args = System.Environment.GetCommandLineArgs();
        launchButton.onClick.AddListener(OnClick);
        
        //UnityEngine.Debug.Log("Command line argument length:" + args.Length);
        if (args.Length > 1)
        {
            for (int i = 1; i < args.Length; i++)
            {
                //UnityEngine.Debug.Log("Command line argument: " + args[i]);
                if (args[i] == "-filePath" && i + 1 < args.Length)
                {
                    unityAppPath = args[i + 1];
                    //UnityEngine.Debug.Log("Unity app path: " + unityAppPath);
                    break;
                }
            }
        }
    }

    void OnClick()
    {
        UnityEngine.Debug.Log("This application is not ready to run a Monte Carlo Simuation");
        return; // Temp handler (application not ready to run Monte Carlo sim)
        if (unityAppPath == "not found")
        {
            UnityEngine.Debug.LogError("Unity app path not found");
            return;
        }
        for (int i = 1; i <= numSimulations; i++)
        {
            Process process = new Process();
            process.StartInfo.FileName = unityAppPath;
            process.StartInfo.Arguments = "-batchmode -nographics -simNumber " + i;
            
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
        }
    }
}
