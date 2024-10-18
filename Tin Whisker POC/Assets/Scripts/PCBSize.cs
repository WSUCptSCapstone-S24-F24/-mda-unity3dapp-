using UnityEngine;

public class PCBSize : MonoBehaviour
{
    private GameObject mainCircuitBoard;

    // Finds the object tagged as "Board" and calculates the size of its specific child named "CO0"
    public Vector3 GetBoardSizeInCm()
    {
        // Find the MainCircuitBoard tagged as "Board"
        mainCircuitBoard = GameObject.FindWithTag("Board");

        if (mainCircuitBoard == null)
        {
            Debug.LogError("MainCircuitBoard with tag 'Board' is not found.");
            return Vector3.zero;
        }

        // Search for the child object named "CO0"
        Transform childCO0 = mainCircuitBoard.transform.Find("CO0");

        // If no child object "CO0" is found, log an error
        if (childCO0 == null)
        {
            Debug.LogError("No child object named 'CO0' found under MainCircuitBoard.");
            return Vector3.zero;
        }

        // Try to get the MeshCollider on the child object "CO0"
        MeshCollider meshCollider = childCO0.GetComponent<MeshCollider>();

        // If no MeshCollider is attached, log an error
        if (meshCollider == null)
        {
            Debug.LogError("No MeshCollider found on 'CO0'. Unable to calculate size.");
            return Vector3.zero;
        }

        // Calculate the bounds based on the mesh collider
        Bounds boardBounds = meshCollider.bounds;
        Vector3 boardSizeInMeters = boardBounds.size;

        // Return the size in centimeters (Unity units are usually meters)
        return boardSizeInMeters / 10f; // Convert to centimeters
    }
}






