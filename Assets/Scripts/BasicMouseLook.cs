using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    //[SerializeField]
    //private InputActionAsset pointerPosition;

    float xRotation = 0f;

    private bool registerMouse = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor

        
    }

    void Update()
    {
        // Check if dialogue is active
        if (DialogueManager.Instance?.IsDialogueActive ?? false)
        {
            LockCursor(false); // Unlock the cursor if dialogue is active
            return; // Skip the mouse look logic
        }
        else
        {
            LockCursor(true); // Lock the cursor when not in dialogue
        }

        if (Input.GetKeyDown(KeyCode.P) || GlobalStateManager.Instance.isPlayerDead)
        {
            registerMouse = false;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            registerMouse = true;
        }

        if (registerMouse == false)
        {

        }
        else
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

    private void LockCursor(bool isLocked)
    {
        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.Locked; // Locks the cursor
            Cursor.visible = false; // Hide the cursor
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
            Cursor.visible = true; // Show the cursor
        }
    }
}
