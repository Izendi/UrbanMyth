using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DialogueSystem.Models;
using UnityEngine;

public class InteractableNpc : InteractableObject
{

    public TextAsset DialogueFile;

    private bool isPlayerInRange = true;

    private void Start()
    {
        if (string.IsNullOrEmpty(InteractPrompt))
            InteractPrompt = "Press E to talk.";
    }

    public void Update()
    {
        if ( isPlayerInRange && Input.GetKeyDown(KeyCode.T))
        {
            TriggerDialogue();
        }
    }

    public override void Interact()
    {
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(DialogueFile);
    }
}
