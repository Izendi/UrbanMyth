using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public Material red;
    public Material green;
    public Camera playerCamera;

    public KeyCode keyToPress;

    private Renderer objectRenderer;
    private bool greenColor;

    private SlidingDoorController sdc;

    [SerializeField]
    private GameObject linkedDoor;

    [SerializeField]
    private float radius = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        sdc = linkedDoor.GetComponent<SlidingDoorController>();
        objectRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {

            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            //var hits = Physics.SphereCastAll(t.position + t.forward, radius, t.forward, radius);
            RaycastHit[] hits = Physics.RaycastAll(ray, radius);

            var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "Button");

            if (hitIndex != -1)
            {
                if (greenColor)
                {
                    objectRenderer.material = red;
                    greenColor = false;

                    sdc.activateDoor();
                }
                else
                {
                    objectRenderer.material = green;
                    greenColor = true;
                    sdc.activateDoor();
                }
            }
    
        }
    }
}
