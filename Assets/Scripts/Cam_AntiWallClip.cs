using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Cam_AntiWallClip : MonoBehaviour
{
    public Transform cameraTransform;

    public float minDistanceFromWall = 0.5f;

    public float offsetSpeed = 10f;

    public LayerMask offsetObjectsLayer;

    private Vector3 originalCameraPosition;



    // Start is called before the first frame update
    void Start()
    {
        originalCameraPosition = transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, minDistanceFromWall, offsetObjectsLayer))
        {
            // If we hit a wall, move the camera backward to prevent clipping
            Vector3 hitPoint = hit.point;
            Vector3 cameraDirection = (transform.position - hitPoint).normalized;
            float distanceToMoveBack = minDistanceFromWall - hit.distance;

            // Shift the camera back away from the wall smoothly
            transform.position -= cameraDirection * distanceToMoveBack;
        }
        else
        {
            // If no walls are detected, smoothly return to the original position
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalCameraPosition, offsetSpeed * Time.deltaTime);
        }

    }
}
