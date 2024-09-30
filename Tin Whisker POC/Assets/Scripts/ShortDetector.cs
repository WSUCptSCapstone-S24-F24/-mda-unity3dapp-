using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ShortDetector : MonoBehaviour
{
    public List<WhiskerCollider> whiskers;  // Assign all whiskers to this list
    public int maxBridgingWhiskers = 10; // Replace with your desired stop condition

    private HashSet<(int, GameObject, GameObject)> bridgedComponentSets = new HashSet<(int, GameObject, GameObject)>();
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
                    (int, GameObject, GameObject) set = NormalizeSet(whiskers[i].WhiskerNum, components[0], components[1]);
                    bridgedComponentSets.Add(set);
                }

                // Wait for next frame after checking a few whiskers (you can adjust this number)
                if (i % 100 == 0)
                {
                    yield return null;
                }
            }
        }
        if (whiskers.Count == 0)
            Debug.LogError("No whiskers assigned to ShortDetector");
    }

    
    private (int, GameObject, GameObject) NormalizeSet(int a, GameObject b, GameObject c)
    {
        if (b.GetInstanceID() < c.GetInstanceID())
        {
            return (a, b, c);
        }
        else
        {
            return (a, c, b);
        }
    }

    public void StopWhiskerChecks(int sim_id)
    {
        if (whiskerCheckCoroutine != null)
        {
            StopCoroutine(whiskerCheckCoroutine);
        }

        // Aggregate and process the results
        CSVHandler.LogBridgedWhiskers(bridgedComponentSets, sim_id);
    }
}
