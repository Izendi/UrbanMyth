using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactDistance = 3.0f;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactDistance);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out InteractableNpc dialogueTrigger))
                    dialogueTrigger.TriggerDialogue();
            }
        }
    }

    public InteractableObject GetInteractableObject()
    {
        float interactDistance = 2.0f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out InteractableObject interactableObject))
                return interactableObject;
        }

        return null;
    }
}
