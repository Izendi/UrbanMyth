using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteJitter : MonoBehaviour
{
    // How fast the object jitters
    public float jitterSpeed = 20f;

    // How far the object jitters
    public float jitterAmount = 0.05f;

    // Original local position to oscillate around
    private Vector3 originalLocalPosition;

    void Start()
    {
        // Store the original local position of the sprite relative to the parent
        originalLocalPosition = transform.localPosition;
    }

    void Update()
    {
        // Calculate the jitter effect
        float jitter = Mathf.Sin(Time.time * jitterSpeed) * jitterAmount;

        // Apply the jitter to the object's local position (horizontal jitter, but can change to vertical or both)
        transform.localPosition = new Vector3(originalLocalPosition.x + jitter, originalLocalPosition.y, originalLocalPosition.z);
    }
}
