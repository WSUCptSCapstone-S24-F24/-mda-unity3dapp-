using System.IO;
using UnityEngine;
using TMPro;

public class CSVHandler : MonoBehaviour
{
    public TextMeshProUGUI csvText;

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
        csvText.text = "";

        string[] lines = File.ReadAllLines(csvFilePath);

        foreach (string line in lines)
        {
            string[] fields = line.Split(','); // Split the line into fields using comma as delimiter

            // Construct the row using TextMeshPro's rich text formatting
            string row = "<line-height=150%>";

            foreach (string field in fields)
            {
                row += field + "    "; // Add each field separated by tabs (adjust spacing as needed)
            }

            row += "\n</line-height>";

            csvText.text += row; // Append the row to the TextMeshProUGUI component
        }
    }
}
