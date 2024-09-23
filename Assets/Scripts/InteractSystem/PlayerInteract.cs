using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private const float INTERACT_DISTANCE = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, INTERACT_DISTANCE);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out InteractableNpc dialogueTrigger))
                    dialogueTrigger.TriggerDialogue();
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
}
