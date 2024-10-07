
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

  
}
