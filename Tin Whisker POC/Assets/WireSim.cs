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
    public float LengthmuIn = 0.5f;
    public float LengthsigmaIn = 0.5f;
    public float WidthmuIn = 0.5f;
    public float WidthsigmaIn = 0.5f;


    private void Start()
    {

        //print current scene name
        Debug.Log("Current scene is:" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Open the CSV file for writing
        StreamWriter writer = new StreamWriter(Application.dataPath + "/../Data/cylinder_lengths.csv");

        // Write the header row
        writer.WriteLine("Cylinder Index,Length,Width");
        Vector3 originalScale = cylinder.transform.localScale;
        for (int i = 0; i < cylinderCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnAreaSize, spawnAreaSize), heightAboveCircuitBoard, Random.Range(-spawnAreaSize, spawnAreaSize));
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject newCylinder = Instantiate(cylinder, spawnPosition, spawnRotation);

            float lengthMultiplier = LognormalRandom(LengthmuIn, LengthsigmaIn);
            float widthMultiplier = LognormalRandom(WidthmuIn, WidthsigmaIn);
            newCylinder.transform.localScale = new Vector3(originalScale.x * widthMultiplier, originalScale.y * lengthMultiplier, originalScale.z * widthMultiplier);

            // Write the cylinder index and length to the CSV file
            writer.WriteLine(i + "," + lengthMultiplier + "," + widthMultiplier);
        }

        // Close the CSV file
        writer.Close();
    }

    public static float LognormalRandom(float mean, float stdDev)
    {
        Random rnd = new Random();
        float mu = Mathf.Log((mean * mean) / Mathf.Sqrt((stdDev * stdDev) + (mean * mean)));
        float sigma = Mathf.Sqrt(Mathf.Log((stdDev * stdDev) / (mean * mean) + 1));
        float random = rnd.NextSingle();
        float lognormal = Mathf.Exp(mu + sigma * Mathf.Sqrt(-2 * Mathf.Log(random)));
        return lognormal;
    }

}
