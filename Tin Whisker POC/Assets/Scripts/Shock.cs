using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimInfo;

public class Shock : MonoBehaviour
{
    private MainController MainController;
    private WhiskerSim whiskerSim;
    private bool isShocking = false;

    public void Start()
    {
        //StartCoroutine(InitializeShock());
    }

    public IEnumerator InitializeShock()
    {
        // Wait until MainController and WhiskerSim are available
        while (MainController == null || whiskerSim == null)
        {
            MainController = FindObjectOfType<MainController>();
            whiskerSim = FindObjectOfType<WhiskerSim>();

            if (MainController == null)
            {
                Debug.LogError("MainController not found in the scene.");
            }

            if (whiskerSim == null)
            {
                Debug.LogError("WhiskerSim not found in the scene.");
            }

            yield return null; // Wait for the next frame
        }

        // Once both are available, start the ShockCoroutine
        StartCoroutine(ShockCoroutine());
    }

    IEnumerator ShockCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(MainController.simState.ShockDuration);
            isShocking = true;
            yield return new WaitForFixedUpdate();
            isShocking = false;
        }
    }

    void FixedUpdate()
    {
        if (MainController != null && MainController.simState != null && whiskerSim != null && isShocking)
        {
            SimState simState = MainController.simState;
            float shockForce = simState.ShockIntensity;
            float velocityTolerance = 1.0f; // Tolerance for zero velocity check
            float varianceAmount = 5f; // Small variance for x and z

            // Generate random variance for x and z once per shock interval
            float randomX = Random.Range(-varianceAmount, varianceAmount);
            float randomZ = Random.Range(-varianceAmount, varianceAmount);

            foreach (GameObject whisker in whiskerSim.whiskers)
            {
                if (whisker != null)
                {
                    Rigidbody rb = whisker.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        // Check if the velocity is approximately zero (within the tolerance)
                        if (Mathf.Abs(rb.velocity.y) < velocityTolerance)
                        {
                            Vector3 shockImpact = new Vector3(randomX, shockForce, randomZ); // Use same randomX and randomZ for all whiskers
                            rb.AddForce(shockImpact, ForceMode.Impulse);
                        }
                    }
                }
            }
        }
    }
}
