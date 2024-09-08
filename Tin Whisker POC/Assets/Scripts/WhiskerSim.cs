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

        if (WhiskerCount > 2000)
        {
            WhiskerCount = 2000;
            Debug.LogError("Whisker count is too high\nWhisker count: " + WhiskerCount);
        }
        for (int i = 0; i < WhiskerCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-simState.spawnAreaSizeX / 2f * 10f, simState.spawnAreaSizeX / 2f * 10f) + simState.spawnPositionX * 10f - 5f,
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

            // float lengthMultiplier = (float)lognormalRandomLength.NextDouble();
            // float widthMultiplier = (float)lognormalRandomWidth.NextDouble();
            float lengthMultiplier = (float)lognormalRandomLength.Next();
            float widthMultiplier = (float)lognormalRandomWidth.Next();

            newCylinder.transform.localScale = new Vector3(originalScale.x * widthMultiplier, originalScale.y * lengthMultiplier, originalScale.z * widthMultiplier);
            ScaleCylinder(newCylinder, 1f, 0.1f);
            WhiskerCollider whiskerCollider = newCylinder.GetComponent<WhiskerCollider>();
            whiskerCollider.WhiskerNum = i;
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
        // Log the SimState to simstate_log_{simNumber}
        CSVHandler.LogSimState(simState, SimNumber);


        if (simState.simNumber != SimNumber)
        {
            Debug.LogError("Sim number mismatch\nSim number: " + simState.simNumber + "\nMy sim number: " + SimNumber);
        }
    }

    // This method is called to scale the cylinder
    public void ScaleCylinder(GameObject cylinderObject, float widthScale, float heightScale)
    {
        if (cylinderObject == null)
        {
            Debug.LogError("Cylinder object reference is not set.");
            return;
        }

        // Get the current scale of the cylinder object
        Vector3 currentScale = cylinderObject.transform.lossyScale;

        // Calculate the width and height based on the current scale
        float width = currentScale.x * widthScale;
        float height = currentScale.y * heightScale;

        // Calculate the new scale based on the scaled width and height
        Vector3 newScale = new Vector3(
            width, // Width
            height, // Height
            width  // Depth 
        );

        // Apply the new scale to the cylinder object
        cylinderObject.transform.localScale = newScale;
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
        shortDetector.StopWhiskerChecks(SimNumber);
    }

    public void SaveResults(int simNumber)
    {
        simState.SaveSimToJSON(myjsonPath);
        shortDetector.StopWhiskerChecks(simNumber);
    }
}

public class LognormalRandom
{
    private System.Random rand;
    private double mu;
    private double sigma;

    public LognormalRandom(double mu, double sigma, int? seed = null)
    {
        this.mu = mu;
        this.sigma = sigma;
        this.rand = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
    }

    // Generates a normal random variable matching Excel's approach
    public double GenerateNormalRandom()
    {
        // Generate a uniform random number in the range (0, 1)
        double uniformRandom = this.rand.NextDouble();

        // Use the inverse of the normal cumulative distribution function
        // Excel's approach: NORMINV(rand(), mu, sigma)
        double normalRandom = mu + sigma * InverseCumulativeNormal(uniformRandom);
        return normalRandom;
    }

    // Method to generate the next random lognormal value
    public double Next()
    {
        return Math.Exp(GenerateNormalRandom());
    }

    // Approximate the inverse cumulative distribution function for the standard normal
    private static double InverseCumulativeNormal(double p)
    {
        // Approximation for the inverse cumulative normal distribution (quantile function)
        // Algorithm reference: https://en.wikipedia.org/wiki/Inverse_distribution_function

        // Constants for the approximation
        double[] a = { -3.969683028665376e+01, 2.209460984245205e+02, -2.759285104469687e+02, 1.383577518672690e+02, -3.066479806614716e+01, 2.506628277459239e+00 };
        double[] b = { -5.447609879822406e+01, 1.615858368580409e+02, -1.556989798598866e+02, 6.680131188771972e+01, -1.328068155288572e+01 };
        double[] c = { -7.784894002430293e-03, -3.223964580411365e-01, -2.400758277161838e+00, -2.549732539343734e+00, 4.374664141464968e+00, 2.938163982698783e+00 };
        double[] d = { 7.784695709041462e-03, 3.224671290700398e-01, 2.445134137142996e+00, 3.754408661907416e+00 };

        // Define break points
        double p_low = 0.02425;
        double p_high = 1 - p_low;

        double q, r, val;

        if (p < p_low)
        {
            // Rational approximation for lower region
            q = Math.Sqrt(-2 * Math.Log(p));
            val = (((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) /
                  ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1);
        }
        else if (p <= p_high)
        {
            // Rational approximation for central region
            q = p - 0.5;
            r = q * q;
            val = (((((a[0] * r + a[1]) * r + a[2]) * r + a[3]) * r + a[4]) * r + a[5]) * q /
                  (((((b[0] * r + b[1]) * r + b[2]) * r + b[3]) * r + b[4]) * r + 1);
        }
        else
        {
            // Rational approximation for upper region
            q = Math.Sqrt(-2 * Math.Log(1 - p));
            val = -(((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) /
                  ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1);
        }

        return val;
    }
}

