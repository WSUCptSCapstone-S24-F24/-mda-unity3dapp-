using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimInfo;

/// <summary>
/// Whisker simulation
/// </summary>
public class WhiskerSim : MonoBehaviour
{
    public ShortDetector shortDetector;
    public SimState simState;
    public GameObject whisker;
    public float simulationDuration;

    private string myjsonPath;
    private int mySimNumber;
    public List<GameObject> whisker_clones = new List<GameObject>();

    /// <summary>
    /// Set up a whisker simulation
    /// </summary>
    private void Start()
    {

    }

    /// <summary>
    /// Runs a whisker falling simulation
    /// </summary>
    public void RunSimulation(int Duration)
    {
        StartSimulation();
        StartCoroutine(EndSimulationAfterDuration(Duration));
    }

    /// <summary>
    /// Performs the logic and directions for starting a simulation (dropping whiskers)
    /// </summary>
    private void StartSimulation()
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




        //print current scene name
        Debug.Log("Current scene is:" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Open the CSV file for writing
        //StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/whisker_lengths.csv");

        //Test the dimensions of the whisker
        // // Write the header row

        Vector3 originalScale = whisker.transform.localScale;
        float WhiskerCount = (simState.spawnAreaSizeX * simState.spawnAreaSizeY * simState.spawnAreaSizeZ) * simState.whiskerDensity;

        LognormalRandom lognormalRandomLength = new LognormalRandom(simState.LengthMu, simState.LengthSigma);
        LognormalRandom lognormalRandomWidth = new LognormalRandom(simState.WidthMu, simState.WidthSigma);

        if (WhiskerCount > 1000)
        {
            WhiskerCount = 1000;
            Debug.LogError("Whisker count is too high\nWhisker count: " + WhiskerCount);
        }
        for (int i = 0; i < WhiskerCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-simState.spawnAreaSizeX / 2f, simState.spawnAreaSizeX / 2f), Random.Range(1, simState.spawnAreaSizeY + 1), Random.Range(-simState.spawnAreaSizeZ / 2f, simState.spawnAreaSizeZ / 2f));
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject newWhisker = Instantiate(whisker, spawnPosition, spawnRotation);
            // Enable Visibility
            newWhisker.GetComponent<MeshRenderer>().enabled = true;

            // Enable Collider component for the clone (if it exists)
            Collider collider = newWhisker.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true; // Enable collisions
            }
            whisker_clones.Add(newWhisker);
            WhiskerCollider whiskerCollider = newWhisker.GetComponent<WhiskerCollider>();
            if (whiskerCollider && shortDetector)
            {
                //Debug.Log("Adding whisker collider to the list, count is now: " + shortDetector.whiskers.Count);
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

            float lengthMultiplier = (float)lognormalRandomLength.NextDouble();
            float widthMultiplier = (float)lognormalRandomWidth.NextDouble();
            newWhisker.transform.localScale = new Vector3(originalScale.x * widthMultiplier, originalScale.y * lengthMultiplier, originalScale.z * widthMultiplier);
        }


        if (simState.simNumber != mySimNumber)
        {
            Debug.LogError("Sim number mismatch\nSim number: " + simState.simNumber + "\nMy sim number: " + mySimNumber);
        }
    }

    private IEnumerator EndSimulationAfterDuration(int Duration)
    {
        if (Duration <= 0)
        {
            Debug.LogError("Simulation duration time out of bounds, default value used (10) \n");
            simulationDuration = 10;
        }
        else
        {
            simulationDuration = Duration;
        }
        yield return new WaitForSeconds(simulationDuration);
        SaveResults();
        ClearWhiskers();
    }

    void ClearWhiskers()
    {
        foreach (GameObject a_whisker in whisker_clones)
        {
            Destroy(a_whisker);
        }
    }

    public void SaveResults()
    {
        simState.SaveSimToJSON(myjsonPath);
        shortDetector.StopWhiskerChecks(mySimNumber);
    }

    public void SaveResults(int simNumber)
    {
        simState.SaveSimToJSON(myjsonPath);
        shortDetector.StopWhiskerChecks(simNumber);
    }

    public void QuitApplication()
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