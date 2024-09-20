using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;


// Monte Carlo Sim Plan
// Decrease the size scale of the simulation?
// Burst compile for fast code
// Speed up timesteps
// Split whatever is possible into Jobs
// Split whatever possible into threads?


[BurstCompile] 
struct TestSimulationJob : IJob
{
    public NativeArray<float> simulationData;

    public void Execute()
    {
        for (int i = 0; i < simulationData.Length; i++)
        {
            // Test for later
        }
    }
}

public class TestSimulationManager : MonoBehaviour
{
    public int numberOfSimulations = 10;

    void Start()
    {
        RunSimulations();
    }

    void RunSimulations()
    {
        NativeArray<JobHandle> jobHandles = new NativeArray<JobHandle>(numberOfSimulations, Allocator.TempJob);

        for (int i = 0; i < numberOfSimulations; i++)
        {
            NativeArray<float> simulationData = new NativeArray<float>(1000, Allocator.TempJob);

            TestSimulationJob job = new TestSimulationJob
            {
                simulationData = simulationData
            };

            jobHandles[i] = job.Schedule();
        }

        JobHandle.CompleteAll(jobHandles);
        jobHandles.Dispose();

        Debug.Log("All simulations completed.");
    }
}
