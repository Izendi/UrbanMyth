using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Cam_AntiWallClip : MonoBehaviour
{
    public Transform targetTransform; // The target the camera is following
    public float minDistanceFromWall = 0.5f;
    public float offsetSpeed = 10f;
    public LayerMask offsetObjectsLayer;

    private Vector3 originalCameraLocalPosition;

    void Start()
    {
        // Store the original local position of the camera relative to its parent (the target)
        originalCameraLocalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        // Desired camera position in world space
        Vector3 desiredCameraWorldPosition = transform.parent.TransformPoint(originalCameraLocalPosition);

        // Direction from the target to the desired camera position
        Vector3 direction = desiredCameraWorldPosition - targetTransform.position;
        float distance = direction.magnitude;
        direction.Normalize();

        RaycastHit hit;

        // Cast a ray from the target towards the desired camera position
        if (Physics.Raycast(targetTransform.position, direction, out hit, distance + minDistanceFromWall, offsetObjectsLayer))
        {
            // Position the camera at the hit point minus minDistanceFromWall
            float hitDistance = Mathf.Max(hit.distance - minDistanceFromWall, 0f);

            // Calculate the new position for the camera
            Vector3 newCameraWorldPosition = targetTransform.position + direction * hitDistance;

            // Smoothly move the camera to the new position
            transform.position = Vector3.Lerp(transform.position, newCameraWorldPosition, offsetSpeed * Time.deltaTime);
        }
        else
        {
            // No obstruction, smoothly move the camera to its original position
            Vector3 desiredLocalPosition = Vector3.Lerp(transform.localPosition, originalCameraLocalPosition, offsetSpeed * Time.deltaTime);
            transform.localPosition = desiredLocalPosition;
        }
    }
}