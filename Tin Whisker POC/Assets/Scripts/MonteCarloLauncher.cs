using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;

public class MonteCarloLauncher : MonoBehaviour
{
    public int numSimulations = 2; // 2 Default
    private string unityAppPath = "not found";

    public void OnClickStart()
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
