using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ShortDetector : MonoBehaviour
{
    public List<WhiskerCollider> whiskers;  // Assign all whiskers to this list
    public int maxBridgingWhiskers = 10; // Replace with your desired stop condition

    private HashSet<(GameObject, GameObject)> bridgedComponentPairs = new HashSet<(GameObject, GameObject)>();
    private Coroutine whiskerCheckCoroutine;
  
    private void Start()
    {
        whiskerCheckCoroutine = StartCoroutine(CheckWhiskersRoutine());
    }

    private IEnumerator CheckWhiskersRoutine()
    {
        while (true)
        {
            if (whiskers.Count == 0)
            {
                Debug.LogError("No whiskers assigned to ShortDetector");
                yield return new WaitForSeconds(1.0f);
                continue; // Continue the while loop if no whiskers are found
            }

            for (int i = 0; i < whiskers.Count; i++)
            {
                if (whiskers[i].IsBridgingComponents())
                {
                    // Get the two components the whisker is bridging
                    GameObject[] components = whiskers[i].GetBridgedComponents();

                    // Store the components in a normalized order (smallest instance ID first)
                    (GameObject, GameObject) pair = NormalizePair(components[0], components[1]);
                    bridgedComponentPairs.Add(pair);
                }

                // Wait for next frame after checking a few whiskers (you can adjust this number)
                if (i % 100 == 0)
                {
                    yield return null;
                }
            }
        }
        
    }

    
    private (GameObject, GameObject) NormalizePair(GameObject a, GameObject b)
    {
        if (a.GetInstanceID() < b.GetInstanceID())
        {
            return (a, b);
        }
        else
        {
            return (b, a);
        }
    }

    public void StopWhiskerChecks(int sim_id)
    {
        if (whiskerCheckCoroutine != null)
        {
            StopCoroutine(whiskerCheckCoroutine);
        }

        // Aggregate and process the results
        AggregateResults(sim_id);
    }

    private void AggregateResults(int sim_id)
    {
        // Define the path where you want to save the results
        string path = Application.dataPath + "/BridgedComponentsResults/sim_" + sim_id + "_bridged_components.txt";
        Debug.Log("Saving results to: " + path);

        // Use StringBuilder for efficient string manipulations
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Bridged Component Pairs:");

        // Write the number of bridged component pairs
        foreach (var pair in bridgedComponentPairs)
        {
            string line = "(" + pair.Item1.name + "," + pair.Item2.name + ")";
            stringBuilder.AppendLine(line);
        }

        // Create the directory if it doesn't exist
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        // Write all lines to the file
        File.WriteAllText(path, stringBuilder.ToString());

        Debug.Log("Results saved to: " + path);
    }
}
