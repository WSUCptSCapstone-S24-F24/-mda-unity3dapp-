using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSim : MonoBehaviour
{
    public GameObject cylinder;
    public int cylinderCount = 10;
    public float spawnAreaSize = 5f;
    public float heightAboveCircuitBoard = 10f;
    public float minLength = 0.5f;
    public float maxLength = 2f;

    private void Start()
    {
        for (int i = 0; i < cylinderCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnAreaSize, spawnAreaSize), heightAboveCircuitBoard, Random.Range(-spawnAreaSize, spawnAreaSize));
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject newCylinder = Instantiate(cylinder, spawnPosition, spawnRotation);
            Vector3 originalScale = cylinder.transform.localScale;
            newCylinder.transform.localScale = new Vector3(originalScale.x, Random.Range(minLength, maxLength), originalScale.z);
        }
    }
}
