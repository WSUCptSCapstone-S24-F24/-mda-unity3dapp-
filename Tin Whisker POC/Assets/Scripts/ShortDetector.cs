using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ShortDetector : MonoBehaviour
{
    private Dictionary<int, HashSet<(int, GameObject, GameObject)>> bridgedComponentSets = new Dictionary<int, HashSet<(int, GameObject, GameObject)>>();
    private Coroutine whiskerCheckCoroutine;
    private static int WHISKERS_CHECKED_PER_FRAME = 100;

    public void StartWhiskerChecks(List<WhiskerCollider> whiskerColliders, int simNumber)
    {
        bridgedComponentSets[simNumber] = new HashSet<(int, GameObject, GameObject)>();
        whiskerCheckCoroutine = StartCoroutine(CheckWhiskersRoutine(whiskerColliders, simNumber));
    }

    private IEnumerator CheckWhiskersRoutine(List<WhiskerCollider> whiskerColliders, int simNumber)
    {
        while (true)
        {
            for (int i = 0; i < whiskerColliders.Count; i++)
            {
                if (whiskerColliders[i].IsBridgingComponents())
                {
                    GameObject[] components = whiskerColliders[i].GetBridgedComponents();
                    (int, GameObject, GameObject) set = NormalizeSet(whiskerColliders[i].WhiskerNum, components[0], components[1]);
                    bridgedComponentSets[simNumber].Add(set);
                }

                // TODO: Check if cuases real issues with results only checking first 100
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
        ResultsProcessor.LogBridgedWhiskers(bridgedComponentSets[simNumber], simNumber);
        bridgedComponentSets[simNumber].Clear();
    }
}
