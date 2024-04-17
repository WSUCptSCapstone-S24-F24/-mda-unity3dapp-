using System;
using System.IO;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using SimInfo;
public class CSVHandler : MonoBehaviour
{
    public TextMeshProUGUI csvText;
    public int padding = 2; // Padding between columns
    private static bool cleared = false;

    private static void ClearSimulationResultsDirectory()
    {
        string directoryPath = Path.Combine(Application.dataPath, "..", "SimulationResults");

        try
        {
            // Check if the directory exists
            if (Directory.Exists(directoryPath))
            {
                // Delete all files in the directory
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                // Delete all subdirectories
                foreach (DirectoryInfo subDirectory in directoryInfo.GetDirectories())
                {
                    subDirectory.Delete(true);
                }

                Debug.Log($"Successfully cleared SimulationResults directory at: {directoryPath}");
            }
            else
            {
                Debug.LogWarning($"SimulationResults directory not found at: {directoryPath}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to clear SimulationResults directory: {ex.Message}");
        }
    }

    public static void LogWhiskers(List<GameObject> whiskers, int simNumber)
    {
        // Clear directory if not cleared
        if (!cleared)
        {
            ClearSimulationResultsDirectory();
            cleared = true;
        }

        // Creating the file path
        string directoryPath = Path.Combine(Application.dataPath, "..", "SimulationResults"); // Make sure note monte carlo sim
        string fileName = $"whiskers_log_{simNumber}.csv";
        string fullPath = Path.Combine(directoryPath, fileName);

        try
        {
            // Ensure the directory exists
            Directory.CreateDirectory(directoryPath);

            // Prepare to write to the file
            using (StreamWriter writer = new StreamWriter(fullPath, false))
            {
                // Write headers or any initial data 
                writer.WriteLine("GameObjectName,PositionX,PositionY,PositionZ,Length,Radius");

                // Loop through each whisker and write its properties
                foreach (GameObject whisker in whiskers)
                {
                    Vector3 pos = whisker.transform.position;
                    Vector3 localScale = whisker.transform.localScale;

                    // Calculate the length using the magnitude of the local scale vector
                    float length = localScale.magnitude;

                    // Get the rotation of the whisker
                    Quaternion rotation = whisker.transform.rotation;

                    // Rotate the local Y axis to match the whisker's rotation
                    Vector3 localYAxis = rotation * Vector3.up;

                    // Calculate the diameter as the component of the local scale that aligns with the direction of the local Y axis after rotation
                    float diameter = Vector3.Dot(localScale, localYAxis.normalized);

                    // Calculate the radius by dividing the diameter by 2
                    float radius = Mathf.Abs(diameter) / 2f; // Ensure radius is non-negative

                    writer.WriteLine($"{whisker.name},{pos.x},{pos.y},{pos.z},{length},{radius}");
                }
            }

            Debug.Log($"Successfully wrote to {fullPath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to write to {fullPath}: {ex.Message}");
        }
    }

    public static void LogSimState(SimState simState, int simNumber)
    {
        // Clear directory if not cleared
        if (!cleared)
        {
            ClearSimulationResultsDirectory();
            cleared = true;
        }

        // Creating the file path
        string directoryPath = Path.Combine(Application.dataPath, "..", "SimulationResults");
        string fileName = $"simstate_log_{simNumber}.csv";
        string fullPath = Path.Combine(directoryPath, fileName);

        try
        {
            // Ensure the directory exists
            Directory.CreateDirectory(directoryPath);

            // Prepare to write to the file
            using (StreamWriter writer = new StreamWriter(fullPath, false))
            {
                // Write headers
                writer.WriteLine("WhiskerDensity,SpawnAreaSizeX,SpawnAreaSizeY,SpawnAreaSizeZ,SpawnPositionX,SpawnPositionY,SpawnPositionZ,LengthMu,LengthSigma,WidthMu,WidthSigma,SimNumber,SimDuration");

                // Write values of each property
                writer.WriteLine($"{simState.whiskerDensity},{simState.spawnAreaSizeX},{simState.spawnAreaSizeY},{simState.spawnAreaSizeZ},{simState.spawnPositionX},{simState.spawnPositionY},{simState.spawnPositionZ},{simState.LengthMu},{simState.LengthSigma},{simState.WidthMu},{simState.WidthSigma},{simState.simNumber},{simState.simDuration}");
            }

            Debug.Log($"Successfully wrote sim state to {fullPath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to write sim state to {fullPath}: {ex.Message}");
        }
    }

    public static void LogBridgedWhiskers(HashSet<(int, GameObject, GameObject)> bridgedComponentSets, int simNumber)
    {
        // Clear directory if not cleared
        if (!cleared)
        {
            ClearSimulationResultsDirectory();
            cleared = true;
        }

        // Define the path where you want to save the results
        string directoryPath = Path.Combine(Application.dataPath, "..", "SimulationResults");
        string fileName = $"bridgedcomponents_log_{simNumber}.csv";
        string fullPath = Path.Combine(directoryPath, fileName);

        try
        {
            // Ensure the directory exists
            Directory.CreateDirectory(directoryPath);

            // Prepare to write to the file
            using (StreamWriter writer = new StreamWriter(fullPath, false))
            {
                // Write headers or any initial data 
                writer.WriteLine("Whisker,Component1,Component2");

                // Write the number of bridged component pairs
                foreach (var set in bridgedComponentSets)
                {
                    writer.WriteLine($"{set.Item1},{set.Item2.name},{set.Item3.name}");
                }
            }

            Debug.Log($"Successfully wrote to {fullPath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to write to {fullPath}: {ex.Message}");
        }
    }

    // Function to read and display CSV file given its path
    public void ShowCSVFile(string csvFileName)
    {
        // Construct the full path to the CSV file using Application.dataPath
        string csvFilePath = Path.Combine(Application.dataPath, "..", "SimulationResults", csvFileName);

        if (!File.Exists(csvFilePath))
        {
            Debug.LogError($"File not found: {csvFilePath}");
            PopupManagerSingleton.Instance.ShowPopup("No simulation results found");
            return;
        }

        // Clear previous content
        csvText.text = "<mspace=0.56em>";

        string[] lines = File.ReadAllLines(csvFilePath);

        // Get the maximum width of each column
        int[] columnWidths = GetColumnWidths(lines);

        // Start building the table-like layout
        foreach (string line in lines)
        {
            string[] fields = line.Split(','); // Split the line into fields using comma as delimiter

            // Add fields for each column
            for (int i = 0; i < fields.Length; i++)
            {
                // Calculate the number of spaces needed for alignment based on the maximum width of the column and the length of the current field
                int numSpaces = columnWidths[i] - fields[i].Length + padding;
                string spaces = new string(' ', numSpaces);

                // Add field and spaces for alignment
                csvText.text += $" {fields[i]}{spaces}";
            }

            csvText.text += "\n"; // New line for the next row
        }
    }

    // Function to calculate the maximum width of each column
    private int[] GetColumnWidths(string[] lines)
    {
        int numColumns = lines[0].Split(',').Length;
        int[] columnWidths = new int[numColumns];

        foreach (string line in lines)
        {
            string[] fields = line.Split(',');
            for (int i = 0; i < numColumns; i++)
            {
                if (i < fields.Length)
                {
                    columnWidths[i] = Math.Max(columnWidths[i], fields[i].Trim().Length);
                }
            }
        }

        // Add padding
        for (int i = 0; i < columnWidths.Length; i++)
        {
            columnWidths[i] += padding;
        }

        return columnWidths;
    }
}
