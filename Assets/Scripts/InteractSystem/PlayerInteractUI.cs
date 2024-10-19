using System.Diagnostics;
using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.Events;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerInteractUI : MonoBehaviour, IEventHandler<InRangeOfLiftableObjectEvent>, IEventHandler<ObjectLiftedEvent>
{
    [SerializeField]
    private GameObject PlayerInteractPrompt;

    [SerializeField]
    private PlayerInteract playerInteract;

    [SerializeField]
    private TextMeshProUGUI interactText;

    private void Start()
    {
        EventAggregator.Instance.Subscribe<InRangeOfLiftableObjectEvent>(this);
        EventAggregator.Instance.Subscribe<ObjectLiftedEvent>(this);
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

        if (interactableObject is null || DialogueManager.IsDialogueActive)
        {
            Hide();
        }
        else
        {
            Show();
            interactText.text = interactableObject.InteractPrompt;
        }
            
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
        Debug.Log("press E to lift");
        Show();
        interactText.text = "Press E to lift.";
    }

    public void Handle(ObjectLiftedEvent @event)
    {
        Debug.Log("press E to drop");
        Show();
        interactText.text = "Press E to drop.";
    }
}
