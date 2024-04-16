using System;
using System.IO;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using SimInfo;
public class CSVHandler : MonoBehaviour
{
    public TextMeshProUGUI csvText;
    public int padding = 0; // Padding between columns

    public void LogWhiskers(List<GameObject> whiskers, int simNumber)
    {
        // Create file path to inside /Users/trevorbuchanan/Desktop/Classes/CPTS421/-mda-unity3dapp-/Tin Whisker POC/SimulationResults
        // string csvFilePath = Path.Combine(Application.dataPath, "..", "SimulationResults", $"whisker_log_{simNumber}.csv"); // Make sure note monte carlo sim
        // Check if file already exists
        // if already exisits, then clear and write to file
        // otherwise create the file and write the results
    }

    public void LogSimState(SimState simState, int simNumber)
    {
        // Create file path to inside /Users/trevorbuchanan/Desktop/Classes/CPTS421/-mda-unity3dapp-/Tin Whisker POC/SimulationResults
        // string csvFilePath = Path.Combine(Application.dataPath, "..", "SimulationResults", "simState_log_{simNumber}.csv");
        // Check if file already exists
        // if already exisits, then clear and write to file
        // otherwise create the file and write the results
    }

    // Function to read and display CSV file given its path
    public void ShowCSVFile(string csvFileName)
    {
        // Construct the full path to the CSV file using Application.dataPath
        string csvFilePath = Path.Combine(Application.dataPath, "..", "SimulationResults", csvFileName);

        if (!File.Exists(csvFilePath))
        {
            Debug.LogError($"File not found: {csvFilePath}");
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
