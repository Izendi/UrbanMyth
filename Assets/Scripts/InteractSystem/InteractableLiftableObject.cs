using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLiftableObject : InteractableObject
{
    // Start is called before the first frame update
    private void Start()
    {
        if (string.IsNullOrEmpty(InteractPrompt))
            InteractPrompt = "Press E to lift.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
