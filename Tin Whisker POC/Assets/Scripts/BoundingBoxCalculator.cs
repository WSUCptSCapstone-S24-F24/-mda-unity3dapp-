using UnityEngine;

public class BoundingBoxCalculator
{
    public static Vector3 CalculateTotalBounds(GameObject gameObject)
    {
        Bounds combinedBounds = new Bounds(gameObject.transform.position, Vector3.zero);
        
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }

        return combinedBounds.size;
    }
}
