using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalStateManager : MonoBehaviour
{
    public static GlobalStateManager Instance { get; private set; }
    public GameObject MenuSystemObj;
    private MenuInteraction MI_script;

    private PlayerInput playerInput;

    private InputAction menuOpenAction;

    // Global states
    public bool isGamePaused;
    public bool isPlayerDead;

    public bool[] collectedNotes;


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
    void Start()
    {
        MI_script = MenuSystemObj.GetComponent<MenuInteraction>();
    }

    public void CollectedNote(String noteNumber)
    {
        int noteNum = int.Parse(noteNumber);

        collectedNotes[noteNum] = true;
    }

    public int GetCollectedNotesSize()
    {
        return collectedNotes.Length;
    }

    public bool[] GetCollectedNoteArray()
    {
        return (bool[])collectedNotes.Clone(); //return a copy of the collectedNotes array.
    }

    public void PauseAndDisplayNote(int i)
    {

        MI_script.DisplayNoteOnMenu(i);

    }

    private void Unpause()
    {

    }
}
