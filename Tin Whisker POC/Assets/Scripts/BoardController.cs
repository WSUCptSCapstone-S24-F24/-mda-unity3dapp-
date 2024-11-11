using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class BoardController : MonoBehaviour
{
    public static Vector3 BoardSize;
    public TMP_InputField xTiltInput;
    public TMP_InputField zTiltInput;
    public TMP_InputField BoardXSize;
    public TMP_InputField BoardYSize;
    public TMP_InputField BoardZSize;
    public TMP_InputField BoardXPos;
    public TMP_InputField BoardYPos;
    public TMP_InputField BoardZPos;
    private GameObject board;
    private GameObject previousBoard;  // Track the previously loaded board
    private bool boardLoaded = false;
    private float scaler = 10;

    private Vector3 unitsScaledBoardSize;
    private Vector3 originalBoardScale; // To store the original scale of the board
    private float yToXRatio;
    private float zToXRatio;

    void Start()
    {
        AddListeners();
    }

    void OnEnable()
    {
        StartCoroutine(WaitForBoardToLoad());
    }

    IEnumerator WaitForBoardToLoad()
    {
        while (true)
        {
            board = GameObject.FindWithTag("Board");

            // Check if the board has changed
            if (board != previousBoard)
            {
                previousBoard = board;

                if (board != null)
                {
                    boardLoaded = true;
                    originalBoardScale = BoundingBoxCalculator.CalculateTotalBounds(board);
                    float toOneXChangeScale = 1 / originalBoardScale.x;
                    unitsScaledBoardSize = new Vector3(board.transform.localScale.x * toOneXChangeScale,
                                                        board.transform.localScale.y * toOneXChangeScale,
                                                        board.transform.localScale.z * toOneXChangeScale);
                    UpdateSizes(unitsScaledBoardSize.x, unitsScaledBoardSize.y, unitsScaledBoardSize.z);
                    yToXRatio = unitsScaledBoardSize.y / unitsScaledBoardSize.x;
                    zToXRatio = unitsScaledBoardSize.z / unitsScaledBoardSize.x;
                    UpdateBoardProperties();
                }
                else
                {
                    boardLoaded = false;
                }
            }

            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds to reduce load
        }
    }

    private void AddListeners()
    {
        xTiltInput.onValueChanged.AddListener(delegate { OnValueChanged(xTiltInput); });
        zTiltInput.onValueChanged.AddListener(delegate { OnValueChanged(zTiltInput); });
        BoardXSize.onEndEdit.AddListener(delegate { OnSizeFieldChanged(BoardXSize, "X"); });
        BoardYSize.onEndEdit.AddListener(delegate { OnSizeFieldChanged(BoardYSize, "Y"); });
        BoardZSize.onEndEdit.AddListener(delegate { OnSizeFieldChanged(BoardZSize, "Z"); });
        BoardXPos.onEndEdit.AddListener(delegate { OnValueChanged(BoardXPos); });
        BoardYPos.onEndEdit.AddListener(delegate { OnValueChanged(BoardYPos); });
        BoardZPos.onEndEdit.AddListener(delegate { OnValueChanged(BoardZPos); });
        UpdateBoardProperties();
    }

    private void OnValueChanged(TMP_InputField inputField)
    {
        float value;
        if (float.TryParse(inputField.text, out value))
        {
            UpdateBoardProperties();
        }
        else
        {
            Debug.LogWarning("Invalid input: " + inputField.text);
        }
    }

    private void OnSizeFieldChanged(TMP_InputField inputField, string field)
    {
        float size;
        if (!float.TryParse(inputField.text, out size))
        {
            Debug.LogWarning("Invalid input: " + inputField.text);
            return;
        }

        // Update board size based on the changed field and preserve ratios
        switch (field)
        {
            case "X":
                UpdateSizes(size, size * yToXRatio, size * zToXRatio);
                break;
            case "Y":
                UpdateSizes(size / yToXRatio, size, size / yToXRatio * zToXRatio);
                break;
            case "Z":
                UpdateSizes(size / zToXRatio, size / zToXRatio * yToXRatio, size);
                break;
        }

        UpdateBoardProperties();
    }

    private void UpdateSizes(float xSize, float ySize, float zSize)
    {
        // Update input fields with the calculated values
        BoardXSize.text = xSize.ToString("F2");
        BoardYSize.text = ySize.ToString("F2");
        BoardZSize.text = zSize.ToString("F2");
    }

    public void UpdateBoardProperties()
    {
        float boardXSize, boardYSize, boardZSize;
        float boardXPos, boardYPos, boardZPos;
        float xTilt, zTilt;

        if (!float.TryParse(BoardXSize.text, out boardXSize) ||
            !float.TryParse(BoardYSize.text, out boardYSize) ||
            !float.TryParse(BoardZSize.text, out boardZSize) ||
            !float.TryParse(BoardXPos.text, out boardXPos) ||
            !float.TryParse(BoardYPos.text, out boardYPos) ||
            !float.TryParse(BoardZPos.text, out boardZPos) ||
            !float.TryParse(xTiltInput.text, out xTilt) ||
            !float.TryParse(zTiltInput.text, out zTilt))
        {
            Debug.LogWarning("Invalid input. Unable to parse float values.");
            return;
        }

        if (boardLoaded && board != null)
        {
            board.transform.localScale = new Vector3(boardXSize * scaler, boardYSize * scaler, boardZSize * scaler);
            board.transform.position = new Vector3(boardXPos * scaler, boardYPos * scaler, boardZPos * scaler);
            board.transform.rotation = Quaternion.Euler(xTilt, board.transform.rotation.eulerAngles.y, zTilt);
        }
    }
}
