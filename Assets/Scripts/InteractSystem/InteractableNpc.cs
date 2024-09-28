using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DialogueSystem.Models;
using UnityEngine;

public class InteractableNpc : InteractableObject
{
    [SerializeField]
    private TextAsset DialogueFile; // The dialogue file to be used for this NPC

    private bool isPlayerInRange = true;

    public override void Interact()
    {
        TriggerDialogue();
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(InteractPrompt))
            InteractPrompt = "Press E to talk.";
    }

    private void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(DialogueFile);
    }
}
