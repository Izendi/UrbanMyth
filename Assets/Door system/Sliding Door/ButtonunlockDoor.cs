using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;

public class ButtonunlockDoor : MonoBehaviour
{
    public Transform door; // Reference to the door object
    public Vector3 openPosition; // The target position when the door is open
    public Vector3 closedPosition; // The initial closed position of the door
    public float speed = 2f; // Speed of door opening and closing
    public KeyCode interactKey = KeyCode.F; // The key to interact with the door

    private bool isOpen = false; // Check if the door is open
    private bool isPlayerNear = false; // Check if the player is near the door
    public TMP_Text interactionText; // Text to show when player is near
    public bool isDoorUnlocked = false; // Check if player has the key
    // Start is called before the first frame update
    void Start()
    {
        closedPosition = door.localPosition;
        openPosition = closedPosition - new Vector3(1f, 0, 0); // Slides 1 unit to the right
        interactionText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isPlayerNear && Input.GetKeyDown(interactKey) && isDoorUnlocked)
        {
            isOpen = !isOpen; // Toggle the door state
        }

        // Smoothly move the door between open and closed positions
        if (isOpen)
        {
            door.localPosition = Vector3.MoveTowards(door.localPosition, openPosition, Time.deltaTime * speed);
        }
        else
        {
            door.localPosition = Vector3.MoveTowards(door.localPosition, closedPosition, Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;

            if (isDoorUnlocked)
            {
                interactionText.text = "Press F to open the door"; // Show the interaction text if the player has the key
            }
            else
            {
                interactionText.text = "You need a key to open the door"; // Show the "need key" text if the player doesn't have the key
            }

            interactionText.enabled = true; // Enable the text
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactionText.enabled = false; // Hide the text when player leaves
        }
    }
}
