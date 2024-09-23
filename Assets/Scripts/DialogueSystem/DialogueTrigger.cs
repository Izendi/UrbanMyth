using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DialogueSystem.Models;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public TextAsset DialogueFile;

    private bool isPlayerInRange = true;

    public void Update()
    {
        if ( isPlayerInRange && Input.GetKeyDown(KeyCode.T))
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        Debug.Log("interaction started");
    }
}
