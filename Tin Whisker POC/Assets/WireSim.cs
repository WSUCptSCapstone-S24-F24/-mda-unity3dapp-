using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimInfo;

public class WireSim : MonoBehaviour
{
    public SimState simState;
    public GameObject cylinder;

    private void Start()
    {
        string jsonPath = Application.persistentDataPath + "/SimState.JSON";  // replace with your desired JSON folder path


        if (System.IO.File.Exists(jsonPath))
        {
            // JSON folder exists, read data from file and initialize SimState object
            string jsonString = System.IO.File.ReadAllText(jsonPath);
            Debug.Log("JSON file exists");
            Debug.Log("JSON path:\n" + jsonPath);
            Debug.Log("JSON string:\n" + jsonString);
            simState = JsonUtility.FromJson<SimState>(jsonString);
        }
        else
        {
            // JSON folder doesn't exist, create SimState object with default constructor
            simState = new SimState();
            Debug.Log("JSON not found\nSaving class to JSON");
            simState.SaveSimToJSON(jsonPath);
        }

        //print current scene name
        Debug.Log("Current scene is:" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Open the CSV file for writing
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/cylinder_lengths.csv");

        // Write the header row
        writer.WriteLine("Cylinder Index,Length,Width");
        Vector3 originalScale = cylinder.transform.localScale;
        for (int i = 0; i < simState.WhiskerCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-simState.spawnAreaSize, simState.spawnAreaSize), simState.heightAboveCircuitBoard, Random.Range(-simState.spawnAreaSize, simState.spawnAreaSize));
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject newCylinder = Instantiate(cylinder, spawnPosition, spawnRotation);

            float lengthMultiplier = LognormalRandom(simState.LengthMu, simState.LengthSigma);
            float widthMultiplier = LognormalRandom(simState.WidthMu, simState.WidthSigma);
            newCylinder.transform.localScale = new Vector3(originalScale.x * widthMultiplier, originalScale.y * lengthMultiplier, originalScale.z * widthMultiplier);

            // Write the cylinder index and length to the CSV file
            writer.WriteLine(i + "," + lengthMultiplier + "," + widthMultiplier);
        }

        // Close the CSV file
        writer.Close();
    }

    public static float LognormalRandom(float mean, float stdDev)
    {
        System.Random rnd = new System.Random();
        float mu = Mathf.Log((mean * mean) / Mathf.Sqrt((stdDev * stdDev) + (mean * mean)));
        float sigma = Mathf.Sqrt(Mathf.Log((stdDev * stdDev) / (mean * mean) + 1));
        float random = (float)rnd.NextDouble();
        float lognormal = Mathf.Exp(mu + sigma * Mathf.Sqrt(-2 * Mathf.Log(random)));
        return lognormal;
    }

}
