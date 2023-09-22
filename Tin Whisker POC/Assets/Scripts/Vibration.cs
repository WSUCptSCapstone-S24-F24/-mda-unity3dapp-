using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vibration : MonoBehaviour
{
    public GameObject Board;

    public float amplitude = 1.0f;      // Amplitude of the sine wave.
    public float speed = 2.0f;  // Speed of the movement.
    private Vector3 initialPosition;     // Initial position of the GameObject.
    private bool status = false;

    public void Start()
    {
        Board = GameObject.FindWithTag("Board");
        initialPosition = Board.transform.position; // Store the initial position of the GameObject.

        if (status)
            status = false;
        else
            status = true;
    }

    void Update()
    {
        float verticalPosition = initialPosition.y + amplitude * Mathf.Sin(speed * Time.time);

        if (status)
        {
            Board.transform.position = Vector3.MoveTowards(Board.transform.position, new Vector3(0f, verticalPosition, 0f), 1.0f);
        }
    }
}
