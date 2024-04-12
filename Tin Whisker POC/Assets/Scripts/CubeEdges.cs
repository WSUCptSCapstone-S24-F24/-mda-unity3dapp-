using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CubeEdges : MonoBehaviour
{
    public TMP_InputField VolumeX;
    public TMP_InputField VolumeY;
    public TMP_InputField VolumeZ;

    public TMP_InputField PositionX;
    public TMP_InputField PositionY;
    public TMP_InputField PositionZ;

    private void Start()
    {
        // Add listeners to handle input field changes
        AddListeners();
        // Draw the initial cube
        DrawEdges();
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
    }

    private void OnValueChanged(TMP_InputField inputField)
    {
        // Parse the value from the input field
        float value;
        if (float.TryParse(inputField.text, out value))
        {
            // Value is valid, redraw the cube
            DrawEdges();
        }
        else
        {
            // Invalid input, reset the input field to previous value
            Debug.LogWarning("Invalid input: " + inputField.text);
            inputField.text = "0"; // Reset the input field to a default value
        }
    }

    private void DrawEdges()
    {
        // Parse and validate input field values
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

        // Define cube vertices based on volume and position variables
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-volumeX / 2 + positionX, -volumeY / 2 + positionY, -volumeZ / 2 + positionZ),
            new Vector3(volumeX / 2 + positionX, -volumeY / 2 + positionY, -volumeZ / 2 + positionZ),
            new Vector3(volumeX / 2 + positionX, -volumeY / 2 + positionY, volumeZ / 2 + positionZ),
            new Vector3(-volumeX / 2 + positionX, -volumeY / 2 + positionY, volumeZ / 2 + positionZ),
            new Vector3(-volumeX / 2 + positionX, volumeY / 2 + positionY, -volumeZ / 2 + positionZ),
            new Vector3(volumeX / 2 + positionX, volumeY / 2 + positionY, -volumeZ / 2 + positionZ),
            new Vector3(volumeX / 2 + positionX, volumeY / 2 + positionY, volumeZ / 2 + positionZ),
            new Vector3(-volumeX / 2 + positionX, volumeY / 2 + positionY, volumeZ / 2 + positionZ)
        };

        // Define cube edges
        int[] indices = new int[]
        {
            // Original edges
            0, 1, 1, 2, 2, 3, 3, 0,
            4, 5, 5, 6, 6, 7, 7, 4,
            0, 4, 1, 5, 2, 6, 3, 7
        };

        // Offset vertices for bolder lines
        Vector3[] boldVertices = new Vector3[vertices.Length * 2];
        for (int i = 0; i < vertices.Length; i++)
        {
            boldVertices[i * 2] = vertices[i];
            boldVertices[i * 2 + 1] = vertices[i] + Vector3.up * 0.01f; // Adjust offset as needed for bolder lines
        }

        // Offset indices for bolder lines
        int[] boldIndices = new int[indices.Length * 2];
        for (int i = 0; i < indices.Length; i++)
        {
            boldIndices[i * 2] = indices[i] * 2;
            boldIndices[i * 2 + 1] = indices[i] * 2 + 1;
        }

        // Get or create mesh
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        if (mesh == null)
        {
            mesh = new Mesh();
            meshFilter.mesh = mesh;
        }

        // Set mesh data for bolder lines
        mesh.Clear();
        mesh.vertices = boldVertices;
        mesh.SetIndices(boldIndices, MeshTopology.Lines, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // Assign default material
        GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
    }
}
