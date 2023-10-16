using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskerCollider : MonoBehaviour
{
    private HashSet<GameObject> currentlyCollidingObjects = new HashSet<GameObject>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Part"))
        {
            currentlyCollidingObjects.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Part"))
        {
            currentlyCollidingObjects.Remove(collision.gameObject);
        }
    }

    public bool IsBridgingComponents()
    {
        return currentlyCollidingObjects.Count > 1;
    }

    public GameObject[] GetBridgedComponents()
    {
        //HashSet to List
        List<GameObject> bridgedComponents = new List<GameObject>(currentlyCollidingObjects);
        //List to Array
        return bridgedComponents.ToArray();
    }
}


