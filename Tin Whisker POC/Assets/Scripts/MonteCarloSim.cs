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

    public void RunMonteCarloSim(WhiskerSim whiskerSim, ref int simNumber, float duration) {
        IsSimulationEnded = false;
        this.whiskerSim = whiskerSim;
        MakeLayerNames();
        Time.timeScale = 10.0f;
        for (int i = 0; i < numSimulations; i++) {
            this.whiskerSim.RunSim(ref simNumber, duration, layerNames[i], false);
        }
        StartCoroutine(EndActions());
    }

    IEnumerator EndActions() {
        yield return new WaitUntil(() => whiskerSim.NumberSimsRunning == 0);
        Debug.Log("End of monte carlo sim");
        Time.timeScale = 1.0f;
        IsSimulationEnded = true;
    }

    private void MakeLayerNames() {
        layerNames = new string[numSimulations];
        int[] possibleLayerNums = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        for(int i = 0; i < numSimulations; i++) {
            layerNames[i] = $"Sim layer {possibleLayerNums[i % possibleLayerNums.Length]}";
        }
    }
}
