using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;
using SimInfo;
using TMPro;

public class WhiskerSim : MonoBehaviour
{

    public SimState SimState;
    public GameObject Whisker; // Cylinder/Whisker to clone
    public int NumberSimsRunning;
    public ShortDetector ShortDetector;

    private string myjsonPath;
    private List<GameObject> whiskers = new List<GameObject>();
    private float duration;
    private Coroutine simulationCoroutine;
    private bool render;

    

    public void RunSim(int simNumber, float duration, bool render = true)
    {
        string layerName = $"Sim layer {simNumber % 10 + 1}";  // For 10 physics layers
        NumberSimsRunning++;
        this.duration = duration;
        this.render = render;
        SimStateSetUp(simNumber);
        List<WhiskerCollider> whiskerColliders = SpawnWhiskers(layerName);


        // Log all whiskers to whisker_log_{simNumber}
        ResultsProcessor.LogWhiskers(GetSimLayerWhiskers(whiskers, layerName), simNumber);
        // Log the SimState to other results files
        ResultsProcessor.LogSimState(SimState, simNumber);
        simulationCoroutine = StartCoroutine(EndSimulationAfterDuration(simNumber));
        ShortDetector.GetComponent<ShortDetector>().StartWhiskerChecks(whiskerColliders);
    }

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

    public void ClearLayerWhiskers(string layerNameToDelete)
    {
        int layerNum = LayerMask.NameToLayer(layerNameToDelete);
        foreach (GameObject whisker in whiskers)
        {
            if (whisker.layer == layerNum)
            {
                DestroyImmediate(whisker);
            }
        }
        
        whiskers.RemoveAll(whisker => whisker == null);
    }

    public void SaveResults(int simNumber)
    {
        Debug.Log($"Saving sim number: {simNumber}");
        SimState.SaveSimToJSON(myjsonPath);
        ShortDetector.StopWhiskerChecks(simNumber);
    }

    private void SimStateSetUp(int simNumber)
    {
        Debug.Log("Sim number: " + simNumber);

        myjsonPath = Application.persistentDataPath + "/SimState.JSON";
        if (File.Exists(myjsonPath))
        {
            // JSON folder exists, read data from file and initialize SimState object
            string jsonString = File.ReadAllText(myjsonPath);
            SimState = JsonUtility.FromJson<SimState>(jsonString);
            SimState.simNumber = simNumber;
        }
        else
        {
            // JSON folder doesn't exist, create SimState object with default constructor
            SimState = new SimState();
            SimState.SaveSimToJSON(myjsonPath);
            SimState.simNumber = simNumber;
        }
    }

    private List<WhiskerCollider> SpawnWhiskers(string layerName)
    {
        List<WhiskerCollider> whiskerColliders = new List<WhiskerCollider>();
        Whisker.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 originalScale = Whisker.transform.localScale;
        // Defualt is scale: (1, 1, 1) which makes a length of 2 or 1/5 mm and a diameter of 1 or 1/10 mm
        // (1, 5, 1) is 1 mm long --> (1, 0.005, 1) is 1 micron long
        // (10, 1, 10) is 1 mm diameter --> (0.01, 1, 0.01) is 1 micron diameter 
        Vector3 scaledTransform = new Vector3(originalScale.x * 10.0f / 1000.0f, originalScale.y * 5.0f / 1000.0f, originalScale.z * 10.0f / 1000.0f);
        Whisker.transform.localScale = scaledTransform;

        float WhiskerCount = SimState.spawnAreaSizeX * SimState.spawnAreaSizeY * SimState.spawnAreaSizeZ * SimState.whiskerDensity;
        LognormalRandom lognormalRandomLength = new LognormalRandom(SimState.LengthMu, SimState.LengthSigma);
        LognormalRandom lognormalRandomWidth = new LognormalRandom(SimState.WidthMu, SimState.WidthSigma);

        if (WhiskerCount > 1000)
        {
            WhiskerCount = 1000;
            Debug.LogError("Whisker count is too high\nWhisker count: " + WhiskerCount);
        }
        for (int i = 0; i < WhiskerCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-SimState.spawnAreaSizeX / 2f * 10f, SimState.spawnAreaSizeX / 2f * 10f) + SimState.spawnPositionX * 10f - 5f,
                                                Random.Range(-SimState.spawnAreaSizeY / 2f * 10f, SimState.spawnAreaSizeY / 2f * 10f) + SimState.spawnPositionY * 10f + SimState.spawnAreaSizeY * 10f / 2,
                                                Random.Range(-SimState.spawnAreaSizeZ / 2f * 10f, SimState.spawnAreaSizeZ / 2f * 10f) + SimState.spawnPositionZ * 10f - 5f);
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject newWhisker = Instantiate(Whisker, spawnPosition, spawnRotation);
            int layer = LayerMask.NameToLayer(layerName);
            newWhisker.layer = layer;
            newWhisker.name = $"Whisker{i}";
            // Make Whisker visable
            newWhisker.GetComponent<MeshRenderer>().enabled = render;
            // Enable Whisker collisions
            Collider collider = newWhisker.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = true; // Enable collisions

            whiskers.Add(newWhisker);

            float lengthMultiplier = (float)lognormalRandomLength.Next();
            float widthMultiplier = (float)lognormalRandomWidth.Next();

            ScaleCylinder(newWhisker, widthMultiplier, lengthMultiplier);
            WhiskerCollider whiskerCollider = newWhisker.GetComponent<WhiskerCollider>();
            whiskerCollider.WhiskerNum = i;
            whiskerColliders.Add(whiskerCollider);
        }
        return whiskerColliders;
    }

    IEnumerator EndSimulationAfterDuration(int simNumber)
    {
        // Check if simState and its duration are set, otherwise use a default value
        float simulationDuration = duration >= 0.1 ? duration : 10f;

        // Wait for the specified simulation duration
        yield return new WaitForSeconds(simulationDuration);

        SaveResults(simNumber);
        ClearLayerWhiskers($"Sim layer {simNumber % 10 + 1}");
        yield return null;

        // Proceed to call cleanup for all WhiskerCollider instances
        foreach (WhiskerCollider whiskerCollider in FindObjectsOfType<WhiskerCollider>())
            whiskerCollider.Cleanup();
        NumberSimsRunning--;
    }

    public void EndSimulationEarly(int simNumber)
    {
        // Stop the coroutine that is waiting for the simulation to end
        StopCoroutine(simulationCoroutine);
        SaveResults(simNumber);
        ClearLayerWhiskers($"Sim layer {simNumber % 10 + 1}");
        NumberSimsRunning--;
    }

    private List<GameObject> GetSimLayerWhiskers(List<GameObject> allWhiskers, string layerName) {
        List<GameObject> resultWhiskers = new List<GameObject>();
        int layerNum = LayerMask.NameToLayer(layerName);
        foreach(GameObject whisker in allWhiskers) {
            if (whisker.layer == layerNum)
            {
                resultWhiskers.Add(whisker);
            }
        }
        return resultWhiskers;
    }
}
