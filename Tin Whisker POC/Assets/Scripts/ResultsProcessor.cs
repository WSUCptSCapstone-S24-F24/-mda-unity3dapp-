using System;
using System.IO;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using SimInfo;
using System.Linq;
using UnityEngine.Analytics;
using System.Globalization;

public class ResultsProcessor : MonoBehaviour
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

        // Creating the file paths for whiskers and bridged components logs
        string whiskersLogPath = Path.Combine(Application.dataPath, "..", "SimulationResults", $"whiskers_log_{simNumber}.csv");
        string bridgedLogPath = Path.Combine(Application.dataPath, "..", "SimulationResults", $"bridgedcomponents_log_{simNumber}.csv");

        try
        {
            // Ensure the directories exist
            Directory.CreateDirectory(Path.GetDirectoryName(whiskersLogPath));
            Directory.CreateDirectory(Path.GetDirectoryName(bridgedLogPath));

            // Get the board size from PCBSize
            PCBSize pcbSize = GameObject.FindObjectOfType<PCBSize>();
            Vector3 boardSizeInCm = pcbSize != null ? pcbSize.GetBoardSizeInCm() : Vector3.zero;

            // Prepare new data to be written (added xTilt, zTilt, and board size)
            string newData = $"WhiskerAmount,SpawnAreaSizeX (mm),SpawnAreaSizeY (mm),SpawnAreaSizeZ (mm),SpawnPositionX (mm),SpawnPositionY (mm),SpawnPositionZ (mm),LengthMu,LengthSigma,WidthMu,WidthSigma,SimNumber,SimDuration (sec),vibrationAmplitude,vibrationSpeed,ShockIntensity,ShockDuration,xTilt,zTilt,BoardSizeX (cm),BoardSizeY (cm),BoardSizeZ (cm)\n" +
                            $"{simState.whiskerAmount},{simState.spawnAreaSizeX},{simState.spawnAreaSizeY},{simState.spawnAreaSizeZ},{simState.spawnPositionX},{simState.spawnPositionY},{simState.spawnPositionZ},{simState.LengthMu},{simState.LengthSigma},{simState.WidthMu},{simState.WidthSigma},{simState.simNumber},{simState.simDuration},{simState.vibrationAmplitude},{simState.vibrationSpeed},{simState.ShockIntensity},{simState.ShockDuration},{simState.xTilt},{simState.zTilt},{boardSizeInCm.x},{boardSizeInCm.y},{boardSizeInCm.z}\n";

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
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to write sim state to the beginning of {whiskersLogPath} or {bridgedLogPath}: {ex.Message}");
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

                // Write the existing content back first
                foreach (string line in existingLines)
                {
                    writer.WriteLine(line);
                }

                // Write headers if the file is new
                writer.WriteLine("Whisker,Component1,Component2");

                // Write the new bridged component pairs
                foreach (var set in bridgedComponentSets)
                {
                    writer.WriteLine($"{set.Item1},{set.Item2.name},{set.Item3.name}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to write to {fullPath}: {ex.Message}");
        }

        bridgedComponentSets.Clear();
    }

    // Function to read and display CSV file given its path
    public void ShowCSVFile(string csvFileName)
    {
        // Construct the full path to the CSV file using Application.dataPath
        string csvFilePath = Path.Combine(Application.dataPath, "..", "SimulationResults", csvFileName);

        if (!File.Exists(csvFilePath))
        {
            Debug.LogError($"File not found: {csvFilePath}");
            PopupManagerSingleton.Instance.ShowPopup("No results found");
            csvText.text = "";
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

    public static void LogMonteCarloResults(int beginningSimNumber, int numSims)
    {
        // TODO: CHANGE ORDER
        // Define the path where you want to save the results
        string directoryPath = Path.Combine(Application.dataPath, "..", "SimulationResults");

        // Calculate results
        List<Dictionary<(string, string), int>> fullPerSimBridgeCounts = new List<Dictionary<(string, string), int>>();
        Dictionary<(string, string), int> pairBridgeCounts = new Dictionary<(string, string), int>();
        Dictionary<string, int> componentBridgeCounts = new Dictionary<string, int>();
        List<int> numBridges = new List<int>();

        int totalSimsWithBridgedComponents = 0;

        for (int i = beginningSimNumber; i < beginningSimNumber + numSims; i++)
        {
            Dictionary<(string, string), int> perSimPairBridgeCounts = new Dictionary<(string, string), int>();
            string fileName = $"bridgedcomponents_log_{i}.csv";
            string fullPath = Path.Combine(directoryPath, fileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"File not found: {fullPath}");
                continue;
            }

            int num_bridges = 0;
            foreach (var line in File.ReadLines(fullPath))
            {
                var parts = line.Split(new char[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 3)
                {
                    if (!parts[1].StartsWith("CO") || !parts[2].StartsWith("CO"))
                    {
                        continue;
                    }
                    num_bridges++;
                    string component1 = parts[1].Trim();
                    string component2 = parts[2].Trim();

                    UpdateComponentCount(componentBridgeCounts, component1);
                    UpdateComponentCount(componentBridgeCounts, component2);
                    UpdatePairCount(pairBridgeCounts, component1, component2);
                    UpdatePairCount(perSimPairBridgeCounts, component1, component2);
                }
            }
            fullPerSimBridgeCounts.Add(perSimPairBridgeCounts);
            numBridges.Add(num_bridges);
            if (num_bridges > 0)
            {
                totalSimsWithBridgedComponents++;
            }

            File.Delete(fullPath);
        }

        int numBuckets = (int)Math.Sqrt(numBridges.Count);
        numBuckets = Math.Max(1, numBuckets); 
        int min = numBridges.Min();
        int max = numBridges.Max();
        int bucketWidth = (int)Math.Ceiling((double)(max - min) / numBuckets);
        int[] buckets = new int[numBuckets];
        foreach (int value in numBridges)
        {
            int bucketIndex = (value - min) / bucketWidth;
            if (bucketIndex == numBuckets) bucketIndex--; 
            buckets[bucketIndex]++;
        }
    

        double percentageWithBridgedComponents = (double)totalSimsWithBridgedComponents / numSims * 100.0;
        // Log results
        string outFileName = $"montecarlo_log_{beginningSimNumber + numSims - 1}.csv";
        string outFullPath = Path.Combine(directoryPath, outFileName);
        using (StreamWriter writer = new StreamWriter(outFullPath, true))
        {
            writer.WriteLine();
            writer.WriteLine($"% sims with bridge: {percentageWithBridgedComponents:F2}%");

            writer.WriteLine();
            writer.WriteLine("Histogram of bridges densities");
            for (int i = 0; i < buckets.Length; i++)
            {
                int bucketStart = min + i * bucketWidth;
                int bucketEnd = bucketStart + bucketWidth - 1;
                writer.WriteLine($"({bucketStart} - {bucketEnd}): {buckets[i]}");
            }

            writer.WriteLine();
            writer.WriteLine("Component,Bridge count");
            foreach (var entry in componentBridgeCounts.OrderByDescending(kv => kv.Value))
            {
                writer.WriteLine($"{entry.Key},{entry.Value}");
            }

            writer.WriteLine();
            writer.WriteLine("Component1,Component2,Bridge pair count");
            foreach (var entry in pairBridgeCounts.OrderByDescending(kv => kv.Value))
            {
                writer.WriteLine($"{entry.Key.Item1},{entry.Key.Item2},{entry.Value}");
            }

            writer.WriteLine();
            writer.WriteLine("Sim number,Component1,Component2,Bridge pair count");
            for (int i = 0; i < fullPerSimBridgeCounts.Count; i++)
            {
                foreach (var entry in fullPerSimBridgeCounts[i].OrderByDescending(kv => kv.Value))
                {
                    writer.WriteLine($"{i},{entry.Key.Item1},{entry.Key.Item2},{entry.Value}");
                }
            }
        }
    }


    public static void LogSimStateToMonteCarlo(SimState simState, int beginningSimNumber, int numSims)
    {
        // Creating the file paths for whiskers and bridged components logs
        string monteCarloLogPath = Path.Combine(Application.dataPath, "..", "SimulationResults", $"montecarlo_log_{beginningSimNumber + numSims - 1}.csv");

        try
        {
            // Ensure the directories exist
            Directory.CreateDirectory(Path.GetDirectoryName(monteCarloLogPath));

            // Get the board size from PCBSize
            PCBSize pcbSize = GameObject.FindObjectOfType<PCBSize>();
            Vector3 boardSizeInCm = pcbSize != null ? pcbSize.GetBoardSizeInCm() : Vector3.zero;

            // Prepare new data to be written, now including vibration, shock, tilt, and board size
            string newData = $"WhiskerAmount,SpawnAreaSizeX (mm),SpawnAreaSizeY (mm),SpawnAreaSizeZ (mm),SpawnPositionX (mm),SpawnPositionY (mm),SpawnPositionZ (mm),LengthMu,LengthSigma,WidthMu,WidthSigma,SimNumber,SimDuration (sec),vibrationAmplitude,vibrationSpeed,ShockIntensity,ShockDuration,xTilt,zTilt,BoardSizeX (cm),BoardSizeY (cm),BoardSizeZ (cm)\n" +
                            $"{simState.whiskerAmount},{simState.spawnAreaSizeX},{simState.spawnAreaSizeY},{simState.spawnAreaSizeZ},{simState.spawnPositionX},{simState.spawnPositionY},{simState.spawnPositionZ},{simState.LengthMu},{simState.LengthSigma},{simState.WidthMu},{simState.WidthSigma},{simState.simNumber},{simState.simDuration},{simState.vibrationAmplitude},{simState.vibrationSpeed},{simState.ShockIntensity},{simState.ShockDuration},{simState.xTilt},{simState.zTilt},{boardSizeInCm.x},{boardSizeInCm.y},{boardSizeInCm.z}\n";

            // Read existing content of whiskers log file
            List<string> whiskersLines = new List<string>();
            if (File.Exists(monteCarloLogPath))
            {
                whiskersLines.AddRange(File.ReadAllLines(monteCarloLogPath));
            }

            // Prepare to write to the whiskers log file
            using (StreamWriter whiskersWriter = new StreamWriter(monteCarloLogPath, false)) // false to overwrite
            {
                // Write new data first
                whiskersWriter.WriteLine(newData);

                // Write back existing data
                foreach (string line in whiskersLines)
                {
                    whiskersWriter.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to write sim state to the beginning of {monteCarloLogPath}: {ex.Message}");
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

    private static void UpdateComponentCount(Dictionary<string, int> componentCounts, string component)
    {
        if (componentCounts.ContainsKey(component))
        {
            componentCounts[component]++;
        }
        else
        {
            componentCounts[component] = 1;
        }
    }

    private static void UpdatePairCount(Dictionary<(string, string), int> pairCounts, string component1, string component2)
    {
        var pair = (string.Compare(component1, component2) < 0) ? (component1, component2) : (component2, component1);

        if (pairCounts.ContainsKey(pair))
        {
            pairCounts[pair]++;
        }
        else
        {
            pairCounts[pair] = 1;
        }
    }
}



