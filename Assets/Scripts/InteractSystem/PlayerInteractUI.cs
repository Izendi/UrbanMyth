using System;
using System.Diagnostics;
using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.Events;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerInteractUI : MonoBehaviour, 
    IEventHandler<InRangeOfLiftableObjectEvent>,
    IEventHandler<LiftableObjectEvent>, 
    IEventHandler<NoObjectToInteractWithEvent>, 
    IEventHandler<InRangeOfDoorButton>,
    IEventHandler<InRangeOfLoadDoorEvent>,
    IEventHandler<InRangeOfNpcEvent>,
    IEventHandler<DialogueInitiatedEvent>,
    IEventHandler<DialogueEndedEvent>
{

    [SerializeField]
    private GameObject PlayerInteractPrompt;

    [SerializeField]
    private PlayerInteract playerInteract;

    [SerializeField]
    private TextMeshProUGUI interactText;

    private CurrentStatus _currentStatus;

    private CurrentStatus currentStatus
    {
        get
        {
            return _currentStatus;
        }

        set
        {
            if (value == _currentStatus)
                return;

            _currentStatus = value;
            UpdatePrompt();
        }
    }

    private static string PRESS_E { get; } = "Press E";
    private static string LIFT_PROMPT { get; } = $"{PRESS_E} to lift.";
    private static string DROP_PROMPT { get; } = $"{PRESS_E} to drop.";
    private static string PUSH_TO_OPEN_PROMPT { get; } = $"{PRESS_E} to push.";
    private static string LOAD_DOOR_PROMPT { get; } = $"{PRESS_E} to proceed to next level.";
    private static string TALK_PROMPT { get; } = $"{PRESS_E} to talk.";

    private void Start()
    {
        EventAggregator.Instance.Subscribe<InRangeOfLiftableObjectEvent>(this);
        EventAggregator.Instance.Subscribe<LiftableObjectEvent>(this);
        EventAggregator.Instance.Subscribe<NoObjectToInteractWithEvent>(this);
        EventAggregator.Instance.Subscribe<InRangeOfDoorButton>(this);
        EventAggregator.Instance.Subscribe<InRangeOfLoadDoorEvent>(this);
        EventAggregator.Instance.Subscribe<InRangeOfNpcEvent>(this);
        EventAggregator.Instance.Subscribe<DialogueInitiatedEvent>(this);
        EventAggregator.Instance.Subscribe<DialogueEndedEvent>(this);
        Hide();
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
    }

    private void OnDestroy()
    {
        EventAggregator.Instance.Unsubscribe<InRangeOfLiftableObjectEvent>(this);
        EventAggregator.Instance.Unsubscribe<LiftableObjectEvent>(this);
        EventAggregator.Instance.Unsubscribe<NoObjectToInteractWithEvent>(this);
        EventAggregator.Instance.Unsubscribe<InRangeOfDoorButton>(this);
        EventAggregator.Instance.Unsubscribe<InRangeOfLoadDoorEvent>(this);
        EventAggregator.Instance.Unsubscribe<InRangeOfNpcEvent>(this);
        EventAggregator.Instance.Unsubscribe<DialogueInitiatedEvent>(this);
        EventAggregator.Instance.Unsubscribe<DialogueEndedEvent>(this);
        playerInteract = null;
    }

    private void Show()
    {
        PlayerInteractPrompt?.SetActive(true);
    }
    private void Hide()
    {
        Debug.Log($"Hide: {PlayerInteractPrompt}");
        PlayerInteractPrompt?.SetActive(false);
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

    public void Handle(InRangeOfDoorButton @event)
    {
        if (currentStatus != CurrentStatus.HoldingObject)
            currentStatus = CurrentStatus.InRangeOfDoorButton;
    }

    public void Handle(InRangeOfLoadDoorEvent @event)
    {
        if (currentStatus != CurrentStatus.HoldingObject)
            currentStatus = CurrentStatus.InRangeOfLoadDoor;
    }
    public void Handle(InRangeOfNpcEvent @event)
    {
        if (currentStatus != CurrentStatus.HoldingObject)
            currentStatus = CurrentStatus.InRangeOfNpc;
    }

    public void Handle(DialogueInitiatedEvent @event)
    {
        currentStatus = CurrentStatus.ActiveDialogue;
    }

    public void Handle(DialogueEndedEvent @event)
    {
        currentStatus = CurrentStatus.Undefined;
    }

    private string GetInteractPrompt()
    {
        switch (currentStatus)
        {
            case CurrentStatus.HoldingObject:
                return DROP_PROMPT;
            case CurrentStatus.InRangeOfLiftableObject:
                return LIFT_PROMPT;
            case CurrentStatus.InRangeOfDoorButton:
                return PUSH_TO_OPEN_PROMPT;
            case CurrentStatus.InRangeOfLoadDoor:
                return LOAD_DOOR_PROMPT;
            case CurrentStatus.InRangeOfNpc:
                return TALK_PROMPT;
            default:
                return string.Empty;
        }
    }

    private void UpdatePrompt()
    {
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
    }
}
internal enum CurrentStatus
{
    Undefined,
    InRangeOfLiftableObject,
    HoldingObject,
    InRangeOfDoorButton,
    InRangeOfLoadDoor,
    InRangeOfNpc,
    ActiveDialogue
}
