using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    [SerializeField]
    private AudioClip movementSound;

    public Transform drawer; // Reference to the drawer object
    //public Transform objectInside; // Reference to the object inside (e.g., paper)
    public Vector3 openPosition; // The target position when the drawer is open
    public Vector3 closedPosition; // The initial closed position of the drawer
    public float speed = 2f; // Speed of drawer opening and closing
    public KeyCode interactKey = KeyCode.F; // The key to interact with the drawer
    public TMP_Text openDrawerText; // Text to show when player is near
    public float slideDistance = 0.3f; // Distance the drawer will slide

    private bool isOpen = false; // Check if the drawer is open
    private bool isPlayerNear = false; // Check if the player is near the drawer
    public bool activated = false; // For activation check
    // Start is called before the first frame update
    void Start()
    {
        closedPosition = drawer.localPosition;
        openPosition = closedPosition - drawer.right * slideDistance; // Open the drawer by sliding it along its right side
        openDrawerText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(interactKey))
        {
            SoundManager.instance.PlaySoundEffect(movementSound, transform, 1.0f);
            isOpen = !isOpen; // Toggle drawer state
        }

        // Smoothly move the drawer between open and closed positions
        if (isOpen)
        {
            drawer.localPosition = Vector3.MoveTowards(drawer.localPosition, openPosition, Time.deltaTime * speed);
            // Move the object inside along with the drawer
            //objectInside.localPosition = Vector3.MoveTowards(objectInside.localPosition, openPosition, Time.deltaTime * speed);
        }
        else
        {
            drawer.localPosition = Vector3.MoveTowards(drawer.localPosition, closedPosition, Time.deltaTime * speed);
            // Move the object inside back to its original position
            //objectInside.localPosition = Vector3.MoveTowards(objectInside.localPosition, closedPosition, Time.deltaTime * speed);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            openDrawerText.enabled = true; // Show the "Press F to open drawer" text
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            openDrawerText.enabled = false; // Hide the text when player leaves
        }
    }
}
