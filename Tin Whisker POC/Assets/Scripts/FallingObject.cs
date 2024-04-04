using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float gravityScale = 1.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Set initial velocity to simulate falling
        rb.velocity = new Vector3(0, -1, 0);
    }

    void FixedUpdate()
    {
        // Apply gravity to accelerate the object
        rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }
}
