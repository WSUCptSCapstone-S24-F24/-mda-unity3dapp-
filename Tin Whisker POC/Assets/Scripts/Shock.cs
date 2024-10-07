using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimInfo;

public class Shock : MonoBehaviour
{
    private MainController MainController;
    private bool isShocking = false; // Determines whether shocking is enabled
    private float shockTimer = 0f;   // Tracks the time left until the next shock
    private float shockDuration = 0f; // Caches the ShockDuration from SimState

    void Start()
    {
        // Initialize when needed; no automatic start
    }

    public void StartShock()
    {
        if (MainController == null)
        {
            MainController = FindObjectOfType<MainController>();
            if (MainController == null || MainController.simState == null)
            {
                Debug.LogError("MainController or SimState not found. Shock cannot start.");
                return;
            }
        }

        shockDuration = MainController.simState.ShockDuration; // Set shock duration from SimState
        shockTimer = shockDuration; // Start the timer at the ShockDuration value
        isShocking = true; // Enable shocking
        Debug.Log("Shock started with a duration of: " + shockDuration);
    }

    public void StopShock()
    {
        isShocking = false; // Stop the shock logic
        Debug.Log("Shock stopped.");
    }

    void FixedUpdate()
    {
        if (isShocking && MainController != null && MainController.simState != null && MainController.whiskerSim != null)
        {
            // Reduce the shock timer by the time since the last fixed update
            shockTimer -= Time.fixedDeltaTime;

            // When the timer reaches zero, apply the shock
            if (shockTimer <= 0f)
            {
                ApplyShock(); // Apply the shock force to whiskers

                // Reset the timer for the next shock
                shockTimer = shockDuration;
            }
        }
    }

    private void ApplyShock()
    {
        SimState simState = MainController.simState;
        float shockForce = simState.ShockIntensity;
        float velocityTolerance = 1.0f; // Tolerance for zero velocity check
        float varianceAmount = 5f; // Small variance for x and z

        Debug.Log("Applying shock...");

        // Generate random variance for x and z components ONCE for all whiskers
        float randomX = Random.Range(-varianceAmount, varianceAmount);
        float randomZ = Random.Range(-varianceAmount, varianceAmount);

        // Apply shock force to all whiskers in the same direction
        foreach (GameObject whisker in MainController.whiskerSim.whiskers)
        {
            if (whisker != null)
            {
                Rigidbody rb = whisker.GetComponent<Rigidbody>();
                if (rb != null && Mathf.Abs(rb.velocity.y) < velocityTolerance)
                {
                    Vector3 shockImpact = new Vector3(randomX, shockForce, randomZ);

                    // Apply the same shock force to all whiskers
                    rb.AddForce(shockImpact, ForceMode.Impulse);
                }
            }
        }
    }
}