using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField]
    private AudioClip greenButtonSound;

    [SerializeField]
    private AudioClip redButtonSound;

    public Camera playerCamera;

    public KeyCode keyToPress;

    private Renderer objectRenderer;
    private bool greenColor;

    private SlidingDoorController sdc;

    [SerializeField]
    private GameObject linkedDoor;

    [SerializeField]
    private float radius = 1.0f;

    private Material redBut;
    private Material greenBut;

    // Start is called before the first frame update
    void Start()
    {
        sdc = linkedDoor.GetComponent<SlidingDoorController>();
        objectRenderer = GetComponent<Renderer>();

        redBut = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        redBut.color = Color.red;

        greenBut = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        greenBut.color = Color.green;

        objectRenderer.material = redBut;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {

            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            //var hits = Physics.SphereCastAll(t.position + t.forward, radius, t.forward, radius);
            RaycastHit[] hits = Physics.RaycastAll(ray, radius);

            if(hits.Length == 0)
            {
                return;
            }

            var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "Button");


            if (hitIndex != -1)
            {
                GameObject hitObject = hits[hitIndex].transform.gameObject;

                if (hitObject == this.gameObject)
                {
                    if (greenColor)
                    {
                        objectRenderer.material = redBut;
                        greenColor = false;

                        SoundManager.instance.PlaySoundEffect(redButtonSound, transform, 1.0f);

                        sdc.activateDoor();
                    }
                    else
                    {
                        objectRenderer.material = greenBut;
                        greenColor = true;

                        SoundManager.instance.PlaySoundEffect(greenButtonSound, transform, 1.0f);

                        sdc.activateDoor();
                    }
                }
            }

            
    
        }
    }
}
