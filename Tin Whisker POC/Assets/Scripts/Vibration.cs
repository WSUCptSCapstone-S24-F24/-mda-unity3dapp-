using UnityEngine;
using SimInfo;
using System.Collections;

public class Vibration : MonoBehaviour
{
    public GameObject Board;
    private Vector3 initialPosition;
    private bool status = true;
    private float t;
    private SceneHandler sceneHandler;
    private WhiskerSim whiskerSim;
    public float twitchFrequency = 10f; // Twitches per second
    private bool isTwitching = false;

    public void Start()
    {
        Board = GameObject.FindWithTag("Board");
        initialPosition = Board.transform.position;
        status = !status;
        t = Time.time;
        sceneHandler = FindObjectOfType<SceneHandler>();
        if (sceneHandler == null)
        {
            Debug.LogError("SceneHandler not found in the scene.");
        }
        whiskerSim = FindObjectOfType<WhiskerSim>();
        if (whiskerSim == null)
        {
            Debug.LogError("WhiskerSim not found in the scene.");
        }

        StartCoroutine(TwitchCoroutine());
    }

    IEnumerator TwitchCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / twitchFrequency);
            isTwitching = true;
            yield return new WaitForFixedUpdate();
            isTwitching = false;
        }
    }

    void FixedUpdate()
    {
        if (sceneHandler != null && sceneHandler.simState != null && whiskerSim != null && status && isTwitching)
        {
            SimState simState = sceneHandler.simState;
            float offset = simState.vibrationAmplitude * Mathf.Sin(simState.vibrationSpeed * (Time.time - t));
            foreach (GameObject whisker in whiskerSim.cylinder_clone)
            {
                if (whisker != null)
                {
                    Rigidbody rb = whisker.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        // Check if the whisker is falling (negative vertical velocity)
                        if (rb.velocity.y >= 0)
                        {
                            Vector3 twitchForce = new Vector3(offset, 0, 0);
                            rb.AddForce(twitchForce, ForceMode.Impulse);
                        }
                    }
                }
            }
        }
    }
}