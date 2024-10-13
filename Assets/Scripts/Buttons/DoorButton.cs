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

    [SerializeField]
    private AudioClip inactiveSound;

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
    private Material blueBut;

    public float pulseDuration = 2.0f;
    public float maxEmission = 1.5f;

    public bool isActive = false;

    private float pulseTimer;

    // Start is called before the first frame update
    void Start()
    {
        sdc = linkedDoor.GetComponent<SlidingDoorController>();
        objectRenderer = GetComponent<Renderer>();

        redBut = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        redBut.color = Color.red;

        greenBut = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        greenBut.color = Color.green;

        blueBut = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        blueBut.color = Color.blue;

        objectRenderer.material = redBut;
    }

    public void DeactivateButton()
    {
        isActive = false;
    }

    public void ActivateButton()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCamera == null)
        {
            playerCamera = FindObjectOfType<Camera>(); // Reassign the reference
            if (playerCamera == null)
            {
                Debug.LogWarning("PlayerInteract object not found in the scene.");
                return; // Exit early if playerInteract is still null
            }
        }

        if (isActive == false)
        {
            objectRenderer.material = blueBut;
        }
        else
        {
            if(greenColor)
            {
                objectRenderer.material = greenBut;
            }
            else
            {
                objectRenderer.material = redBut;
            }
        }

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
                    if (isActive)
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
                    else
                    {

                        Color baseColor = blueBut.GetColor("_BaseColor");

                        pulseTimer += Time.deltaTime;

                        // Calculate a value that oscillates between 0 and 1 over time
                        float pulse = Mathf.PingPong(pulseTimer / pulseDuration, 1);

                        Color emissionColor = baseColor * Mathf.Lerp(0.1f, maxEmission, pulse);

                        // Apply the new emission color to the material
                        blueBut.SetColor("_EmissionColor", emissionColor);

                        SoundManager.instance.PlaySoundEffect(inactiveSound, transform, 1.0f);

                        if(pulseTimer > pulseDuration)
                        {
                            pulseTimer = 0.0f;
                        }
                    }
                }
            }

            
    
        }
    }
}
