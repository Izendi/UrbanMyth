using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenDrawer : MonoBehaviour
{
    public Animator drawerAnimator; // Reference to the Animator component
    public TMP_Text openDrawerText; // Reference to the TextMeshPro UI Text
    private bool isPlayerNear = false; // To check if the player is near the drawer
    private bool isOpen = false; // To check if the drawer is open

    public KeyCode interactKey = KeyCode.E; // Key to interact with the drawer

    void Start()
    {
        openDrawerText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(interactKey))
        {
            isOpen = !isOpen; // Toggle the drawer's state
            drawerAnimator.SetBool("Open", isOpen);
            drawerAnimator.SetBool("Close", !isOpen);// Set the Animator parameter to trigger animation
        }
    }

    // Detect when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Only trigger if the player enters
        {
            isPlayerNear = true;
            openDrawerText.enabled = true; // Show the "Press E to open drawer" text
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Only trigger if the player exits
        {
            isPlayerNear = false;
            openDrawerText.enabled = false; // Hide the text
        }
    }

}
