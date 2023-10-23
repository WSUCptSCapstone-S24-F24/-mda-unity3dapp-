using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimInfo;

public class WireSim : MonoBehaviour
{
    public ShortDetector shortDetector;
    public SimState simState;
    public GameObject cylinder;
    public float simulationDuration;
    //list of all the cylinders
    private string myjsonPath;
    private int mySimNumber;
    public List<GameObject> cylinder_clone = new List<GameObject>();

    private void Start()
    {
        mySimNumber = -1;
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-simNumber" && i + 1 < args.Length)
            {
                int.TryParse(args[i + 1], out int parsedSimNumber);
                mySimNumber = parsedSimNumber;
                break;
            }
        }
        
        if (mySimNumber == -1)
        {
            Debug.LogError("Sim number not found");
            mySimNumber = 0;
        }
        else
        {
            Debug.Log("Sim number: " + mySimNumber);
        }


        myjsonPath = Application.persistentDataPath + "/SimState" + (mySimNumber >= 1 ? mySimNumber : "") + ".JSON";  // replace with your desired JSON folder path


        if (System.IO.File.Exists(myjsonPath))
        {
            // JSON folder exists, read data from file and initialize SimState object
            string jsonString = System.IO.File.ReadAllText(myjsonPath);
            Debug.Log("JSON file exists");
            Debug.Log("JSON path:\n" + myjsonPath);
            Debug.Log("JSON string:\n" + jsonString);
            simState = JsonUtility.FromJson<SimState>(jsonString);
        }
        else
        {
            // JSON folder doesn't exist, create SimState object with default constructor
            simState = new SimState();
            Debug.Log("JSON not found\nSaving class to JSON");
            simState.SaveSimToJSON(myjsonPath);
        }


        // Get the game object that the user loaded and attach it to the cylinder colorchanger script
        // cylinder.GetComponent<ColorChanger>().object1 = simState.Model;

        //print current scene name
        Debug.Log("Current scene is:" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Open the CSV file for writing
        //StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/cylinder_lengths.csv");

        //Test the dimensions of the cylinder
        // // Write the header row
        // writer.WriteLine("Cylinder Index,Length,Width");
        Vector3 originalScale = cylinder.transform.localScale;
        for (int i = 0; i < simState.WhiskerCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-simState.spawnAreaSize, simState.spawnAreaSize), simState.heightAboveCircuitBoard, Random.Range(-simState.spawnAreaSize, simState.spawnAreaSize));
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject newCylinder = Instantiate(cylinder, spawnPosition, spawnRotation);
            cylinder_clone.Add(newCylinder);
            // WhiskerCollider whiskerCollider = newCylinder.GetComponent<WhiskerCollider>();
            //     if (whiskerCollider && shortDetector) 
            //     {
            //         shortDetector.whiskers.Add(whiskerCollider);
            //     }

            float lengthMultiplier = LognormalRandom(simState.LengthMu, simState.LengthSigma);
            float widthMultiplier = LognormalRandom(simState.WidthMu, simState.WidthSigma);
            newCylinder.transform.localScale = new Vector3(originalScale.x * widthMultiplier, originalScale.y * lengthMultiplier, originalScale.z * widthMultiplier);

            // Write the cylinder index and length to the CSV file
            //writer.WriteLine(i + "," + lengthMultiplier + "," + widthMultiplier);
        }

        // // Close the CSV file
        // writer.Close();
        if (simState.simNumber != mySimNumber)
        {
            Debug.LogError("Sim number mismatch\nSim number: " + simState.simNumber + "\nMy sim number: " + mySimNumber);
        }
        
        Debug.Log("Sim number: " + simState.simNumber + "\nMy sim number: " + mySimNumber + "\n trying to end simulation");
        if (simState.simNumber >= 1 || mySimNumber >= 1){
            Debug.Log("Ending simulation and saving results");
            StartCoroutine(EndSimulationAfterDuration());
        } 
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

     IEnumerator EndSimulationAfterDuration()
    {
        simulationDuration = 10;
        yield return new WaitForSeconds(simulationDuration);
        SaveResults();
        QuitApplication();
    }

    void ClearCylinders()
    {
        foreach (GameObject cylinder in cylinder_clone)
        {
            Destroy(cylinder);
        }
    }

    void SaveResults()
    {   
        simState.SaveSimToJSON(myjsonPath);
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/Sim_Output_" + simState.simNumber + ".txt");
        int pinsBridged = 0;
        pinsBridged = Random.Range(0, 5);
        writer.WriteLine("Pins Briged: " + pinsBridged);

        writer.Close();
    }

    void QuitApplication()
    {
        Debug.Log("Quitting application");
        Debug.Log("Sim number: " + simState.simNumber);
        // Quit the application
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        Application.Quit();
    }

}
