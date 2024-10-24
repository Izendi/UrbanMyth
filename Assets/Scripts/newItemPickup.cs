using Assets.Scripts.Events;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class newItemPickup : MonoBehaviour
{
    public Camera playerCamera;

    [SerializeField]
    private float pickupRadius;

    [SerializeField]
    private float maxHoldDistance;

    public float distance = 2f;
    public float height = 1f;

    [SerializeField]
    private float lerpSpeed = 2.0f;

    [SerializeField]
    private Transform objectGrabPointTransform;

    private GameObject heldObject;

    private float distanceFromPlayer;

    private void Update()
    {
        var t = transform;
        var pressedE = Input.GetKeyDown(KeyCode.E);
        if (heldObject)
        {
            distanceFromPlayer = Vector3.Distance(heldObject.transform.position, playerCamera.transform.position);

            if (pressedE || (distanceFromPlayer > maxHoldDistance))
            {
                EventAggregator.Instance.Publish(new LiftableObjectEvent { ObjectLifted = false });

                var rigidBody = heldObject.GetComponent<Rigidbody>();
                rigidBody.drag = 1f;
                rigidBody.useGravity = true;
                rigidBody.constraints = RigidbodyConstraints.None;
                heldObject = null;
            }
        }
        else
        {
            if (pressedE)
            {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

                //var hits = Physics.SphereCastAll(t.position + t.forward, radius, t.forward, radius);
                RaycastHit[] hits = Physics.RaycastAll(ray, pickupRadius);

                var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "PickUpAble");

                if (hitIndex != -1)
                {
                    EventAggregator.Instance.Publish(new LiftableObjectEvent());
                    var hitObject = hits[hitIndex].transform.gameObject;
                    heldObject = hitObject;
                    var rigidBody = heldObject.GetComponent<Rigidbody>();
                    rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
                    rigidBody.drag = 25f;
                    rigidBody.useGravity = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        /*
        if (heldObject == null)
            return;
        //var t = transform;
        var rigidBody = heldObject.GetComponent<Rigidbody>();

        Vector3 newPos = Vector3.Lerp(objectGrabPointTransform.position, heldObject.transform.position, Time.deltaTime * lerpSpeed);

        //var difference = objectGrabPointTransform.position - heldObject.transform.position;
        //rigidBody.AddForce(difference * 500);

        rigidBody.MovePosition(newPos);

        heldObject.transform.rotation = objectGrabPointTransform.rotation;
        */

        if (heldObject == null)
            return;
        var t = transform;
        var rigidBody = heldObject.GetComponent<Rigidbody>();
        var moveTo = t.position + distance * t.forward + height * t.up;
        var difference = moveTo - heldObject.transform.position;
        rigidBody.AddForce(difference * 500);
        heldObject.transform.rotation = t.rotation;
    }
}
