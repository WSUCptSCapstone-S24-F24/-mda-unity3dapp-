using UnityEngine;
using System.Collections;
using TMPro;

public class CameraMover : MonoBehaviour
{
    public GameObject Camera;
    public float moveDuration = 2.0f;
    public float heightAboveObject = 10.0f;
    public TMP_InputField targetInputField;

    private bool isMoving = false;
    private bool cameraFound = false;
    private Vector3 startPosition;
    private Vector3 targetCenter;  // To store the center of the target renderer's bounds
    private float startTime;

    public void MoveBasedOnInputField()
    {
        string targetName = ("CO" + targetInputField.text);
        Debug.Log("Target name: " + targetName);

        GameObject foundObject = GameObject.Find(targetName);

        if (foundObject == null)
        {
            Debug.LogError("No game object found with the name: " + targetName);
            return;
        }

        Renderer renderer = foundObject.GetComponent<Renderer>();

        if (renderer == null)
        {
            Debug.LogError("The target object does not have a Renderer component!");
            return;
        }

        targetCenter = renderer.bounds.center;  // Store the center of the renderer bounds

        MoveCameraToTarget();
    }

    public void MoveCameraToTarget()
    {
        if (!cameraFound)
        {
            Camera = GameObject.Find("Main Camera");
            if (Camera == null)
            {
                Debug.LogError("No camera found!");
                return;
            }
            Debug.Log("Camera found");
            cameraFound = true;
        }

        startPosition = Camera.transform.position;
        Vector3 endPosition = targetCenter + Vector3.up * heightAboveObject;

        startTime = Time.time;
        isMoving = true;

        // Coroutine to handle both movement and rotation smoothly
        StartCoroutine(MoveAndRotate(endPosition));
    }

    private IEnumerator MoveAndRotate(Vector3 endPosition)
    {
        while (isMoving)
        {
            Debug.Log("Moving camera to target");
            float elapsedTime = Time.time - startTime;
            float fraction = elapsedTime / moveDuration;

            Camera.transform.position = Vector3.Lerp(startPosition, endPosition, fraction);
            Camera.transform.LookAt(targetCenter);  // Use targetCenter instead of targetObject.position

            if (fraction >= 1.0f)
            {
                isMoving = false;
            }

            yield return null;  // Wait for the next frame
        }
    }
}
