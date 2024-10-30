using UnityEngine;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using TMPro;


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
    public TextMeshProUGUI NumSimsProgressText;
    public bool IsSimulationEnded;

    private WhiskerSim whiskerSim;
    private const int MAX_BATCH_SIZE = 10;

    public void RunMonteCarloSim(WhiskerSim whiskerSim, int simNumber, float duration)
    {
        IsSimulationEnded = false;
        this.whiskerSim = whiskerSim;
        Time.timeScale = 10.0f;
        StartCoroutine(RunSimulationsInBatches(simNumber, duration));
    }


    IEnumerator RunSimulationsInBatches(int beginningSimNumber, float duration)
    {
        int totalSimulations = numSimulations;
        int batchStart = beginningSimNumber;
        int simsRun = 0;
        NumSimsProgressText.text = $"{simsRun} of {numSimulations} Completed";
        while (batchStart < totalSimulations + beginningSimNumber)
        {
            int batchEnd = Mathf.Min(batchStart + MAX_BATCH_SIZE + beginningSimNumber, totalSimulations + beginningSimNumber);
            // Debug.Log($"Running simulations from {batchStart} to {batchEnd - 1}");
            for (int i = batchStart; i < batchEnd; i++)
            {
                simsRun++;
                whiskerSim.RunSim(i, duration, false);
            }
            yield return new WaitUntil(() => whiskerSim.NumberSimsRunning == 0);
            NumSimsProgressText.text = $"{simsRun} of {numSimulations} Completed";
            batchStart = batchEnd;
        }

        StartCoroutine(EndActions(beginningSimNumber));
    }

    IEnumerator EndActions(int beginningSimNumber)
    {
        // Debug.Log("End of monte carlo sim");
        Time.timeScale = 1.0f;
        ResultsProcessor.LogSimStateToMonteCarlo(whiskerSim.SimState, beginningSimNumber, numSimulations);
        ResultsProcessor.LogMonteCarloResults(beginningSimNumber, numSimulations, whiskerSim.SimState.whiskerAmount);
        IsSimulationEnded = true;
        yield return null;
    }
}
