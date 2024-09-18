using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;
using System.Collections;
using Unity.Jobs;

public class MonteCarloLauncher : MonoBehaviour
{
    public int numSimulations = 2; // 2 Default

    private string unityAppPath = "not found";

    public void OnClickStart()
    {
        // UnityEngine.Debug.Log("This application is not ready to run a Monte Carlo Simuation");
        // return; // Temp handler (application not ready to run Monte Carlo sim)

        // Previous teams implementation of monte carlo parallel 
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


    // Option 2: Running simultaneously through coroutines
    // public GameObject simulationPrefab; // Prefab of your simulation setup

    // void Start()
    // {
    //     for (int i = 0; i < numSimulations; i++)
    //     {
    //         StartCoroutine(RunSimulation());
    //     }
    // }

    // IEnumerator RunSimulation()
    // {
    //     GameObject simulationObject = Instantiate(simulationPrefab);
    //     SimulationInstance simInstance = simulationObject.GetComponent<SimulationInstance>();

    //     yield return new WaitForSeconds(simInstance.duration);

    //     // Collect results after simulation
    //     Destroy(simulationObject);
    // }


    // Option 2: Using Unity's Job System and Burst Compiler
    public struct SimulationJob : IJobParallelFor
    {
        public void Execute(int index)
        {
            // Implement the simulation logic for a single instance

            // Each index represents a different simulation run

        }
    }

    public class SimulationManager : MonoBehaviour
    {
        public int numSimulations = 100;

        void Start()
        {
            SimulationJob job = new SimulationJob();
            JobHandle handle = job.Schedule(numSimulations, 64);
            handle.Complete();

            // Collect and analyze results
        }
    }

}
