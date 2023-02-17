using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float speed = 10f;
    public float mouseSensitivity = 1f;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float upDown = Input.GetAxis("UpDown");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.position += transform.right * horizontal * speed * Time.deltaTime;
        transform.position += transform.forward * vertical * speed * Time.deltaTime;
        transform.position += transform.up * upDown * speed * Time.deltaTime;

        transform.eulerAngles += new Vector3(-mouseY * mouseSensitivity, mouseX * mouseSensitivity, 0f);
    }
}
