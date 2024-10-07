using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimInfo;

public class Shock : MonoBehaviour
{
    private MainController MainController;
    private bool isShocking = false;
    private Coroutine shockCoroutine; // Track the running coroutine

    public void Start()
    {
        // Only initialize if shock is supposed to start automatically
        // You can remove this if you want to control shock manually
    }

    public void StartShock()
    {
        if (shockCoroutine == null)  // Ensure only one coroutine runs
        {
            shockCoroutine = StartCoroutine(InitializeShock());
        }
    }

    public IEnumerator InitializeShock()
    {
        // Wait until MainController and WhiskerSim are available
        while (MainController == null || MainController.whiskerSim == null)
        {
            MainController = FindObjectOfType<MainController>();
            if (MainController == null)
            {
                Debug.LogError("MainController not found in the scene.");
                yield break; // Exit if MainController is not found
            }

            if (MainController.whiskerSim == null)
            {
                Debug.LogError("WhiskerSim not found in the scene.");
                yield break; // Exit if WhiskerSim is not found
            }

            yield return null; // Wait for the next frame
        }

        // Once both are available, start the ShockCoroutine
        shockCoroutine = StartCoroutine(ShockCoroutine());
    }

    public void StopShock()
    {
        if (shockCoroutine != null)
        {
            isShocking = false; // Stop the shock logic
            StopCoroutine(shockCoroutine); // Stop the coroutine
            shockCoroutine = null; // Clear the coroutine reference
            Debug.Log("Shock coroutine stopped.");
        }
    }

    IEnumerator ShockCoroutine()
    {
        while (true)  // Run indefinitely until stopped
        {
            yield return new WaitForSeconds(MainController.simState.ShockDuration);
            isShocking = true;  // Set flag to apply shock
            yield return new WaitForFixedUpdate(); // Wait for one physics frame
            isShocking = false; // Reset flag after one update
        }
    }

    void FixedUpdate()
    {
        if (MainController != null && MainController.simState != null && MainController.whiskerSim != null && isShocking)
        {
            SimState simState = MainController.simState;
            float shockForce = simState.ShockIntensity;
            float velocityTolerance = 1.0f; // Tolerance for zero velocity check
            float varianceAmount = 5f; // Small variance for x and z

            // Generate random variance for x and z once per shock interval
            float randomX = Random.Range(-varianceAmount, varianceAmount);
            float randomZ = Random.Range(-varianceAmount, varianceAmount);

            // Apply shock force to all whiskers
            foreach (GameObject whisker in MainController.whiskerSim.whiskers)
            {
                if (whisker != null)
                {
                    Rigidbody rb = whisker.GetComponent<Rigidbody>();
                    if (rb != null && Mathf.Abs(rb.velocity.y) < velocityTolerance)
                    {
                        Vector3 shockImpact = new Vector3(randomX, shockForce, randomZ); // Use same randomX and randomZ for all whiskers
                        rb.AddForce(shockImpact, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
