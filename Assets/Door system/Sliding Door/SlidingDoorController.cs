using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlidingDoorController : MonoBehaviour
{
    public Transform door; // Reference to the door object
    public Vector3 openPosition; // The target position when the door is open
    public Vector3 closedPosition; // The initial closed position of the door
    public float speed = 2f; // Speed of door opening and closing
    public float slideDistance = 1f;
    private bool isOpen = false; // Check if the door is open
    //private bool isPlayerNear = false; // Check if the player is near the door

    public bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        closedPosition = door.localPosition;
        // Set the open position (you can adjust this based on how far you want the door to slide)
        //openPosition = closedPosition - new Vector3(1f, 0, 0); // Slides 3 units to the right
        openPosition = closedPosition + door.right * slideDistance; //slide the door to its local right
    }

    public void activateDoor()
    {
        activated = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (activated)
        {
            isOpen = !isOpen; // Toggle door state
            activated = false;
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

}
