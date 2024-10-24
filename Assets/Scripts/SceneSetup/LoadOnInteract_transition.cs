using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadOnInteract_transition : MonoBehaviour
{
    public Camera playerCamera;
    public KeyCode keyToPress;

    public TMP_Text loadSceneText;
    public TMP_Text entryDoorText;

    [SerializeField]
    private int NoSceneToLoad;

    [SerializeField]
    private float radius = 1.0f;

    public GameObject MenuSystemObj;
    private MenuInteraction MI_script;

    // New variables for transition
    public GameObject transitionCanvas; // Assign this in the inspector (the canvas that holds the image and text)
    public Sprite transitionImage; // Assign the Image (background or visual)
    public TMP_Text transitionText; // Assign the TextMeshPro for the caption
    public float transitionDuration = 3.0f; // How long the transition scene should last

   

    // Start is called before the first frame update
    void Start()
    {
        loadSceneText.enabled = false;
        MenuSystemObj = GameObject.FindWithTag("MENU");
        MI_script = MenuSystemObj.GetComponent<MenuInteraction>();
        

        // Ensure the transition canvas is initially disabled
        if (transitionCanvas != null)
        {
            transitionCanvas.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        loadSceneText.enabled = false;
        entryDoorText.enabled = false;

        if (MenuSystemObj == null)
        {
            MenuSystemObj = GameObject.FindWithTag("MENU");
            MI_script = MenuSystemObj.GetComponent<MenuInteraction>();
        }

        if (playerCamera == null)
        {
            GameObject cam = GameObject.FindWithTag("MainCamera");
            playerCamera = cam.GetComponent<Camera>();
        }

        if (playerCamera == null)
        {
            playerCamera = FindObjectOfType<Camera>(); // Reassign the reference
            if (playerCamera == null)
            {
                Debug.LogWarning("PlayerInteract object not found in the scene.");
                return; // Exit early if playerInteract is still null
            }
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, radius))
        {
            if (hit.collider.CompareTag("LoadDoor"))
            {
                loadSceneText.enabled = true;

                if (Input.GetKeyDown(keyToPress))
                {
                    if (NoSceneToLoad >= 0)
                    {
                        // StartCoroutine(ShowTransitionAndLoadScene(NoSceneToLoad));
                        ShowTransition();
                        
                        // Use Invoke to delay the scene load after the transition duration
                        Invoke(nameof(LoadScene), transitionDuration);
                        
                    }
                    else
                    {
                        //StartCoroutine(ShowTransitionAndLoadScene(SceneManager.GetActiveScene().buildIndex + 1));
                        ShowTransition();
                        Invoke(nameof(LoadNextScene), transitionDuration);
                    }
                }
            }
            else
            {
                if (hit.collider.CompareTag("EntryDoor"))
                {
                    entryDoorText.enabled = true;
                }
                else
                {
                    loadSceneText.enabled = false;
                    entryDoorText.enabled = false;
                }
            }
        }
        
    }
    void ShowTransition()
    {
        if (transitionCanvas != null)
        {
            transitionCanvas.SetActive(true);
            // Optionally, configure the image or UI if needed
        }
    }

    void LoadScene()
    {
        MI_script.LoadNextScene(NoSceneToLoad);
    }

    void LoadNextScene()
    {
        MI_script.LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator ShowTransitionAndLoadScene(int sceneIndex)
    {
        // Show the transition canvas
        if (transitionCanvas != null)
        {
            transitionCanvas.SetActive(true);

            // Set the transition text and image (customize these based on the scene or event)
            //if (transitionText != null)
            //{
              //  transitionText.text = "As I walk upstairs, memories start to come back to me... I remember I used to have a house.";
            //}

            if (transitionImage != null)
            {
                // Optionally, change the image if needed
                // transitionImage.sprite = yourSprite;
            }
            Debug.Log("dispaly img");
            // Wait for the specified duration
            yield return new WaitForSecondsRealtime(transitionDuration);
            Debug.Log("wait for second");

            // Hide the transition canvas after the duration
            transitionCanvas.SetActive(false);
            Debug.Log("set inactive");
        }

        // Load the next scene after the transition
        MI_script.LoadNextScene(sceneIndex);
    }
}
