using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;

public class MonteCarloLauncher : MonoBehaviour
{
    public int numSimulations = 10;
    public string unityAppPath = "path/to/your/built/unity/application";

    void Start()
    {
        Button launchButton = GetComponent<Button>();
        launchButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        for (int i = 0; i < numSimulations; i++)
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
