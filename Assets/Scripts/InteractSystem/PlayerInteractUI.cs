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
