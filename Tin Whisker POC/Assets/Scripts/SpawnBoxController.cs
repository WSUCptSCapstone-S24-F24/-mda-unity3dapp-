using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SpawnBoxController : MonoBehaviour
{
    public TMP_InputField VolumeX;
    public TMP_InputField VolumeY;
    public TMP_InputField VolumeZ;
    public TMP_InputField PositionX;
    public TMP_InputField PositionY;
    public TMP_InputField PositionZ;
    public Toggle showBox;
    public float scaler = 10;

    private GameObject cube;

    void Start()
    {
        AddListeners();

    }

    private void AddListeners()
    {
        // Add listeners to VolumeX, VolumeY, VolumeZ, PositionX, PositionY, and PositionZ input fields
        VolumeX.onEndEdit.AddListener(delegate { OnValueChanged(VolumeX); });
        VolumeY.onEndEdit.AddListener(delegate { OnValueChanged(VolumeY); });
        VolumeZ.onEndEdit.AddListener(delegate { OnValueChanged(VolumeZ); });
        PositionX.onEndEdit.AddListener(delegate { OnValueChanged(PositionX); });
        PositionY.onEndEdit.AddListener(delegate { OnValueChanged(PositionY); });
        PositionZ.onEndEdit.AddListener(delegate { OnValueChanged(PositionZ); });
        showBox.onValueChanged.AddListener(delegate { OnToggleValueChanged(); });

        // After adding listeners, explicitly call UpdateCubeProperties to initialize cube size and position
        UpdateCubeProperties();
    }

    private void OnValueChanged(TMP_InputField inputField)
    {
        // Parse the value from the input field
        float value;
        if (float.TryParse(inputField.text, out value))
        {
            // Value is valid, redraw the cube
            UpdateCubeProperties();
        }
        else
        {
            // Invalid input, reset the input field to previous value
            Debug.LogWarning("Invalid input: " + inputField.text);
            inputField.text = "0"; // Reset the input field to a default value
        }
    }

    public void OnToggleValueChanged()
    {
        cube.SetActive(showBox.isOn);
    }

    public void UpdateCubeProperties()
    {
        float volumeX, volumeY, volumeZ, positionX, positionY, positionZ;
        if (!float.TryParse(VolumeX.text, out volumeX) ||
            !float.TryParse(VolumeY.text, out volumeY) ||
            !float.TryParse(VolumeZ.text, out volumeZ) ||
            !float.TryParse(PositionX.text, out positionX) ||
            !float.TryParse(PositionY.text, out positionY) ||
            !float.TryParse(PositionZ.text, out positionZ))
        {
            // If parsing fails, log a warning and return without drawing the cube
            Debug.LogWarning("Invalid input. Unable to parse float values.");
            return;
        }

        // Find or create the cube GameObject
        if (cube == null)
        {
            cube = GameObject.Find("WhiskerSpawnBox");
            if (cube == null)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = "WhiskerSpawnBox";
            }
        }

        // Update cube scale
        cube.transform.localScale = new Vector3(volumeX * scaler, volumeY * scaler, volumeZ * scaler);

        // Update cube position
        cube.transform.position = new Vector3(positionX * scaler - 5f, positionY * scaler + volumeY * scaler / 2, positionZ * scaler - 5f);
    }
}
