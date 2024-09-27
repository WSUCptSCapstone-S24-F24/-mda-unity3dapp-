using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimInfo;

public class Shock : MonoBehaviour
{
    private SceneHandler sceneHandler;
    private WhiskerSim whiskerSim;
    private bool isShocking = false;

    public void Start()
    {
        //StartCoroutine(InitializeShock());
    }

    public IEnumerator InitializeShock()
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

        // Once both are available, start the ShockCoroutine
        StartCoroutine(ShockCoroutine());
    }

    IEnumerator ShockCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(sceneHandler.simState.ShockDuration);
            isShocking = true;
            yield return new WaitForFixedUpdate();
            isShocking = false;
        }
    }

    void FixedUpdate()
    {
        if (sceneHandler != null && sceneHandler.simState != null && whiskerSim != null && isShocking)
        {
            SimState simState = sceneHandler.simState;
            float shockForce = simState.ShockIntensity;
            float velocityTolerance = 0.01f; // Tolerance for zero velocity check
            float varianceAmount = 5f; // Small variance for x and z

            // Generate random variance for x and z once per shock interval
            float randomX = Random.Range(-varianceAmount, varianceAmount);
            float randomZ = Random.Range(-varianceAmount, varianceAmount);

            foreach (GameObject whisker in whiskerSim.cylinder_clone)
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
