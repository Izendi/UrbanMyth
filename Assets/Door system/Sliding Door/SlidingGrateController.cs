using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlidingGrateController : MonoBehaviour
{
    [SerializeField]
    private AudioClip movementSound;

    public Transform door; // Reference to the door object
    public Vector3 openPosition; // The target position when the door is open
    public Vector3 closedPosition; // The initial closed position of the door
    public float speed = 2f; // Speed of door opening and closing
    public KeyCode interactKey = KeyCode.F; // The key to interact with the door

    private bool isOpen = false; // Check if the door is open
    private bool isPlayerNear = false; // Check if the player is near the door
    public float slideDistance = 1f;
    public bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        closedPosition = door.localPosition;
        // Set the open position (you can adjust this based on how far you want the door to slide)
        //openPosition = closedPosition - new Vector3(1f, 0, 0); // Slides 3 units to the right
        openPosition = closedPosition + door.right * slideDistance; //slide the door to its local right
    }

    // Update is called once per frame
    void Update()
    {

        if (isPlayerNear && Input.GetKeyDown(interactKey))
        {
            SoundManager.instance.PlaySoundEffect(movementSound, transform, 1.0f);
            isOpen = !isOpen; // Toggle door state
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
    // Detect when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OtherPlayer"))
        {
            isPlayerNear = true;
        }
    }

    public void activateDoor()
    {
        activated = true;
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OtherPlayer"))
        {
            isPlayerNear = false;
        }
    }
}


