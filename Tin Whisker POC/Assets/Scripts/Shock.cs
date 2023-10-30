using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : MonoBehaviour
{
    public GameObject Board;

    public float amplitude;      // Amplitude of the sine wave.
    public float speed;  // Speed of the movement.
    private Vector3 initialPosition;     // Initial position of the GameObject.
    private bool status = true;

    private float t;

    public void Start()
    {
        Board = GameObject.FindWithTag("Board");
        initialPosition = Board.transform.position; // Store the initial position of the GameObject.

        if (status)
            status = false;
        else {
                status = true;
                t = Time.time;
            }
    }

    void Update()
    {
        if (Time.time - t > (speed/(3.14*2))) {
            status = false;
            return;
        }
        
        float verticalPosition = initialPosition.y + amplitude * Mathf.Sin(speed * (Time.time - t));

        if (status)
            Board.transform.position = Vector3.MoveTowards(Board.transform.position, new Vector3(0f, verticalPosition, 0f), 1.0f);
    }
}
