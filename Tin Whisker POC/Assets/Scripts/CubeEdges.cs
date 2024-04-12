using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CubeEdges : MonoBehaviour
{
    public GameObject VolumeX;
    public GameObject VolumeY;
    public GameObject VolumeZ;

    public GameObject PositionX;
    public GameObject PositionY;
    public GameObject PositionZ;



    private void Start()
    {
        DrawEdges();
    }

    private void DrawEdges()
    {
        // Get or create mesh
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        if (mesh == null)
        {
            mesh = new Mesh();
            meshFilter.mesh = mesh;
        }

        // Define cube vertices
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f)
        };

        // Define cube edges
        int[] indices = new int[]
        {
            0, 1,
            1, 2,
            2, 3,
            3, 0,
            4, 5,
            5, 6,
            6, 7,
            7, 4,
            0, 4,
            1, 5,
            2, 6,
            3, 7
        };

        // Set mesh data
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // Assign default material
        GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
    }
}
