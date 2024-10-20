using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalStateManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip wayOpenSound;

    [SerializeField]
    private AudioClip wayClosedSound;

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
    public bool[] backup_collectedNotes;

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

    public bool backup_has_ChildhoodToy;
    public bool backup_has_oldKey;
    public bool backup_has_codeBreaker;
    public bool backup_has_torch;
    public bool backup_has_vipRationCard;
                
    public bool backup_givenAway_ChildhoodToy;
    public bool backup_givenAway_oldKey;
    public bool backup_givenAway_codeBreaker;
    public bool backup_givenAway_torch;
    public bool backup_givenAway_vipRationCard;

    public void backUpData()
    {
        for(int i =0; i < collectedNotes.Length; i++)
        {
            backup_collectedNotes[i] = collectedNotes[i];
        }

        backup_has_ChildhoodToy = has_ChildhoodToy;
        backup_has_oldKey = has_oldKey;
        backup_has_codeBreaker = has_codeBreaker;
        backup_has_torch = has_torch;
        backup_has_vipRationCard = has_vipRationCard;

        backup_givenAway_ChildhoodToy = givenAway_ChildhoodToy;
        backup_givenAway_oldKey = givenAway_oldKey;
        backup_givenAway_codeBreaker = givenAway_codeBreaker;
        backup_givenAway_torch = givenAway_torch;
        backup_givenAway_vipRationCard = givenAway_vipRationCard;

    }

    public void wipeData()
    {
        for (int i = 0; i < collectedNotes.Length; i++)
        {
            collectedNotes[i] = false;

            
        }

        MI_script.deactivateNotes();
        MI_script.deactivateNonBackupItems();

        has_ChildhoodToy = false;
        has_oldKey = false;
        has_codeBreaker = false;
        has_torch = false;
        has_vipRationCard = false;

        givenAway_ChildhoodToy = false;
        givenAway_oldKey = false;
        givenAway_codeBreaker = false;
        givenAway_torch = false;
        givenAway_vipRationCard = false;

        
    }

    public void restoreBackupData()
    {
        MenuSystemObj = GameObject.FindWithTag("MENU");
        MI_script = MenuSystemObj.GetComponent<MenuInteraction>();

        for (int i = 0; i < collectedNotes.Length; i++)
        {
            collectedNotes[i] = backup_collectedNotes[i];


        }

        MI_script.activateCollectedNotes();

        has_ChildhoodToy = backup_has_ChildhoodToy;
        has_oldKey = backup_has_oldKey;
        has_codeBreaker = backup_has_codeBreaker;
        has_torch = backup_has_torch;
        has_vipRationCard = backup_has_vipRationCard;

        givenAway_ChildhoodToy = backup_givenAway_ChildhoodToy;
        givenAway_oldKey = backup_givenAway_oldKey;
        givenAway_codeBreaker = backup_givenAway_codeBreaker;
        givenAway_torch = backup_givenAway_torch;
        givenAway_vipRationCard = backup_givenAway_vipRationCard;

        
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // Ensure that there's only one instance of the GlobalStateManager
        if (Instance == null)
        {
            Instance = this;

            isGamePaused = false;
            isPlayerDead = false;

            playerInput = GetComponent<PlayerInput>();
            menuOpenAction = playerInput.actions["MenuOpen"];

            for (int i = 0; i < collectedNotes.Length; i++)
            {
                backup_collectedNotes[i] = false;
            }

            backup_has_ChildhoodToy = false;
            backup_has_oldKey = false;
            backup_has_codeBreaker = false;
            backup_has_torch = false;
            backup_has_vipRationCard = false;

            backup_givenAway_ChildhoodToy = false;
            backup_givenAway_oldKey = false;
            backup_givenAway_codeBreaker = false;
            backup_givenAway_torch = false;
            backup_givenAway_vipRationCard = false;

            //DontDestroyOnLoad(gameObject);  // Keep it alive between scenes
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

    public void KeyItemCollected(string itemName)
    {

        if (itemName == "ChildhoodToy")
        {
            has_ChildhoodToy = true;
        }
        if (itemName == "OldKey")
        {
            has_oldKey = true;
        }
        if (itemName == "CodeBreaker")
        {
            has_codeBreaker = true;
        }
        if (itemName == "Torch")
        {
            has_torch = true;
        }
        if (itemName == "VipRationCard")
        {
            has_vipRationCard = true;
        }
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

    public void PauseAndDisplayKeyItem(string keyItemName)
    {

        if (keyItemName == "ChildhoodToy")
        {
            has_ChildhoodToy = true;
            MI_script.DisplayKeyItem(0);

        }
        if (keyItemName == "OldKey")
        {
            has_oldKey = true;
            MI_script.DisplayKeyItem(1);
        }
        if (keyItemName == "CodeBreaker")
        {
            has_codeBreaker = true;
            MI_script.DisplayKeyItem(2);
        }
        if (keyItemName == "Torch")
        {
            has_torch = true;
            MI_script.DisplayKeyItem(3);
        }
        if (keyItemName == "VipRationCard")
        {
            has_vipRationCard = true;
            MI_script.DisplayKeyItem(4);
        }

    }

    private void Unpause()
    {

    }

    public void DoAction(string actionName)
    {
        if (actionName == "UnlockAllDoors")
        {
            GameObject[] Buttons = GameObject.FindGameObjectsWithTag("Button");

            SoundManager.instance.PlaySoundEffect(wayOpenSound, transform, 1.0f);

            foreach (GameObject but in Buttons)
            {
                //Debug.Log("Found object: " + but.name);
                DoorButton doorButton = but.GetComponent<DoorButton>();
                doorButton.ActivateButton();
            }
        }

        if (actionName == "LockAllDoors")
        {
            GameObject[] Buttons = GameObject.FindGameObjectsWithTag("Button");

            SoundManager.instance.PlaySoundEffect(wayClosedSound, transform, 1.0f);

            foreach (GameObject but in Buttons)
            {
                //Debug.Log("Found object: " + but.name);
                DoorButton doorButton = but.GetComponent<DoorButton>();
                doorButton.DeactivateButton();
            }
        }

        if(actionName == "UnlockDoor_1")
        {
            GameObject[] Buttons = GameObject.FindGameObjectsWithTag("Button");

            for(int i = 0; i < Buttons.Length; i++)
            {
                if(Buttons[i].name == "Lock_1")
                {
                    DoorButton doorButton = Buttons[i].GetComponent<DoorButton>();
                    doorButton.ActivateButton();
                }
            }

            SoundManager.instance.PlaySoundEffect(wayClosedSound, transform, 1.0f);
        }

        if (actionName == "UnlockDoor_2")
        {
            GameObject[] Buttons = GameObject.FindGameObjectsWithTag("Button");

            for (int i = 0; i < Buttons.Length; i++)
            {
                if (Buttons[i].name == "Lock_2")
                {
                    DoorButton doorButton = Buttons[i].GetComponent<DoorButton>();
                    doorButton.ActivateButton();
                }
            }

            SoundManager.instance.PlaySoundEffect(wayClosedSound, transform, 1.0f);
        }

        if (actionName == "placeholder")
        {
            MI_script.DoAction(actionName);
        }
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
