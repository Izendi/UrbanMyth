using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] public string InteractPrompt;
    // Start is called before the first frame update

    public virtual void Interact()
    {

    }
}
