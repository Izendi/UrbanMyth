using Assets.Scripts;
using Assets.Scripts.Events;
using System.Linq;
using Assets.Scripts.InteractSystem;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private const float INTERACT_DISTANCE = 2.0f;
    private const float OFFSET_DISTANCE = 1.5f;
    private const float TRANSITION_SPEED = 2f; // Speed of the transition
    
    [SerializeField] private Camera PlayerCamera;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Quaternion resetRotation;
    private Vector3 resetPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray, INTERACT_DISTANCE);

            if (hits.Any(h => h.transform.tag == "NPC"))
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, INTERACT_DISTANCE);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.TryGetComponent(out InteractableNpc dialogueTrigger) && !DialogueManager.IsDialogueActive)
                    {
                        dialogueTrigger.Interact();
                    }
                }
            }
        }
    }

    public PlayerInteractUIState GetCurrentInteractableType()
    {
        Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(ray, INTERACT_DISTANCE);

        if (hits.Any(h => h.transform.tag == "PickUpAble"))
        {
            return PlayerInteractUIState.InRangeOfLiftableObject;
        }

        if (hits.Any(h => h.transform.tag == "Button"))
        {
            return PlayerInteractUIState.InRangeOfDoorButton;
        }

        if (hits.Any(h => h.transform.tag == "LoadDoor"))
        {
            return PlayerInteractUIState.InRangeOfLoadDoor;
        }

        if (hits.Any(h => h.transform.tag == "NPC"))
        {
            return PlayerInteractUIState.InRangeOfNpc;
        }

        if (hits.Any(h => h.transform.tag == "Openable"))
        {
            return PlayerInteractUIState.InRangeOfDoor;
        }

        if (hits.Any(h => h.transform.tag == "CollectibleItem"))
        {
            return PlayerInteractUIState.CollectibleItem;
        }

        return PlayerInteractUIState.Undefined;
    }
}
