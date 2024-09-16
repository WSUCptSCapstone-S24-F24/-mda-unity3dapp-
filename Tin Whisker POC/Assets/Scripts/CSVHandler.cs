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
                writer.WriteLine("GameObjectName,PositionX (mm),PositionY (mm),PositionZ (mm),Length (µm),Diameter (µm)");

                // Loop through each whisker and write its properties
                foreach (GameObject whisker in whiskers)
                {
                    Vector3 pos = whisker.transform.position;
                    Vector3 localScale = whisker.transform.localScale;
                    float length = localScale.y; // Height is along the y-axis
                    float diameter = Mathf.Max(localScale.x, localScale.z); // Diameter is the larger of x and z

                    // Write the properties to the file
                    writer.WriteLine($"{whisker.name}, {Math.Round(pos.x / 10f, 1)}, {Math.Round(pos.y / 10f, 1)}, {Math.Round(pos.z / 10f, 1)}, {Math.Round(length * 2 * 100f, 2)}, {Math.Round(diameter * 100f, 2)}");
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
        // Creating the file paths for whiskers and bridged components logs
        string whiskersLogPath = Path.Combine(Application.dataPath, "..", "SimulationResults", $"whiskers_log_{simNumber}.csv");
        string bridgedLogPath = Path.Combine(Application.dataPath, "..", "SimulationResults", $"bridgedcomponents_log_{simNumber}.csv");
        string simStateLogPath = Path.Combine(Application.dataPath, "..", "SimulationResults", $"simstate_log_{simNumber}.csv");

        try
        {
            // Ensure the directories exist
            Directory.CreateDirectory(Path.GetDirectoryName(whiskersLogPath));
            Directory.CreateDirectory(Path.GetDirectoryName(bridgedLogPath));
            Directory.CreateDirectory(Path.GetDirectoryName(simStateLogPath));

            // Prepare new data to be written
            string newData = $"WhiskerDensity,SpawnAreaSizeX (mm),SpawnAreaSizeY (mm),SpawnAreaSizeZ (mm),SpawnPositionX (mm),SpawnPositionY (mm),SpawnPositionZ (mm),LengthMu,LengthSigma,WidthMu,WidthSigma,SimNumber,SimDuration (sec)\n{simState.whiskerDensity},{simState.spawnAreaSizeX},{simState.spawnAreaSizeY},{simState.spawnAreaSizeZ},{simState.spawnPositionX},{simState.spawnPositionY},{simState.spawnPositionZ},{simState.LengthMu},{simState.LengthSigma},{simState.WidthMu},{simState.WidthSigma},{simState.simNumber},{simState.simDuration}\n";

            // Read existing content of whiskers log file
            List<string> whiskersLines = new List<string>();
            if (File.Exists(whiskersLogPath))
            {
                whiskersLines.AddRange(File.ReadAllLines(whiskersLogPath));
            }

            // Read existing content of bridged components log file
            List<string> bridgedLines = new List<string>();
            if (File.Exists(bridgedLogPath))
            {
                bridgedLines.AddRange(File.ReadAllLines(bridgedLogPath));
            }

            // Prepare to write to the whiskers log file
            using (StreamWriter whiskersWriter = new StreamWriter(whiskersLogPath, false)) // false to overwrite
            {
                // Write new data first
                whiskersWriter.WriteLine(newData);

                // Write back existing data
                foreach (string line in whiskersLines)
                {
                    whiskersWriter.WriteLine(line);
                }
            }

            // Prepare to write to the bridged components log file
            using (StreamWriter bridgedWriter = new StreamWriter(bridgedLogPath, false)) // false to overwrite
            {
                // Write new data first
                bridgedWriter.WriteLine(newData);

                // Write back existing data
                foreach (string line in bridgedLines)
                {
                    bridgedWriter.WriteLine(line);
                }
            }

            // Prepare to write to the sim state log file
            using (StreamWriter simStateWriter = new StreamWriter(simStateLogPath, false)) // false to overwrite
            {
                // Write new data first
                simStateWriter.WriteLine(newData);
            }

            Debug.Log($"Successfully wrote sim state to the beginning of {whiskersLogPath}, {bridgedLogPath}, and {simStateLogPath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to write sim state to the beginning of {whiskersLogPath} or {bridgedLogPath} or {simStateLogPath}: {ex.Message}");
        }
    }


    public static void LogBridgedWhiskers(HashSet<(int, GameObject, GameObject)> bridgedComponentSets, int simNumber)
    {
        // Define the path where you want to save the results
        string directoryPath = Path.Combine(Application.dataPath, "..", "SimulationResults");
        string fileName = $"bridgedcomponents_log_{simNumber}.csv";
        string fullPath = Path.Combine(directoryPath, fileName);

        try
        {
            // Ensure the directory exists
            Directory.CreateDirectory(directoryPath);

            // Read existing content of bridged components log file
            List<string> existingLines = new List<string>();
            if (File.Exists(fullPath))
            {
                existingLines.AddRange(File.ReadAllLines(fullPath));
            }

            // Prepare to write to the file
            using (StreamWriter writer = new StreamWriter(fullPath, false))
            {
                // Write headers if the file is new
                if (existingLines.Count == 0)
                {
                    writer.WriteLine("Whisker,Component1,Component2");
                }

                // Write the existing content back first
                foreach (string line in existingLines)
                {
                    writer.WriteLine(line);
                }

                // Write the new bridged component pairs
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
