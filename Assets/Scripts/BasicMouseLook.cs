using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the camera up and down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevents flipping over
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player left and right
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
