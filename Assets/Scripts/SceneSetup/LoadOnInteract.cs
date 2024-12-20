using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnInteract : MonoBehaviour
{

    public Camera playerCamera;
    public KeyCode keyToPress;

    
    public int NoSceneToLoad;

    [SerializeField]
    private float radius = 1.0f;

    public GameObject MenuSystemObj;
    private MenuInteraction MI_script;

    public Animator transitionAnimator;

    // Start is called before the first frame update
    void Start()
    {
        MenuSystemObj = GameObject.FindWithTag("MENU");
        MI_script = MenuSystemObj.GetComponent<MenuInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuSystemObj == null)
        {
            MenuSystemObj = GameObject.FindWithTag("MENU");
            MI_script = MenuSystemObj.GetComponent<MenuInteraction>();
        }

        if(playerCamera == null)
        {
            GameObject cam =  GameObject.FindWithTag("PlayCam");
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

        //var hits = Physics.SphereCastAll(t.position + t.forward, radius, t.forward, radius);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, radius))
        {
            // Optional: Check if the object hit is the one you're looking for (by tag, layer, or name)
            if (hit.collider.CompareTag("LoadDoor"))
            {

                if (Input.GetKeyDown(keyToPress))
                {
                    if (NoSceneToLoad >= 0)
                    {
                        //MI_script.LoadNextScene(NoSceneToLoad);
                        StartCoroutine(LoadLevel(NoSceneToLoad));
                    }
                    else
                    {
                        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Load the next scene in the sequence
                        MI_script.LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                }
            }
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        MI_script.LoadNextScene(NoSceneToLoad);
    }

    void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}
