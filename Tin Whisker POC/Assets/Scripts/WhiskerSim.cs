using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;
using SimInfo;
using UnityEngine.SceneManagement;

public class WhiskerSim : MonoBehaviour
{
    public ShortDetector shortDetector;
    public SimState simState;
    public GameObject cylinder; // Cylinder/whisker to clone
    public float simulationDuration;
    private string myjsonPath;
    private int SimNumber;
    public List<GameObject> cylinder_clone = new List<GameObject>();

    private void Start()
    {
        SimNumber = GameObject.Find("SceneControl").GetComponent<SceneHandler>().SimNumber;
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-simNumber" && i + 1 < args.Length)
            {
                int.TryParse(args[i + 1], out int parsedSimNumber);
                SimNumber = parsedSimNumber;
                break;
            }
        }

        if (SimNumber == -1) // Set sim number to 0 if no simNumber argument found
                SimNumber = 0;
        Debug.Log("Sim number: " + SimNumber);


        myjsonPath = Application.persistentDataPath + "/SimState.JSON";  


        if (System.IO.File.Exists(myjsonPath))
        {
            // JSON folder exists, read data from file and initialize SimState object
            string jsonString = System.IO.File.ReadAllText(myjsonPath);
            Debug.Log("JSON file exists");
            Debug.Log("JSON path:\n" + myjsonPath);
            Debug.Log("JSON string:\n" + jsonString);
            simState = JsonUtility.FromJson<SimState>(jsonString);
            simState.simNumber = SimNumber;
        }
        else
        {
            // JSON folder doesn't exist, create SimState object with default constructor
            simState = new SimState();
            Debug.Log("JSON not found\nSaving class to JSON");
            simState.SaveSimToJSON(myjsonPath);
            simState.simNumber = SimNumber;
        }

        //print current scene name
        Debug.Log("Current scene is:" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        //Test the dimensions of the cylinder
        // // Write the header row

        Vector3 originalScale = cylinder.transform.localScale;
        float WhiskerCount = (simState.spawnAreaSizeX * simState.spawnAreaSizeY * simState.spawnAreaSizeZ) * simState.whiskerDensity;

        LognormalRandom lognormalRandomLength = new LognormalRandom(simState.LengthMu, simState.LengthSigma);
        LognormalRandom lognormalRandomWidth = new LognormalRandom(simState.WidthMu, simState.WidthSigma);

        if (WhiskerCount > 5000)
        {
            WhiskerCount = 5000;
            Debug.LogError("Whisker count is too high\nWhisker count: " + WhiskerCount);
        }
        for (int i = 0; i < WhiskerCount; i++)
        {   
            Vector3 spawnPosition = new Vector3(Random.Range(-simState.spawnAreaSizeX / 2f * 10f, simState.spawnAreaSizeX / 2f  * 10f) + simState.spawnPositionX * 10f - 5f, 
                                                Random.Range(-simState.spawnAreaSizeY / 2f * 10f, simState.spawnAreaSizeY / 2f * 10f) + simState.spawnPositionY * 10f + simState.spawnAreaSizeY * 10f / 2, 
                                                Random.Range(-simState.spawnAreaSizeZ / 2f * 10f, simState.spawnAreaSizeZ / 2f * 10f) + simState.spawnPositionZ * 10f - 5f);
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject newCylinder = Instantiate(cylinder, spawnPosition, spawnRotation);
            newCylinder.name = $"Whisker{i}";
            // Make cylinder/whisker visable
            newCylinder.GetComponent<MeshRenderer>().enabled = true;
            // Enable cylinder/whisker collisions
            Collider collider = newCylinder.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = true; // Enable collisions

            cylinder_clone.Add(newCylinder);

            float lengthMultiplier = (float)lognormalRandomLength.NextDouble();
            float widthMultiplier = (float)lognormalRandomWidth.NextDouble();
            newCylinder.transform.localScale = new Vector3(originalScale.x * widthMultiplier, originalScale.y * lengthMultiplier, originalScale.z * widthMultiplier);

            WhiskerCollider whiskerCollider = newCylinder.GetComponent<WhiskerCollider>();
            if (whiskerCollider && shortDetector)
            {
                shortDetector.whiskers.Add(whiskerCollider);
            }
            else
            {
                Debug.LogError("Whisker collider or short detector not found");
                if (!shortDetector)
                {
                    Debug.LogError("Short detector not found");
                }
            }
        }

        // Log all whiskers to whisker_log_{simNumber}
        CSVHandler.LogWhiskers(cylinder_clone, SimNumber);


        if (simState.simNumber != SimNumber)
        {
            Debug.LogError("Sim number mismatch\nSim number: " + simState.simNumber + "\nMy sim number: " + SimNumber);
        }
    }


    public void ClearCylinders()
    {
        foreach (GameObject cylinder in cylinder_clone)
        {
            Destroy(cylinder);
        }
    }

    public void SaveResults()
    {
        simState.SaveSimToJSON(myjsonPath);
        // simState.SaveToCSV(mySimNumber);
        shortDetector.StopWhiskerChecks(SimNumber);
    }

    public void SaveResults(int simNumber)
    {
        simState.SaveSimToJSON(myjsonPath);
        // simState.SaveToCSV(simNumber);
        shortDetector.StopWhiskerChecks(simNumber);
    }
}

public class LognormalRandom
{
    private readonly System.Random rand;
    private readonly double mu;
    private readonly double sigma;

    public LognormalRandom(double mu, double sigma, int? seed = null)
    {
        this.mu = mu;
        this.sigma = sigma;
        rand = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
    }

    public double NextDouble()
    {
        // Generate a random number from a normal distribution
        double u1 = rand.NextDouble();
        double u2 = rand.NextDouble();
        double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2);

        // Convert the standard normal number to a lognormal number
        double randLogNormal = System.Math.Exp(mu + sigma * randStdNormal);

        return randLogNormal;
    }
}