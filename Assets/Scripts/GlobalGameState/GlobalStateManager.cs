using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.Scripts;
using Assets.Scripts.Events;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;  
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class GlobalStateManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip wayOpenSound;

    [SerializeField]
    private AudioClip wayClosedSound;

    [SerializeField]
    private AudioMixerGroup audioMixer;

    public Vector3 level_0_startPos = new Vector3(16f,1f,8f);
    public Quaternion level_0_startRot = Quaternion.Euler(0, 0, 0);
    public Vector3 level_1_startPos = new Vector3(0f, 1f, 0f);
    public Quaternion level_1_startRot = Quaternion.Euler(0, 0, 0);

    public static GlobalStateManager Instance { get; private set; }
    public GameObject MenuSystemObj;
    private MenuInteraction MI_script;

    private PlayerInput playerInput;

    private InputAction menuOpenAction;

    public int KarmaLevel = 0;

    // Global states
    public bool isGamePaused;
    public bool isPlayerDead;
    public bool shownOnce = false;

    public bool[] collectedNotes;
    public bool[] backup_collectedNotes;

    public bool has_ChildhoodToy;
    public bool has_money;
    public bool has_fireEscapePlan;
    public bool has_oldKey;
    public bool has_vipRationCard;
    public bool has_catTreat;
    public bool has_photo;

    public bool hasShownPhoto = false;

    public bool givenAway_ChildhoodToy;
    public bool givenAway_money;
    public bool givenAway_fireEscapePlan;
    public bool givenAway_oldKey;
    public bool givenAway_vipRationCard;
    public bool givenAway_catTreat;
    public bool givenAway_photo;

    public bool backup_has_ChildhoodToy;
    public bool backup_has_money;
    public bool backup_has_fireEscapePlan;
    public bool backup_has_oldKey;
    public bool backup_has_vipRationCard;
    public bool backup_has_catTreat;
    public bool backup_has_photo;

    public bool backup_givenAway_ChildhoodToy;
    public bool backup_givenAway_money;
    public bool backup_givenAway_fireEscapePlan;
    public bool backup_givenAway_oldKey;
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
        backup_has_fireEscapePlan = has_fireEscapePlan;
        backup_has_oldKey = has_oldKey;
        backup_has_vipRationCard = has_vipRationCard;
        backup_has_catTreat = has_catTreat;
        backup_has_photo = has_photo;

        backup_givenAway_ChildhoodToy = givenAway_ChildhoodToy;
        backup_givenAway_money = givenAway_money;
        backup_givenAway_fireEscapePlan = givenAway_fireEscapePlan;
        backup_givenAway_oldKey = givenAway_oldKey;
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
        has_fireEscapePlan = false;
        has_oldKey = false;
        has_vipRationCard = false;
        has_catTreat = false;
        has_photo = false;

        givenAway_ChildhoodToy = false;
        givenAway_money = false;
        givenAway_fireEscapePlan = false;
        givenAway_oldKey = false;
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
        has_fireEscapePlan = backup_has_fireEscapePlan;
        has_oldKey = backup_has_oldKey;
        has_vipRationCard = backup_has_vipRationCard;
        has_catTreat = backup_has_catTreat;
        has_photo = backup_has_photo;

        givenAway_ChildhoodToy = backup_givenAway_ChildhoodToy;
        givenAway_money = backup_givenAway_money;
        givenAway_fireEscapePlan = backup_givenAway_fireEscapePlan;
        givenAway_oldKey = backup_givenAway_oldKey;
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
            backup_has_fireEscapePlan = false;
            backup_has_oldKey = false;
            backup_has_vipRationCard = false;
            backup_has_catTreat = false;
            backup_has_photo = false;

            backup_givenAway_ChildhoodToy = false;
            backup_givenAway_money = false;
            backup_givenAway_fireEscapePlan = false;
            backup_givenAway_oldKey = false;
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

        if (audioMixer == null)
        {
            Debug.LogError("AudioMixerGroup not assigned in the Inspector.");
            return;
        }
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
        if (itemName == "Money")
        {
            has_money = true;
        }
        if (itemName == "fireEscapePlan")
        {
            has_fireEscapePlan = true;
        }
        if (itemName == "oldKey")
        {
            has_fireEscapePlan = true;
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
        if (keyItemName == "fireEscapePlan")
        {
            has_fireEscapePlan = true;
            MI_script.DisplayKeyItem(2);
        }
        if (keyItemName == "oldKey")
        {
            has_oldKey = true;
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
        else if (actionName == "LockAllDoors")
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
        else if (actionName == "EndGame")
        {
            MI_script.LoadNextScene(10);
        }
        else if(actionName == "UnlockDoor_1")
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
        else if (actionName == "UnlockDoor_2")
        {   if (has_money)
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
        }
        else if(actionName == "GiveCatTreat")
        {
            if(has_catTreat)
            {
                SoundManager.instance.PlaySoundEffect(wayOpenSound, transform, 1.0f);

                GameObject[] Buttons = GameObject.FindGameObjectsWithTag("Button");

                //SoundManager.instance.PlaySoundEffect(wayOpenSound, transform, 1.0f);

                foreach (GameObject but in Buttons)
                {
                    //Debug.Log("Found object: " + but.name);
                    DoorButton doorButton = but.GetComponent<DoorButton>();
                    doorButton.ActivateButton();
                }
            }
            else
            {
                SoundManager.instance.PlaySoundEffect(wayClosedSound, transform, 1.0f);
            }

            

            //Do open action
        }
        else if (actionName == "RecievePhoto")
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
        else if (actionName == "placeholder")
        {
            MI_script.DoAction(actionName);
        }
        else if (actionName.Any(c => char.IsDigit(c)))
        {
            var actions = actionName.Split("|");
            if (actions.Length != 3)
                return;

            var npcName = actions[0];
            var newPosition = actions[1];
            var newStart = actions[2];
            Vector3? newPositionVector = null;
            int? newStartNode = null;

            if (!string.IsNullOrEmpty(newPosition))
            {
                var position = newPosition.Split(",");

                float x = float.Parse(position[0], CultureInfo.InvariantCulture);
                float y = float.Parse(position[1], CultureInfo.InvariantCulture);
                float z = float.Parse(position[2], CultureInfo.InvariantCulture);
                Debug.Log($"{x}; {y}; {z};");

                newPositionVector = new Vector3(x, y, z);
            }

            if (!string.IsNullOrEmpty(newStart))
            {
                newStartNode = int.TryParse(newStart, out var nsn) ? nsn : (int?)null;
            }

            if (!newPositionVector.HasValue && !newStartNode.HasValue)
                return;

            EventAggregator.Instance.Publish(new NewDialogueStartNodeEvent
            {
                NewPosition = newPositionVector,
                NewStartNodeId = newStartNode,
                NpcName = npcName
            }); 
        }
        else if(actionName == "RemoveAllNPCs")
        {
            GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

            foreach (GameObject npc in npcs)
            {
                npc.SetActive(false);
            }
        }
        else if (actionName == "ShowPhoto")
        {
            hasShownPhoto = true;
        }
        else if (actionName == "IncrementKarma")
        {
            KarmaLevel = KarmaLevel + 2; //#FIX_LATER
        }
        else if (actionName == "DecrementKarma")
        {
            KarmaLevel = KarmaLevel - 1;
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


        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.outputAudioMixerGroup = audioMixer;
        }
    }

}
