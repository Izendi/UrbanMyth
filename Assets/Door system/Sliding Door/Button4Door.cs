using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Button4Door : MonoBehaviour
{

    public ButtonunlockDoor doorScript; // Reference to the door script
    public KeyCode interactKey = KeyCode.F; // Key to press the button
    public TMP_Text ButtoninteractText;
    private bool isPlayerNearButton = false;
    // Start is called before the first frame update
    void Start()
    {
        ButtoninteractText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNearButton && Input.GetKeyDown(interactKey))
        {
            doorScript.isDoorUnlocked = true; // Unlock the door
            Debug.Log("Door unlocked!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearButton = true;
            ButtoninteractText.enabled=true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearButton = false;
            ButtoninteractText.enabled=false;
        }
    }
}
