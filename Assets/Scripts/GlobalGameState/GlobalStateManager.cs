using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalStateManager : MonoBehaviour
{
    public Vector3 level_0_startPos = new Vector3(16f,1f,8f);
    public Quaternion level_0_startRot = Quaternion.Euler(0, 0, 0);
    public Vector3 level_1_startPos = new Vector3(0f, 1f, 0f);
    public Quaternion level_1_startRot = Quaternion.Euler(0, 0, 0);

    public static GlobalStateManager Instance { get; private set; }
    public GameObject MenuSystemObj;
    private MenuInteraction MI_script;

    private PlayerInput playerInput;

    private InputAction menuOpenAction;

    // Global states
    public bool isGamePaused;
    public bool isPlayerDead;
    public bool shownOnce = false;

    public bool[] collectedNotes;

    public bool has_ChildhoodToy;
    public bool has_oldKey;
    public bool has_codeBreaker;
    public bool has_torch;
    public bool has_vipRationCard;

    public bool givenAway_ChildhoodToy;
    public bool givenAway_oldKey;
    public bool givenAway_codeBreaker;
    public bool givenAway_torch;
    public bool givenAway_vipRationCard;


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

    public void DoAction(string actionName)
    {
        MI_script.DoAction(actionName);
    }

    private void Update()
    {
        if (MenuSystemObj == null)
        {
            MenuSystemObj = GameObject.FindWithTag("MENU");
            MI_script = MenuSystemObj.GetComponent<MenuInteraction>();
        }
    }

}
