using UnityEngine;
using System.Collections;


// ****   Monte Carlo Sim Ideas   ****
// Decrease the size scale of the simulation
// Burst compile for fast code
// Speed up time (practical max of 10)
// Split whatever is possible into Jobs
// Split whatever possible into threads
// Physics layers

public class MonteCarloSim : MonoBehaviour
{
    public int numSimulations = 2; // 2 Default
    public bool IsSimulationEnded;

    private string[] layerNames;
    private WhiskerSim whiskerSim;
    private int maxBatchSize = 10;

    public void RunMonteCarloSim(WhiskerSim whiskerSim, ref int simNumber, float duration) {
        IsSimulationEnded = false;
        this.whiskerSim = whiskerSim;
        MakeLayerNames();
        Time.timeScale = 10.0f;
        StartCoroutine(RunSimulationsInBatches(simNumber, duration));
    }


    IEnumerator RunSimulationsInBatches(int simNumber, float duration) {
        int totalSimulations = numSimulations;
        int batchStart = 0;

        while (batchStart < totalSimulations) {
            int batchEnd = Mathf.Min(batchStart + maxBatchSize, totalSimulations);
            Debug.Log($"Running simulations from {batchStart} to {batchEnd - 1}");

            for (int i = batchStart; i < batchEnd; i++) {
                this.whiskerSim.RunSim(ref simNumber, duration, layerNames[i], false);
            }

            yield return new WaitUntil(() => whiskerSim.NumberSimsRunning == 0);

            batchStart = batchEnd;
        }

        StartCoroutine(EndActions());
    }

    IEnumerator EndActions() {
        Debug.Log("End of monte carlo sim");
        Time.timeScale = 1.0f;
        IsSimulationEnded = true;
        yield return null;
    }    
    
    private void MakeLayerNames() {
        layerNames = new string[numSimulations];
        int[] possibleLayerNums = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        for(int i = 0; i < numSimulations; i++) {
            layerNames[i] = $"Sim layer {possibleLayerNums[i % possibleLayerNums.Length]}";
        }
    }
}
