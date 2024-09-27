using UnityEngine;
using SimInfo;
using System.Collections;

public class Vibration : MonoBehaviour
{
    private Vector3 initialPosition;
    private float t;
    private SceneHandler sceneHandler;
    private WhiskerSim whiskerSim;
    public float twitchFrequency = 10f; 
    private bool isTwitching = false;

    public void Start()
    {
        //StartCoroutine(InitializeVibration());
    }

    public IEnumerator InitializeVibration()
    {
        // Wait until SceneHandler and WhiskerSim are available
        while (sceneHandler == null || whiskerSim == null)
        {
            sceneHandler = FindObjectOfType<SceneHandler>();
            whiskerSim = FindObjectOfType<WhiskerSim>();

            if (sceneHandler == null)
            {
                Debug.LogError("SceneHandler not found in the scene.");
            }

            if (whiskerSim == null)
            {
                Debug.LogError("WhiskerSim not found in the scene.");
            }

            yield return null; // Wait for the next frame
        }

        // Once both are available, start the vibration process
        t = Time.time;
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
        if (sceneHandler != null && sceneHandler.simState != null && whiskerSim != null && isTwitching)
        {
            SimState simState = sceneHandler.simState;
            float offset = simState.vibrationAmplitude * Mathf.Sin(simState.vibrationSpeed * (Time.time - t));
            float velocityTolerance = 0.01f; // Tolerance for zero velocity check

            foreach (GameObject whisker in whiskerSim.cylinder_clone)
            {
                if (whisker != null)
                {
                    Rigidbody rb = whisker.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        // Check if the velocity is approximately zero or moving upwards
                        if (Mathf.Abs(rb.velocity.y) < velocityTolerance)
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