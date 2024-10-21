using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.Events;
using Assets.Scripts.InteractSystem;
using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour, 
    IEventHandler<LiftableObjectEvent>, 
    IEventHandler<DialogueInitiatedEvent>,
    IEventHandler<DialogueEndedEvent>
{

    [SerializeField]
    private GameObject PlayerInteractPrompt;

    [SerializeField]
    private PlayerInteract playerInteract;

    [SerializeField]
    private TextMeshProUGUI interactText;

    private PlayerInteractUIState _currentStatus;

    private PlayerInteractUIState currentStatus
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
    private static string PRESS_F { get; } = "Press F";
    private static string LIFT_PROMPT { get; } = $"{PRESS_E} to lift.";
    private static string DROP_PROMPT { get; } = $"{PRESS_E} to drop.";
    private static string PUSH_PROMPT { get; } = $"{PRESS_E} to push.";
    private static string LOAD_DOOR_PROMPT { get; } = $"{PRESS_E} to proceed to next level.";
    private static string TALK_PROMPT { get; } = $"{PRESS_E} to talk.";
    private static string OPERATE_PROMPT { get; } = $"{PRESS_F} to operate.";

    private void Start()
    {
        EventAggregator.Instance.Subscribe<LiftableObjectEvent>(this);
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

        if (!(currentStatus == PlayerInteractUIState.HoldingObject || currentStatus == PlayerInteractUIState.ActiveDialogue))
        {
            var type = playerInteract.GetCurrentInteractableType();
            if (type != currentStatus)
                currentStatus = type;
        }
    }

    private void OnDestroy()
    {
        EventAggregator.Instance.Unsubscribe<LiftableObjectEvent>(this);
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
        PlayerInteractPrompt?.SetActive(false);
    }

    public void Handle(LiftableObjectEvent @event)
    {
        if (@event.ObjectLifted)
            currentStatus = PlayerInteractUIState.HoldingObject;
        else
            currentStatus = PlayerInteractUIState.Undefined;
    }

    public void Handle(DialogueInitiatedEvent @event)
    {
        currentStatus = PlayerInteractUIState.ActiveDialogue;
    }

    public void Handle(DialogueEndedEvent @event)
    {
        currentStatus = PlayerInteractUIState.Undefined;
    }

    private string GetInteractPrompt()
    {
        switch (currentStatus)
        {
            case PlayerInteractUIState.HoldingObject:
                return DROP_PROMPT;
            case PlayerInteractUIState.InRangeOfLiftableObject:
                return LIFT_PROMPT;
            case PlayerInteractUIState.InRangeOfDoor:
                return OPERATE_PROMPT;
            case PlayerInteractUIState.InRangeOfDoorButton:
                return PUSH_PROMPT;
            case PlayerInteractUIState.InRangeOfLoadDoor:
                return LOAD_DOOR_PROMPT;
            case PlayerInteractUIState.InRangeOfNpc:
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
