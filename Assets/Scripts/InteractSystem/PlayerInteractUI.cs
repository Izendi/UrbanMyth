using System;
using System.Diagnostics;
using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.Events;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerInteractUI : MonoBehaviour, IEventHandler<InRangeOfLiftableObjectEvent>, IEventHandler<LiftableObjectEvent>, IEventHandler<NoObjectToInteractWithEvent>, IEventHandler<InRangeOfDoorButton>
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
    private static string LIFT_PROMPT { get; } = $"{PRESS_E} to lift";
    private static string DROP_PROMPT { get; } = $"{PRESS_E} to drop";
    private static string PUSH_TO_OPEN_PROMPT { get; } = $"{PRESS_E} to push";

    private void Start()
    {
        EventAggregator.Instance.Subscribe<InRangeOfLiftableObjectEvent>(this);
        EventAggregator.Instance.Subscribe<LiftableObjectEvent>(this);
        EventAggregator.Instance.Subscribe<NoObjectToInteractWithEvent>(this);
        EventAggregator.Instance.Subscribe<InRangeOfDoorButton>(this);
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

    public void Handle(InRangeOfDoorButton @event)
    {
        if (currentStatus != CurrentStatus.HoldingObject)
            currentStatus = CurrentStatus.InRangeOfDoorButton;
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
            default:
                return string.Empty;
        }
    }

    private void UpdatePrompt()
    {
        Debug.Log("updating prompt");
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
}
