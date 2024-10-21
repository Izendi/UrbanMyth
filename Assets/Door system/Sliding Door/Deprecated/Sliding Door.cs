using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;

public class SlidingDoor: MonoBehaviour
{

    public Animator doorAnimator;  // Animator controlling the door
    public KeyCode interactKey = KeyCode.E;  // Interaction key

    private bool isPlayerNear = false;  // Track if the player is near the door
    private bool isOpen = false;  // Track door state
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(interactKey))
        {
            
                // Close the door
            isOpen = !isOpen;
            doorAnimator.SetBool("Open", isOpen);
            doorAnimator.SetBool("Close", !isOpen);
               
            
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Only trigger if the player enters
        {
            isPlayerNear = true;
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Only trigger if the player exits
        {
            isPlayerNear = false;
        }
    }
}
