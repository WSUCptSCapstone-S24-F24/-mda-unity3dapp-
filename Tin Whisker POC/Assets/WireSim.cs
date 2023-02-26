using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WireSim : MonoBehaviour
{
    public GameObject cylinder;
    public int cylinderCount = 10;
    public float spawnAreaSize = 5f;
    public float heightAboveCircuitBoard = 10f;
    public float muIn = 0.5f;
    public float sigmaIn = 0.5f;

    private void Start()
    {
         // Open the CSV file for writing
        StreamWriter writer = new StreamWriter(Application.dataPath + "/../Data/cylinder_lengths.csv");

        // Write the header row
        writer.WriteLine("Cylinder Index,Length");

        for (int i = 0; i < cylinderCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnAreaSize, spawnAreaSize), heightAboveCircuitBoard, Random.Range(-spawnAreaSize, spawnAreaSize));
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject newCylinder = Instantiate(cylinder, spawnPosition, spawnRotation);
            Vector3 originalScale = cylinder.transform.localScale;
            float length = LognormalRandom(muIn, sigmaIn);
            newCylinder.transform.localScale = new Vector3(originalScale.x, length, originalScale.z);

            // Write the cylinder index and length to the CSV file
            writer.WriteLine(i + "," + length);
        }

        // Close the CSV file
        writer.Close();
    }

    public static float LognormalRandom(float mean, float stdDev)
    {
        float mu = Mathf.Log(mean * mean / Mathf.Sqrt(stdDev * stdDev + mean * mean));
        float sigma = Mathf.Sqrt(Mathf.Log(stdDev * stdDev / (mean * mean) + 1));
        float random = Random.Range(0f, 1f);
        float lognormal = Mathf.Exp(mu + sigma * Mathf.Sqrt(-2 * Mathf.Log(random)));
        return lognormal;
    }

}
