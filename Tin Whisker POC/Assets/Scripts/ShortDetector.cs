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

    // private void Start()
    // {
    //     whiskerCheckCoroutine = StartCoroutine(CheckWhiskersRoutine());
    // }

    private IEnumerator CheckWhiskersRoutine()
    {
        // while (true)
        // {
        //     for (int i = 0; i < whiskers.Count; i++)
        //     {
        //         if (whiskers[i].IsBridgingComponents())
        //         {
        //             // Get the two components the whisker is bridging
        //             GameObject[] components = whiskers[i].GetBridgedComponents();

        //             // Store the components in a normalized order (smallest instance ID first)
        //             (GameObject, GameObject) pair = NormalizePair(components[0], components[1]);
        //             bridgedComponentPairs.Add(pair);
        //         }

        //         // Wait for next frame after checking a few whiskers (you can adjust this number)
        //         if (i % 100 == 0)
        //         {
        //             yield return null;
        //         }
        //     }
        // }
        return null;
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

    private void StopWhiskerChecks()
    {
        if (whiskerCheckCoroutine != null)
        {
            StopCoroutine(whiskerCheckCoroutine);
        }

        // Aggregate and process the results
        AggregateResults();
    }

    private void AggregateResults()
    {
        // Define the path where you want to save the results
        string path = Application.dataPath + "/BridgedComponentsResults.txt";
        Debug.Log("Saving results to: " + path);

        // Use StringBuilder for efficient string manipulations
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var pair in bridgedComponentPairs)
        {
            string line = "Bridged Components: " + pair.Item1.name + " and " + pair.Item2.name;
            stringBuilder.AppendLine(line);
        }

        // Write all lines to the file at once
        File.WriteAllText(path, stringBuilder.ToString());

        Debug.Log("Results saved to: " + path);
    }
}
