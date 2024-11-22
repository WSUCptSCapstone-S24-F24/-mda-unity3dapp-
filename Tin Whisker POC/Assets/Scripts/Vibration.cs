using UnityEngine;
using SimInfo;
using System.Collections;

public class Vibration : MonoBehaviour
{
    private Vector3 initialPosition;
    private float t;
    private MainController MainController;
    public float twitchFrequency = 10f;
    private bool isTwitching = false;
    public bool isVibrationActive = false;
    private Coroutine vibrationCoroutine;

    public void Start()
    {
        // You can initialize values here if necessary.
    }

    public void StartVibration()
    {
        // Avoid starting multiple coroutines by checking if one is already running.
        if (!isVibrationActive)
        {
            isVibrationActive = true;
            StartCoroutine(InitializeVibration());
        }
    }

    public IEnumerator InitializeVibration()
    {
        // Wait until MainController and WhiskerSim are available
        while (MainController == null || MainController.whiskerSim == null)
        {
            MainController = FindObjectOfType<MainController>();
            if (MainController == null)
            {
                Debug.LogError("MainController not found in the scene.");
                yield break; // Exit the coroutine if MainController is not found
            }

            if (MainController.whiskerSim == null)
            {
                Debug.LogError("WhiskerSim not found in the scene.");
                yield break; // Exit the coroutine if WhiskerSim is not found
            }

            yield return null; // Wait for the next frame before checking again
        }

        // Once both are available, start the vibration process
        t = Time.time;
        if (vibrationCoroutine == null)  // Avoid starting multiple coroutines
        {
            vibrationCoroutine = StartCoroutine(TwitchCoroutine());
        }
    }

    public void StopVibration()
    {
        if (vibrationCoroutine != null)
        {
            isVibrationActive = false;  // Set flag to false so the TwitchCoroutine can stop
            StopCoroutine(vibrationCoroutine);  // Stop the running coroutine
            vibrationCoroutine = null;
            Debug.Log("Vibration coroutine stopped.");
        }
    }

    IEnumerator TwitchCoroutine()
    {
        while (isVibrationActive)
        {
            yield return new WaitForSeconds(1f / twitchFrequency); // Control twitch frequency
            isTwitching = true; // Signal twitching event
            yield return new WaitForFixedUpdate(); // Apply force on fixed update cycle
            isTwitching = false;
        }
    }

    void FixedUpdate()
    {
        if (MainController != null && MainController.simState != null && MainController.whiskerSim != null && isTwitching)
        {
            SimState simState = MainController.simState;
            float offset = simState.vibrationAmplitude * Mathf.Sin(simState.vibrationSpeed * (Time.time - t));
            float velocityTolerance = 1.0f; // Tolerance for zero velocity check

            foreach (GameObject whisker in MainController.whiskerSim.whiskers)
            {
                if (whisker != null)
                {
                    Rigidbody rb = whisker.GetComponent<Rigidbody>();
                    if (rb != null && Mathf.Abs(rb.velocity.y) < velocityTolerance)
                    {
                        Vector3 twitchForce = new Vector3(offset, 0, 0);
                        rb.AddForce(twitchForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}