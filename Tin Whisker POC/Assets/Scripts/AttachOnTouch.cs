using UnityEngine;

public class AttachOnTouch : MonoBehaviour
{
    public LayerMask attachableLayer; // Set this to "AttachableObject" layer in the inspector
    private int contactPoints = 0;
    private FixedJoint fixedJoint;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is on the "AttachableObject" layer
        if (((1 << collision.gameObject.layer) & attachableLayer) != 0)
        {
            contactPoints++;

            if (contactPoints == 1)
            {
                AttachFirstPoint(collision);
            }
            
        }
    }

    private void AttachFirstPoint(Collision collision)
    {
        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = collision.rigidbody;
    }

    private void AttachToSecondObject(Collision collision)
    {
        // Adjust the connection so that the object now follows the second object it collided with
        fixedJoint.connectedBody = collision.rigidbody;
    }
}
