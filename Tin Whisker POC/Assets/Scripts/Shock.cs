using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : MonoBehaviour
{
    private float intensity = 0.05f; // How high the object moves
    private float duration = 0.025f; // Total duration of the effect (up and down)

    // Public method to start the shock effect
    public void StartShock()
    {
        GameObject boardObject = GameObject.FindGameObjectWithTag("Board");
        if (boardObject != null)
        {
            StartCoroutine(iShock(boardObject));
        }
    }

    IEnumerator iShock(GameObject target)
    {
        Vector3 originalPosition = target.transform.position;
        Vector3 targetPosition = originalPosition + Vector3.up * intensity; // Move up

        // First half of the duration: move up
        float elapsedTime = 0;
        while (elapsedTime < duration / 2)
        {
            target.transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Second half of the duration: move down
        elapsedTime = 0;
        while (elapsedTime < duration / 2)
        {
            target.transform.position = Vector3.Lerp(targetPosition, originalPosition, (elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object is exactly at its original position after the shock
        target.transform.position = originalPosition;
    }
}
