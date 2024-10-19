using System;
using System.Diagnostics;
using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.Events;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerInteractUI : MonoBehaviour, IEventHandler<InRangeOfLiftableObjectEvent>, IEventHandler<LiftableObjectEvent>, IEventHandler<NoObjectToInteractWithEvent>
{
    


    [SerializeField]
    private GameObject PlayerInteractPrompt;

    [SerializeField]
    private PlayerInteract playerInteract;

    [SerializeField]
    private TextMeshProUGUI interactText;

    CurrentStatus currentStatus = CurrentStatus.Undefined;

    private static string PRESS_E { get; } = "Press E";
    private static string LIFT_PROMPT { get; } = $"{PRESS_E} to lift";
    private static string DROP_PROMPT { get; } = $"{PRESS_E} to drop";


    private void Start()
    {
        EventAggregator.Instance.Subscribe<InRangeOfLiftableObjectEvent>(this);
        EventAggregator.Instance.Subscribe<LiftableObjectEvent>(this);
        EventAggregator.Instance.Subscribe<NoObjectToInteractWithEvent>(this);
    }

    private void Update()
    {
        if (playerInteract == null)
        {
            playerInteract = FindObjectOfType<PlayerInteract>(); // Reassign the reference
            if (playerInteract == null)
            {
                Debug.LogWarning("PlayerInteract object not found in the scene.");
                return; // Exit early if playerInteract is still null
            }
        }

        var interactableObject = playerInteract.GetInteractableObject();


        var interactPrompt = GetInteractPrompt();

        if (!string.IsNullOrEmpty(interactPrompt))
        {
            Show();
            interactText.text = interactPrompt;
        }
        else
        {
            Hide();
        }

        //if (interactableObject is null || DialogueManager.IsDialogueActive)
        //{
        //    Hide();
        //}
        //else
        //{
        //    Show();
        //    interactText.text = interactableObject.InteractPrompt;
        //}
            
    }

    private void Show()
    {
        PlayerInteractPrompt.SetActive(true);
    }
    private void Hide()
    {
        PlayerInteractPrompt.SetActive(false);
    }

    public void Handle(InRangeOfLiftableObjectEvent @event)
    {
        if (currentStatus != CurrentStatus.HoldingObject)
        {
            currentStatus = CurrentStatus.InRangeOfLiftableObject;
        }
    }

    public void Handle(LiftableObjectEvent @event)
    {
        if (@event.ObjectLifted)
        {
            currentStatus = CurrentStatus.HoldingObject;
        }
        else
        {
            Debug.Log("drop object");
            currentStatus = CurrentStatus.Undefined;
        }
    }
    public void Handle(NoObjectToInteractWithEvent @event)
    {
        if (currentStatus != CurrentStatus.HoldingObject)
        {
            currentStatus = CurrentStatus.Undefined;
        }
    }

    private string GetInteractPrompt()
    {
        switch (currentStatus)
        {
            case CurrentStatus.HoldingObject:
                return DROP_PROMPT;
            case CurrentStatus.InRangeOfLiftableObject:
                return LIFT_PROMPT;
            default:
                return string.Empty;
        }
    }

}
internal enum CurrentStatus
{
    Undefined,
    InRangeOfLiftableObject,
    HoldingObject
}
