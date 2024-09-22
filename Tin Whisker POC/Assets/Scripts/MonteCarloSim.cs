using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;
using System.Collections;
using Unity.Jobs;
using System.Collections.Generic;
using Unity.VisualScripting;

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
    
    public void RunSim(WhiskerSim whiskerSim, ref int simNumber, float duration) {
        Time.timeScale = 10.0f;
        for (int i = 0; i < numSimulations; i++) {
            whiskerSim.RunSim(ref simNumber, duration);
        }
        Time.timeScale = 1.0f;
    }
}
