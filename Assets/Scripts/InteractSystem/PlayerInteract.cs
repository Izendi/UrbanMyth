using Assets.Scripts;
using Assets.Scripts.Events;
using System.Linq;
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
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, INTERACT_DISTANCE);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out InteractableNpc dialogueTrigger) && !DialogueManager.IsDialogueActive)
                {
                    dialogueTrigger.Interact();
                }
            }
        }
        else
        {
            
        }
    }

    public InteractableObject GetInteractableObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, INTERACT_DISTANCE);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out InteractableObject interactableObject))
                return interactableObject;
        }

        Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(ray, INTERACT_DISTANCE);

        if (hits.Any(h => h.transform.tag == "PickUpAble"))
        {
            EventAggregator.Instance.Publish(new InRangeOfLiftableObjectEvent());
            return null;
        }

        if (hits.Any(h => h.transform.tag == "Button"))
        {
            EventAggregator.Instance.Publish(new InRangeOfDoorButton());
            return null;
        }

        if (hits.Any(h => h.transform.tag == "LoadDoor"))
        {
            EventAggregator.Instance.Publish(new InRangeOfLoadDoorEvent());
            return null;
        }

        if (hits.Any(h => h.transform.tag == "NPC"))
        {
            EventAggregator.Instance.Publish(new InRangeOfNpcEvent());
            return null;
        }

        if (hits.Any(h => h.transform.tag == "OpenableDoor"))
        {
            EventAggregator.Instance.Publish(new InRangeOfOpenableDoorEvent());
            return null;
        }

        EventAggregator.Instance.Publish(new NoObjectToInteractWithEvent());

        return null;
    }
}
