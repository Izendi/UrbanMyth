using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerInteractPrompt;

    [SerializeField]
    private PlayerInteract playerInteract;

    [SerializeField]
    private TextMeshProUGUI interactText;

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
}
