using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResultsHandler : MonoBehaviour
{
    // Reference to the RawImage and dim image GameObject
    public GameObject heatmapImageObject;
    public GameObject dimImageObject;

    // Function to display the results
    public void ShowResults()
    {
        // Activate the dim effect
        TurnOnDim();

        // Generate heatmap results
        StartCoroutine(GenerateHeatmapResultsAsync());

        // Get current working directory
        string workingDirectory = Application.dataPath + "/BridgedComponentsResults";

        // Load the heatmap image from the file
        string imagePath = $"{workingDirectory}/heatmap_image.png";
        Texture2D heatmapTexture = LoadTexture(imagePath);

        // Display the heatmap image
        if (heatmapTexture != null && heatmapImageObject != null)
        {
            RawImage rawImage = heatmapImageObject.GetComponent<RawImage>();
            if (rawImage != null)
            {
                // Assign the texture to the raw image
                rawImage.texture = heatmapTexture;

                // Get the Canvas size
                RectTransform canvasRectTransform =
                    heatmapImageObject.transform.parent.GetComponent<RectTransform>();
                Vector2 canvasSize = canvasRectTransform.sizeDelta;

                // Stretch the RawImage to fill the Canvas
                RectTransform rectTransform = rawImage.rectTransform;
                rectTransform.sizeDelta = canvasSize;
                rectTransform.sizeDelta -= new Vector2(300, 50);

                // Set alpha to full for raw image
                rawImage.color = new Color(
                    rawImage.color.r,
                    rawImage.color.g,
                    rawImage.color.b,
                    1.0f
                );

                // Ensure RawImage is drawn over other elements by setting its sibling index
                rawImage.transform.SetAsLastSibling();
            }
            // Activate the heatmap image object
            if (heatmapImageObject != null)
                heatmapImageObject.SetActive(true);
        }
        // Call method to listen for key press to hide the image
        StartCoroutine(WaitForKeyPress());
    }

    // Alerts user that results are being calculated and dims background
    private void TurnOnDim()
    {
        // Get the Canvas size
        RectTransform canvasRectTransform =
            heatmapImageObject.transform.parent.GetComponent<RectTransform>();
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        if (dimImageObject != null)
        {
            dimImageObject.SetActive(true);
            RawImage dimImage = dimImageObject.GetComponent<RawImage>();

            // Stretch the dim Image to fill the Canvas
            RectTransform rectTransform2 = dimImage.rectTransform;
            rectTransform2.sizeDelta = canvasSize;
            rectTransform2.sizeDelta += new Vector2(300, 300);

            // Set alpha to 0.8 for dim image
            dimImage.color = new Color(dimImage.color.r, dimImage.color.g, dimImage.color.b, 0.8f);
            // Draw dim over
            dimImage.transform.SetAsLastSibling();
        }
    }

    // Function to generate heatmap results asynchronously
    private IEnumerator GenerateHeatmapResultsAsync()
    {
        // Path to the Python executable
        string pythonExePath = Path.Combine(
            Application.dataPath.Replace("Assets", ".venv/bin/python")
        );

        // Path to the Python script
        string pythonScriptPath = Path.Combine(Application.dataPath, "Heatmap.py");

        // Enclose script path in double quotes
        pythonScriptPath = $"\"{pythonScriptPath}\"";

        // Create process info
        ProcessStartInfo psi = new ProcessStartInfo();
        psi.FileName = pythonExePath;
        psi.Arguments = pythonScriptPath;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;
        psi.UseShellExecute = false;

        // Start the process
        Process process = Process.Start(psi);

        // Wait for the process to finish
        yield return new WaitForEndOfFrame();

        // Capture standard output and standard error
        string stdout = process.StandardOutput.ReadToEnd();
        string stderr = process.StandardError.ReadToEnd();

        // Wait for the process to exit
        process.WaitForExit();

        // Check the exit code
        if (process.ExitCode == 0)
        {
            UnityEngine.Debug.Log("Heatmap generation completed successfully.");
        }
        else
        {
            UnityEngine.Debug.LogError("Error generating heatmap. Exit code: " + process.ExitCode);
            UnityEngine.Debug.LogError("Standard Error: " + stderr);
        }

        // Activate the heatmap image after generation
        if (heatmapImageObject != null)
            heatmapImageObject.SetActive(true);
    }

    // Coroutine to wait for any key press to hide the image
    IEnumerator WaitForKeyPress()
    {
        // Wait until any key is pressed
        yield return new WaitUntil(() => Input.anyKeyDown);

        // Hide the image
        if (heatmapImageObject != null)
            heatmapImageObject.SetActive(false);
        if (dimImageObject != null)
            dimImageObject.SetActive(false);
    }

    // Function to load a texture from a file path
    private Texture2D LoadTexture(string path)
    {
        byte[] fileData = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        return texture;
    }
}
