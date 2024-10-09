using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI; // For Button
using SFB; // StandaloneFileBrowser

public class ComponentHeatMap : MonoBehaviour
{
    private Dictionary<string, int> componentBridgeCounts = new Dictionary<string, int>();
    private GameObject boardModel; // Dynamically find the board with "Board" tag
    public Button HeatMapperButton; // Reference to the Button UI component

    void Start()
    {
        // Set up the HeatMapper button
        SetUpHeatMapperButton();
    }

    private void SetUpHeatMapperButton()
    {
        // Ensure the button is assigned in the Inspector
        if (HeatMapperButton != null)
        {
            HeatMapperButton.onClick.AddListener(OpenLogFileBrowser);
        }
        else
        {
            Debug.LogError("HeatMapperButton not assigned in the Inspector.");
        }
    }

    // Open the file browser to let the user select a CSV file
    public void OpenLogFileBrowser()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open CSV File", "", "csv", false);

        if (paths.Length > 0)
        {
            string logFilePath = paths[0];
            Debug.Log("Selected Log File: " + logFilePath);

            // Attempt to load the heat map from the selected file
            ApplyHeatMap(logFilePath);
        }
        else
        {
            Debug.Log("No file selected.");
        }
    }

    // Attempt to apply the heat map from the selected log file
    private void ApplyHeatMap(string logFilePath)
    {
        // Step 1: Find the board model with the tag "Board"
        boardModel = GameObject.FindGameObjectWithTag("Board");

        if (boardModel == null)
        {
            Debug.LogError("No board model with tag 'Board' found.");
            return;
        }

        // Step 2: Read the log file and parse the bridge counts
        if (!ParseLogFile(logFilePath))
        {
            Debug.LogError("Error: Invalid CSV file format.");
            return;
        }

        // Step 3: Find the maximum bridge count
        int maxBridgeCount = GetMaxBridgeCount();

        // Step 4: Assign colors based on the bridge counts
        ApplyHeatMapColors(maxBridgeCount);
    }

    private bool ParseLogFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"Log file not found at: {path}");
            return false;
        }

        string[] lines = File.ReadAllLines(path);
        bool bridgeSection = false;
        componentBridgeCounts.Clear(); // Clear any previous data

        foreach (string line in lines)
        {
            // Skip empty or whitespace-only lines
            if (string.IsNullOrWhiteSpace(line))
            {
                // Stop parsing when encountering empty or whitespace after the bridge section
                if (bridgeSection)
                {
                    break;
                }
                continue;
            }

            // Identify the start of the "Component, Bridge count" section
            if (line.StartsWith("Component,Bridge count"))
            {
                bridgeSection = true;
                continue;
            }

            // Start reading component bridge counts after the "Component, Bridge count" section
            if (bridgeSection && !string.IsNullOrEmpty(line))
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2)
                {
                    string component = parts[0].Trim(); // Ensure there's no leading/trailing whitespace
                    string countStr = parts[1].Trim();  // Trim count value

                    // Ensure the count is a valid integer
                    if (int.TryParse(countStr, out int bridgeCount))
                    {
                        componentBridgeCounts[component] = bridgeCount;
                    }
                    else
                    {
                        Debug.LogError($"Invalid bridge count format for component: {component} with value: {countStr}");
                        return false; // Invalid CSV format
                    }
                }
                else
                {
                    Debug.LogError($"Unexpected format in line: {line}. Expected 2 parts but got {parts.Length}.");
                    return false;
                }
            }
        }

        // Ensure that at least one component was parsed
        return componentBridgeCounts.Count > 0;
    }

    private int GetMaxBridgeCount()
    {
        int maxCount = 0;
        foreach (var count in componentBridgeCounts.Values)
        {
            maxCount = Mathf.Max(maxCount, count);
        }
        return maxCount;
    }

    private void ApplyHeatMapColors(int maxCount)
    {
        if (boardModel == null)
        {
            Debug.LogError("Model is not loaded.");
            return;
        }

        foreach (Transform child in boardModel.transform)
        {
            string componentName = child.gameObject.name;

            if (componentBridgeCounts.ContainsKey(componentName))
            {
                int bridgeCount = componentBridgeCounts[componentName];

                // Normalize the bridge count between 0 and 1
                float normalizedBridgeCount = (float)bridgeCount / maxCount;

                // Interpolate between green (no bridges) and red (max bridges)
                Color heatMapColor = Color.Lerp(Color.green, Color.red, normalizedBridgeCount);

                // Apply the color to the component's material
                Renderer renderer = child.gameObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = heatMapColor;
                }
            }
        }
    }
}
