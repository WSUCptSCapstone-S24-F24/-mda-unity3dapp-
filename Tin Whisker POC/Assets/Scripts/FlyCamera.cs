using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float moveSpeed = 2f;   // Adjust the speed as needed
    public float sensitivity = 2f; // Adjust the sensitivity of the mouse look
    public float verticalSpeed = 3f; // Adjust the speed of vertical movement

    void Update()
    {
        // Check if the right mouse button is pressed
        if (Input.GetMouseButtonDown(1)) // 1 corresponds to the right mouse button
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when right-click is pressed
            Cursor.visible = false; // Hide the cursor
        }

        // Check if the right mouse button is released
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor when right-click is released
            Cursor.visible = true; // Show the cursor
        }

        // Check if the right mouse button is held down
        if (Input.GetMouseButton(1)) // 1 corresponds to the right mouse button
        {
            // Get input for movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Calculate the movement direction
            Vector3 movement = new Vector3(horizontal, 0f, vertical) * moveSpeed * Time.deltaTime;

            // Apply the movement to the camera's position
            transform.Translate(movement);

            // Get input for mouse look
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Calculate rotation based on mouse input
            float rotationX = transform.localEulerAngles.y + mouseX * sensitivity;
            float rotationY = transform.localEulerAngles.x - mouseY * sensitivity;

            // // Clamp the vertical rotation to limit looking up and down
            // float rotationY = Mathf.Clamp(transform.localEulerAngles.x - mouseY * sensitivity, -90f, 90f);

            // Apply the rotation to the camera
            transform.localEulerAngles = new Vector3(rotationY, rotationX, 0f);
        }

        // Check if Space is pressed to move the camera up
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime);
        }

        // Check if Control is pressed to move the camera down
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime);
        }
    }
}
