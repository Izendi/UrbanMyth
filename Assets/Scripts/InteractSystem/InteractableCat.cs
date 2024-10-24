using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.DialogueSystem.Models;
using Assets.Scripts.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableCat : InteractableNpc
{

    public override void TriggerDialogue()
    {
        var globalStateManagerObj = GameObject.FindWithTag("GSO");

        if (globalStateManagerObj != null && globalStateManagerObj.TryGetComponent(out GlobalStateManager gsm))
        {
            var startNode = gsm.has_catTreat ? 1 : 101;
            EventAggregator.Instance.Publish(new DialogueInitiatedEvent { Dialogue = base.dialogue, StartNodeId = 101 });
        }
    }   

    public void setDialogueFile(TextAsset df)
    {
        DialogueFile = df;
    }

}
