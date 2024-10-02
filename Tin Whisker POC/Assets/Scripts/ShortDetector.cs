using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ShortDetector : MonoBehaviour
{
    private HashSet<(int, GameObject, GameObject)> bridgedComponentSets = new HashSet<(int, GameObject, GameObject)>();
    private Coroutine whiskerCheckCoroutine;
    private static int WHISKERS_CHECKED_PER_FRAME = 100;
  
    public void StartWhiskerChecks(List<WhiskerCollider> whiskerColliders)
    {
        whiskerCheckCoroutine = StartCoroutine(CheckWhiskersRoutine(whiskerColliders));
    }

    private IEnumerator CheckWhiskersRoutine(List<WhiskerCollider> whiskerColliders)
    {
        while (true)
        {
            for (int i = 0; i < whiskerColliders.Count; i++)
            {
                if (whiskerColliders[i].IsBridgingComponents())
                {
                    GameObject[] components = whiskerColliders[i].GetBridgedComponents();
                    (int, GameObject, GameObject) set = NormalizeSet(whiskerColliders[i].WhiskerNum, components[0], components[1]);
                    bridgedComponentSets.Add(set);
                }

                // Wait for next frame after checking a few whiskers (you can adjust this number)
                if (i % WHISKERS_CHECKED_PER_FRAME == 0)
                {
                    yield return null;
                }
            }
        }
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

    public void StopWhiskerChecks(int simNumber)
    {
        if (whiskerCheckCoroutine != null)
        {
            StopCoroutine(whiskerCheckCoroutine);
        }

        // Aggregate and process the results
        ResultsProcessor.LogBridgedWhiskers(bridgedComponentSets, simNumber);
    }
}
