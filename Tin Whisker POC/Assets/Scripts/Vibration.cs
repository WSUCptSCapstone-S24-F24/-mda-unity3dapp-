using UnityEngine;
using SimInfo;

public class Vibration : MonoBehaviour
{
    public GameObject Board;
    private Vector3 initialPosition;     // Initial position of the GameObject.
    private bool status = true;
    private float t;

    private SceneHandler sceneHandler;

    public void Start()
    {
        Board = GameObject.FindWithTag("Board");
        initialPosition = Board.transform.position; // Store the initial position of the GameObject.
        status = !status;
        t = Time.time;

        sceneHandler = FindObjectOfType<SceneHandler>();
        if (sceneHandler == null)
        {
            Debug.LogError("SceneHandler not found in the scene.");
        }
    }

    void Update()
    {
        if (sceneHandler != null && sceneHandler.simState != null)
        {
            SimState simState = sceneHandler.simState;
            float verticalPosition = initialPosition.y + simState.vibrationAmplitude * Mathf.Sin(simState.vibrationSpeed * (Time.time - t));
            if (status)
            {
                Board.transform.position = Vector3.MoveTowards(Board.transform.position, new Vector3(initialPosition.x, verticalPosition, initialPosition.z), 1.0f);
            }
        }
    }
}