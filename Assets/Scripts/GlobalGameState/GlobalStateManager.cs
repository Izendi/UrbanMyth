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
    public bool has_money;
    public bool has_codeBreaker;
    public bool has_torch;
    public bool has_vipRationCard;
    public bool has_catTreat;
    public bool has_photo;

    public bool givenAway_ChildhoodToy;
    public bool givenAway_money;
    public bool givenAway_codeBreaker;
    public bool givenAway_torch;
    public bool givenAway_vipRationCard;
    public bool givenAway_catTreat;
    public bool givenAway_photo;

    public bool backup_has_ChildhoodToy;
    public bool backup_has_money;
    public bool backup_has_codeBreaker;
    public bool backup_has_torch;
    public bool backup_has_vipRationCard;
    public bool backup_has_catTreat;
    public bool backup_has_photo;

    public bool backup_givenAway_ChildhoodToy;
    public bool backup_givenAway_money;
    public bool backup_givenAway_codeBreaker;
    public bool backup_givenAway_torch;
    public bool backup_givenAway_vipRationCard;
    public bool backup_givenAway_catTreat;
    public bool backup_givenAway_photo;

    public void backUpData()
    {
        for(int i =0; i < collectedNotes.Length; i++)
        {
            backup_collectedNotes[i] = collectedNotes[i];
        }

        backup_has_ChildhoodToy = has_ChildhoodToy;
        backup_has_money = has_money;
        backup_has_codeBreaker = has_codeBreaker;
        backup_has_torch = has_torch;
        backup_has_vipRationCard = has_vipRationCard;
        backup_has_catTreat = has_catTreat;
        backup_has_photo = has_photo;

        backup_givenAway_ChildhoodToy = givenAway_ChildhoodToy;
        backup_givenAway_money = givenAway_money;
        backup_givenAway_codeBreaker = givenAway_codeBreaker;
        backup_givenAway_torch = givenAway_torch;
        backup_givenAway_vipRationCard = givenAway_vipRationCard;
        backup_givenAway_catTreat = givenAway_catTreat;
        backup_givenAway_photo = givenAway_photo;
        
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
        has_money = false;
        has_codeBreaker = false;
        has_torch = false;
        has_vipRationCard = false;
        has_catTreat = false;
        has_photo = false;

        givenAway_ChildhoodToy = false;
        givenAway_money = false;
        givenAway_codeBreaker = false;
        givenAway_torch = false;
        givenAway_vipRationCard = false;
        givenAway_catTreat = false;
        givenAway_photo = false;

        
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
        has_money = backup_has_money;
        has_codeBreaker = backup_has_codeBreaker;
        has_torch = backup_has_torch;
        has_vipRationCard = backup_has_vipRationCard;
        has_catTreat = backup_has_catTreat;
        has_photo = backup_has_photo;

        givenAway_ChildhoodToy = backup_givenAway_ChildhoodToy;
        givenAway_money = backup_givenAway_money;
        givenAway_codeBreaker = backup_givenAway_codeBreaker;
        givenAway_torch = backup_givenAway_torch;
        givenAway_vipRationCard = backup_givenAway_vipRationCard;
        givenAway_catTreat = backup_givenAway_catTreat;
        givenAway_photo = backup_givenAway_photo;

        
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
            backup_has_money = false;
            backup_has_codeBreaker = false;
            backup_has_torch = false;
            backup_has_vipRationCard = false;
            backup_has_catTreat = false;
            backup_has_photo = false;

            backup_givenAway_ChildhoodToy = false;
            backup_givenAway_money = false;
            backup_givenAway_codeBreaker = false;
            backup_givenAway_torch = false;
            backup_givenAway_vipRationCard = false;
            backup_givenAway_catTreat= false;
            backup_givenAway_photo = false;



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
            has_money = true;
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
        if (itemName == "CatTreat")
        {
            has_catTreat = true;
        }
        if (itemName == "Photo")
        {
            has_photo = true;
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
        if (keyItemName == "Money")
        {
            has_money = true;
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
        if (keyItemName == "CatTreat")
        {
            has_catTreat = true;
            MI_script.DisplayKeyItem(5);

            GameObject[] NPCs = GameObject.FindGameObjectsWithTag("NPC");

            for (int i = 0; i < NPCs.Length; i++)
            {
                if (NPCs[i].name == "NpcBillboard")
                {
                    TextAsset df = NPCs[i].GetComponent<dfPlaceholder>().getDF();
                    NPCs[i].GetComponent<InteractableNpc>().setDialogueFile(df);
                }
            }

        }
        if (keyItemName == "Photo")
        {
            has_photo = true;
            MI_script.DisplayKeyItem(6);
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

        if(actionName == "OpenHatch")
        {
            if(has_catTreat)
            {
                SoundManager.instance.PlaySoundEffect(wayOpenSound, transform, 1.0f);

                GameObject catHatch = GameObject.FindGameObjectWithTag("cathatch");

                //Open
            }
            else
            {
                SoundManager.instance.PlaySoundEffect(wayClosedSound, transform, 1.0f);
            }

            

            //Do open action
        }

        if (actionName == "RecievePhoto")
        {
            GameObject[] KeyItems = GameObject.FindGameObjectsWithTag("KeyItem");

            for (int i = 0; i < KeyItems.Length; i++)
            {
                if (KeyItems[i].name == "Photo")
                {
                    Vector3 position = KeyItems[i].transform.position;

                    position.y = 2.143f;

                    KeyItems[i].transform.position = position;
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGamePaused)
            {
                isGamePaused = true;
                MI_script.OnEscButtonPress();
            }
            else
            {
                isGamePaused = false;
                MI_script.OnResumeButtonPress();

            }
        }
    }

}
