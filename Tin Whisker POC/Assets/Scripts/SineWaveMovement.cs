// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SineWaveMovement : MonoBehaviour
// {
//     public float speed = 5.0f;        // Speed of the movement
//     public float magnitude = 0.10f;    // Amplitude of the sine wave (how high and low the object goes)

//     private Vector3 startPosition;

//     void Start()
//     {
//         startPosition = transform.position;
//     }

//     void Update()
//     {
//         float newYPosition = startPosition.y + Mathf.Sin(Time.time * speed) * magnitude;
//         transform.position = new Vector3(startPosition.x, newYPosition, startPosition.z);
//     }
// }



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SineWaveMovement : MonoBehaviour
{
    public float speed = 5.0f;        // Speed of the movement
    public float magnitude = 0.10f;    // Amplitude of the sine wave (how high and low the object goes)

    private Vector3 startPosition;
    private Rigidbody rb;

    public void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    //void FixedUpdate()
    //{
    //    if(status)
    //    {
    //        float newYPosition = startPosition.y + Mathf.Sin(Time.time * speed) * magnitude;
    //        Vector3 newPos = new Vector3(startPosition.x, newYPosition, startPosition.z);

    //        rb.MovePosition(newPos);
    //    }
    //}
}
