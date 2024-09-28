using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class opendoor : MonoBehaviour
{
    public Animator doorAnimator;
    public Transform doorPivot; // Reference to the pivot point
    //public float openAngle = 90f; // How far the door opens
    //public float speed = 2f; // Speed of door opening
    public KeyCode interactKey = KeyCode.E; // The key to interact with the door

    public TMP_Text openDoorText;
    private bool isOpen = false;
    private bool isPlayerNear = false; // Check if player is near the door
    //private Quaternion closedRotation; // Initial rotation
    //private Quaternion openRotation; // Target rotation when open
    // Start is called before the first frame update
    void Start()
    {
        //closedRotation = doorPivot.rotation;
        //openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
        openDoorText.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(interactKey))
        {
            isOpen = !isOpen;
            doorAnimator.SetBool("Open", isOpen);
            doorAnimator.SetBool("Close", !isOpen);
        }
       

        //// Smoothly rotate the door between open and closed positions
        //if (isOpen)
        //{
        //    doorPivot.rotation = Quaternion.Slerp(doorPivot.rotation, openRotation, Time.deltaTime * speed);
        //}
        //else
        //{
        //    doorPivot.rotation = Quaternion.Slerp(doorPivot.rotation, closedRotation, Time.deltaTime * speed);
        //}
    }

    // Detect when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Object enter trigger");
        if (other.CompareTag("Player"))
        {
           // Debug.Log("Object enter player");
            isPlayerNear = true;
            openDoorText.enabled = true; // Show the "Press E to open door" text
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            openDoorText.enabled = false; // Hide the text
        }
    }

}
