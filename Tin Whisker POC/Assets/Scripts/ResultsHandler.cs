using System.Collections;
using System.Collections.Generic;
using SimInfo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsHandler : MonoBehaviour {
    // Reference to the RawImage and blur image GameObject
    public GameObject heatmapImageObject;
    public GameObject blurImageObject;

    public void GenerateHeatmapResults() {
        
    }

    public void ShowResults()
    {
        if (heatmapImageObject != null)
            heatmapImageObject.SetActive(true);
        if (blurImageObject != null)
            blurImageObject.SetActive(true);

        // Get current working directory
        string workingDirectory = Application.dataPath + "/BridgedComponentsResults";

        // Load the heatmap image from the file
        string imagePath = $"{workingDirectory}/heatmap_image.png";
        Texture2D heatmapTexture = LoadTexture(imagePath);

        // Display the heatmap image
        if (heatmapTexture != null && heatmapImageObject != null)
        {
            RawImage rawImage = heatmapImageObject.GetComponent<RawImage>();
            RawImage blurImage = blurImageObject.GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.texture = heatmapTexture;

                // Get the Canvas size
                RectTransform canvasRectTransform =
                    heatmapImageObject.transform.parent.GetComponent<RectTransform>();
                Vector2 canvasSize = canvasRectTransform.sizeDelta;

                // Stretch the RawImage to fill the Canvas
                RectTransform rectTransform = rawImage.rectTransform;
                rectTransform.sizeDelta = canvasSize;
                rectTransform.sizeDelta -= new Vector2(300, 50);

                // Stretch the RawImage to fill the Canvas
                RectTransform rectTransform2 = blurImage.rectTransform;
                rectTransform2.sizeDelta = canvasSize;
                rectTransform2.sizeDelta += new Vector2(300, 300);

                // Set alpha to full
                rawImage.color = new Color(
                    rawImage.color.r,
                    rawImage.color.g,
                    rawImage.color.b,
                    1.0f
                );
                blurImage.color = new Color(
                    blurImage.color.r,
                    blurImage.color.g,
                    blurImage.color.b,
                    0.8f
                );

                // Ensure RawImage is drawn over other elements by setting its sibling index
                blurImage.transform.SetAsLastSibling();
                rawImage.transform.SetAsLastSibling();
            }
            // Call method to listen for key press to hide the image
            StartCoroutine(WaitForKeyPress());
        }
    }

    // Coroutine to wait for any key press to hide the image
    IEnumerator WaitForKeyPress()
    {
        // Wait until any key is pressed
        yield return new WaitUntil(() => Input.anyKeyDown);

        // Hide the image
        if (heatmapImageObject != null)
            heatmapImageObject.SetActive(false);
        if (blurImageObject != null)
            blurImageObject.SetActive(false);
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