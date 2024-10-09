using UnityEngine;
using TMPro; // For TMP_InputField
using System.Collections;

public class BoardTiltController : MonoBehaviour
{
    public TMP_InputField xTiltInput; // Input field for X-axis tilt
    public TMP_InputField zTiltInput; // Input field for Z-axis tilt
    private GameObject board; // The board object that will be tilted
    private GameObject scaleGrid; // The ScaleGrid object that will also be tilted
    private bool boardLoaded = false;
    private bool scaleGridLoaded = false;

    void Start()
    {

        // Start coroutines to wait for the board and scaleGrid to load
        StartCoroutine(WaitForBoardToLoad());
        StartCoroutine(WaitForScaleGridToLoad());

        // Add listeners to apply tilt when the input fields are changed
        xTiltInput.onValueChanged.AddListener(delegate { TiltObjects(); });
        zTiltInput.onValueChanged.AddListener(delegate { TiltObjects(); });
    }

    // Coroutine to wait until the board object is loaded
    IEnumerator WaitForBoardToLoad()
    {
        while (board == null)
        {
            // Keep trying to find the board tagged as "Board"
            board = GameObject.FindWithTag("Board");

            if (board != null)
            {
                boardLoaded = true;
                Debug.Log("Board found, applying tilt.");

                // Apply the initial tilt based on current input field values
                TiltObjects();
            }
            else
            {
                Debug.Log("Waiting for the board to be loaded...");
            }

            // Wait for the next frame and try again
            yield return null;
        }
    }

    // Coroutine to wait until the ScaleGrid object is loaded
    IEnumerator WaitForScaleGridToLoad()
    {
        while (scaleGrid == null)
        {
            // Keep trying to find the ScaleGrid tagged as "ScaleGrid"
            scaleGrid = GameObject.FindWithTag("ScaleGrid");

            if (scaleGrid != null)
            {
                scaleGridLoaded = true;
                Debug.Log("ScaleGrid found, applying tilt.");

                // Apply the initial tilt based on current input field values
                TiltObjects();
            }
            else
            {
                Debug.Log("Waiting for the ScaleGrid to be loaded...");
            }

            // Wait for the next frame and try again
            yield return null;
        }
    }

    // Method to tilt both the board and ScaleGrid based on input field values
    public void TiltObjects()
    {
        // Tilt the board if it is loaded
        if (boardLoaded && board != null)
        {
            float xTilt = ParseInputField(xTiltInput);
            float zTilt = ParseInputField(zTiltInput);

            // Apply the tilt to the board (preserving the Y rotation)
            board.transform.rotation = Quaternion.Euler(xTilt, board.transform.rotation.eulerAngles.y, zTilt);
        }

        // Tilt the ScaleGrid if it is loaded
        if (scaleGridLoaded && scaleGrid != null)
        {
            float xTilt = ParseInputField(xTiltInput);
            float zTilt = ParseInputField(zTiltInput);

            // Apply the tilt to the ScaleGrid (preserving the Y rotation)
            scaleGrid.transform.rotation = Quaternion.Euler(xTilt, scaleGrid.transform.rotation.eulerAngles.y, zTilt);
        }
    }

    // Helper function to parse the input field value and return a float
    private float ParseInputField(TMP_InputField inputField)
    {
        if (float.TryParse(inputField.text, out float result))
        {
            return result;
        }
        else
        {
            Debug.LogError($"Invalid input in {inputField.name}. Defaulting to 0.");
            return 0f; // Default to 0 if parsing fails
        }
    }
}
