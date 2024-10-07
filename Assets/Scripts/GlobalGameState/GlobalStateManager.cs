using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalStateManager : MonoBehaviour
{
    public static GlobalStateManager Instance { get; private set; }

    private PlayerInput playerInput;

    private InputAction menuOpenAction;

    // Global states
    public bool isGamePaused;
    public bool isPlayerDead;


    private void Awake()
    {
        // Ensure that there's only one instance of the GlobalStateManager
        if (Instance == null)
        {
            Instance = this;

            isGamePaused = false;
            isPlayerDead = false;

            playerInput = GetComponent<PlayerInput>();
            menuOpenAction = playerInput.actions["MenuOpen"];

            DontDestroyOnLoad(gameObject);  // Keep it alive between scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGamePaused = menuOpenAction.WasPressedThisFrame();

        if(isGamePaused)
        {
            Pause();
        }
        else
        {
            Unpause();
        }
    }

    private void Pause()
    {
        Debug.Log("Pausing!");
    }

    private void Unpause()
    {

    }
}
