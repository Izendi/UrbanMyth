using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.Events;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour, IEventHandler<DialogueEndedEvent>
{
    private const float INTERACT_DISTANCE = 2.0f;
    private const float OFFSET_DISTANCE = 1.5f;
    private const float TRANSITION_SPEED = 2f; // Speed of the transition
    
    [SerializeField] private Camera PlayerCamera;

    private bool isTransitioning = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Quaternion resetRotation;
    private Vector3 resetPosition;

    void Start()
    {
        EventAggregator.Instance.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        // If transitioning - that is, if the camera is moving to focus on the npc we are interacting with, update the camera's position and rotation smoothly
        if (false) // (isTransitioning)
        {
            // Smoothly move the camera towards the target position
            // PlayerCamera.transform.position = Vector3.Lerp(PlayerCamera.transform.position, targetPosition, Time.deltaTime * TRANSITION_SPEED);

            // Smoothly rotate the camera towards the target rotation
            PlayerCamera.transform.rotation = Quaternion.Lerp(PlayerCamera.transform.rotation, targetRotation, Time.deltaTime * TRANSITION_SPEED);

            // Check if the camera has reached the target
            if (Quaternion.Angle(PlayerCamera.transform.rotation, targetRotation) < 0.02f)
                isTransitioning = false; // Stop transitioning
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, INTERACT_DISTANCE);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out InteractableNpc dialogueTrigger) && !DialogueManager.IsDialogueActive)
                {
                    FocusCamera(dialogueTrigger.transform);
                    dialogueTrigger.Interact();
                }
            }
        }
        else if (!DialogueManager.IsDialogueActive)
        {
            if (PlayerCamera.transform.position.x != transform.position.x ||
                PlayerCamera.transform.position.z != transform.position.z)
            {
                var cameraHeight = PlayerCamera.transform.position.y;
                var transitionTarget = new Vector3(transform.position.x, cameraHeight, transform.position.z);

                PlayerCamera.transform.position = transitionTarget;
            }
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

        return null;
    }

    private void FocusCamera(Transform target)
    {
        // Calculate the target rotation to look at the NPC
        var direction = target.position - transform.position;
        direction = new Vector3(-5.5f, PlayerCamera.transform.rotation.y, PlayerCamera.transform.rotation.z);

        targetRotation = Quaternion.LookRotation(direction);

        //Save the current camera position and rotation
        resetPosition = PlayerCamera.transform.position;
        resetRotation = PlayerCamera.transform.rotation;

        // Start transitioning the camera
        isTransitioning = true;
    }

    public void Handle(DialogueEndedEvent eventArgs)
    {
        isTransitioning = false;
        PlayerCamera.transform.position = resetPosition;
        PlayerCamera.transform.rotation = resetRotation;
    }
}
