using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using static System.Net.Mime.MediaTypeNames;

public class MenuInteraction : MonoBehaviour
{
    public static MenuInteraction Instance;

    public GameObject GlobalStateManagerObj;
    private GlobalStateManager GSM_script;
    private GameObject PlayerObj;

    private GameObject PlayCamObj;
    private BasicMouseLook basicMouseLook_script;


    private bool deadState;
    //private bool shownOnce = false;

    public bool MenuOpenCloseInput { get; private set; }

    public string[] noteNames = { "Dear Sister", "17/12/2046", "Find Me", "Trust Me" };
    public bool[] savedNotes = { false, false, false, false, false };

    [SerializeField]
    private GameObject _mainMenuCanvas;

    [SerializeField]
    private GameObject _settingsMenuCanvas;

    [SerializeField]
    private GameObject _inventoryMenuCanvas;

    [SerializeField]
    private GameObject _inventoryNoteCanvas;

    [SerializeField]
    private GameObject _inventoryItemsCanvas;

    [SerializeField]
    private GameObject _DeathMenuCanvas;

    [SerializeField]
    private GameObject _mainMenuFirstSelected;

    [SerializeField]
    private GameObject _settingsMenuFirstSelected;

    [SerializeField]
    private GameObject _inventoryMenuFirstSelected;

    [SerializeField]
    private GameObject _inventoryNotesFirstSelected;

    [SerializeField]
    private GameObject _inventoryItemsFirstSelected;

    [SerializeField]
    private GameObject childhoodToy_selected;

    [SerializeField]
    private GameObject oldKey_selected;

    [SerializeField]
    private GameObject CodeBreaker_selected;

    [SerializeField]
    private GameObject Torch_selected;

    [SerializeField]
    private GameObject vipRationCard_selected;

    [SerializeField]
    private GameObject _deathScreenFirstSelected;

    [SerializeField]
    private GameObject _catTreat_selected;

    [SerializeField]
    private GameObject _photo_selected;



    [SerializeField]
    private GameObject[] pickupSelected;

    [SerializeField]
    private KeyCode menuKey = KeyCode.Z;

    [SerializeField]
    private AudioClip reloadSound;

    [SerializeField]
    private AudioClip buttonPressSound;

    [SerializeField]
    private AudioClip resumeSound;

    [SerializeField]
    private AudioClip deathSound;

    [SerializeField]
    private AudioClip respawnSound;


    [SerializeField]
    private GameObject[] notePanels;

    private bool isPaused = false;

    Options menuOptions = new Options();
    public AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance == null)
        {
            // If no instance exists, this becomes the singleton instance
            Instance = this;

            // Prevent this object from being destroyed when loading new scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists and it's not this one, destroy this instance
            Destroy(gameObject);
        }

        PlayerObj = GameObject.FindWithTag("Player");

        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.sceneLoaded += OnSceneLoaded_2;

        if (PlayCamObj == null)
        {
            PlayCamObj = GameObject.FindWithTag("PlayCam");
            basicMouseLook_script = PlayCamObj.GetComponent<BasicMouseLook>();
        }

    }

    private void Start()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);



        GSM_script = GlobalStateManagerObj.GetComponent<GlobalStateManager>();

        menuOptions.audioMixer = audioMixer;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerObj == null)
        {
            PlayerObj = GameObject.FindWithTag("Player");
        }
        if (GlobalStateManagerObj == null)
        {
            GlobalStateManagerObj = GameObject.FindWithTag("GSO");
            GSM_script = GlobalStateManagerObj.GetComponent<GlobalStateManager>();
        }


        if (PlayCamObj == null)
        {
            PlayCamObj = GameObject.FindWithTag("PlayCam");
            basicMouseLook_script = PlayCamObj.GetComponent<BasicMouseLook>();
        }

        if (Input.GetKeyUp(menuKey) && isPaused == false && deadState == false)
        {
            GameObject DM = GameObject.FindWithTag("DS_MENU");

            if (DM == null)
            {
                GSM_script.isGamePaused = true;
                Pause();
            }
        }
        else if (Input.GetKeyUp(menuKey) && isPaused == true && deadState == false)
        {
            GameObject DM = GameObject.FindWithTag("DS_MENU");

            if (DM == null)
            {
                GSM_script.isGamePaused = false;
                Unpause();
            }
        }

        if(Time.timeSinceLevelLoad < 1.0f)
        {
            GSM_script.isPlayerDead = false;
            basicMouseLook_script.registerMouse = true;

            UnDeathPause();
        }

        if(GSM_script.isPlayerDead && Time.timeSinceLevelLoad > 1.0f)
        {
            GSM_script.isPlayerDead = false;

            DeathPause();

        }


    }

    public void DoAction(string actionName)
    {
        Debug.Log( $"action: {actionName}");


    }

    public void activateCollectedNotes()
    {
        bool[] collectedNotes = GSM_script.GetCollectedNoteArray();

        for (int i = 0; i < collectedNotes.Length; i++)
        {
            if (collectedNotes[i])
            {
                string buttonName = i.ToString();
                Transform buttonTransform = _inventoryNoteCanvas.transform.Find(buttonName);
                Button b = buttonTransform.GetComponent<Button>();

                TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();
     
                buttonText.text = noteNames[i];

            }
        }
    }

    public void deactivateNotes()
    {
        bool[] collectedNotes = GSM_script.GetCollectedNoteArray();

        for (int i = 0; i < collectedNotes.Length; i++)
        {

                string buttonName = i.ToString();
                Transform buttonTransform = _inventoryNoteCanvas.transform.Find(buttonName);
                Button b = buttonTransform.GetComponent<Button>();

                TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();


                buttonText.text = "???";
                
            
        }
    }

    public void deactivateNonBackupItems()
    {
        if(GSM_script.backup_has_ChildhoodToy == false)
        {
            
            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("0");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();


            buttonText.text = "???";
        }
        if (GSM_script.backup_has_money == false)
        {

            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("1");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();


            buttonText.text = "???";
        }
        if (GSM_script.backup_has_fireEscapePlan == false)
        {

            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("2");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();


            buttonText.text = "???";
        }
        if (GSM_script.backup_has_oldKey == false)
        {

            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("3");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();


            buttonText.text = "???";
        }
        if (GSM_script.backup_has_vipRationCard == false)
        {

            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("4");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();


            buttonText.text = "???";
        }
        if (GSM_script.backup_has_catTreat == false)
        {

            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("5");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();


            buttonText.text = "???";
        }

    }

    void activateCollectedItems()
    {
        if(GSM_script.has_ChildhoodToy)
        {
            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("0");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

            buttonText.text = "Childhood Toy";
        }
        if (GSM_script.has_money)
        {
            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("1");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

            buttonText.text = "money";
        }
        if (GSM_script.has_fireEscapePlan)
        {
            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("2");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

            buttonText.text = "fire escape plan";
        }
        if (GSM_script.has_oldKey)
        {
            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("3");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

            buttonText.text = "old key";
        }
        if (GSM_script.has_catTreat)
        {
            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("4");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

            buttonText.text = "Cat Treat";
        }
        if (GSM_script.has_photo)
        {
            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("5");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

            buttonText.text = "Photo";
        }



    }

    void giveAway_ChildhoodToy()
    {
        GSM_script.has_ChildhoodToy = false;
        GSM_script.givenAway_ChildhoodToy = true;

        Transform buttonTransform = _inventoryItemsCanvas.transform.Find("0");
        Button b = buttonTransform.GetComponent<Button>();

        TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

        buttonText.text = "(Given Away)";
    }

    void giveAway_fireEscapePlan()
    {
        GSM_script.has_fireEscapePlan = false;
        GSM_script.givenAway_fireEscapePlan = true;

        Transform buttonTransform = _inventoryItemsCanvas.transform.Find("1");
        Button b = buttonTransform.GetComponent<Button>();

        TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

        buttonText.text = "(Given Away)";
    }

    void giveAway_oldKey()
    {
        GSM_script.has_oldKey = false;
        GSM_script.givenAway_oldKey = true;

        Transform buttonTransform = _inventoryItemsCanvas.transform.Find("2");
        Button b = buttonTransform.GetComponent<Button>();

        TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

        buttonText.text = "(Given Away)";
    }


    private void openMainMenu()
    {
        _mainMenuCanvas.SetActive(true);
        _settingsMenuCanvas.SetActive(false);
        _inventoryMenuCanvas.SetActive(false);
        _inventoryNoteCanvas.SetActive(false);
        _inventoryItemsCanvas.SetActive(false);
        _DeathMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirstSelected);
    }

    private void openDeathMenu()
    {
        SoundManager.instance.PlaySoundEffect(deathSound, transform, 1.0f);

        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        _inventoryMenuCanvas.SetActive(false);
        _inventoryNoteCanvas.SetActive(false);
        _inventoryItemsCanvas.SetActive(false);
        _DeathMenuCanvas.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_deathScreenFirstSelected);
    }

    private void closeAllMenus()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        _inventoryMenuCanvas.SetActive(false);
        _inventoryNoteCanvas.SetActive(false);
        _inventoryItemsCanvas.SetActive(false);
        _DeathMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void openSettingsMenu()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(true);
        _inventoryMenuCanvas.SetActive(false);
        _inventoryNoteCanvas.SetActive(false);
        _inventoryItemsCanvas.SetActive(false);
        _DeathMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_settingsMenuFirstSelected);
    }

    private void openInventoryMenu()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        _inventoryMenuCanvas.SetActive(true);
        _inventoryNoteCanvas.SetActive(false);
        _inventoryItemsCanvas.SetActive(false);
        _DeathMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_inventoryMenuFirstSelected);
    }

    private void openInventoryNoteMenu()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        _inventoryMenuCanvas.SetActive(false);
        _inventoryNoteCanvas.SetActive(true);
        _inventoryItemsCanvas.SetActive(false);
        _DeathMenuCanvas.SetActive(false);

        activateCollectedNotes();

        //Modify Canvas to display active notes

        EventSystem.current.SetSelectedGameObject(_inventoryNotesFirstSelected);
    }

    private void openInventoryNoteMenu_NoSound()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        _inventoryMenuCanvas.SetActive(false);
        _inventoryNoteCanvas.SetActive(true);
        _inventoryItemsCanvas.SetActive(false);
        _DeathMenuCanvas.SetActive(false);

        activateCollectedNotes();

    }

    private void openInventoryItemsMenu()
    {
        activateCollectedItems();

        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        _inventoryMenuCanvas.SetActive(false);
        _inventoryNoteCanvas.SetActive(false);
        _inventoryItemsCanvas.SetActive(true);
        _DeathMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_inventoryItemsFirstSelected);
    }

    public void reloadCurrentScene()
    {
        
        GSM_script.wipeData();
        GSM_script.restoreBackupData();

        //deadState = false;
        //GSM_script.isPlayerDead = false;
        Scene currentScene = SceneManager.GetActiveScene();

        // Get the build index (ID) of the active scene
        int sceneID = currentScene.buildIndex;

        loadScene(sceneID);
    }

    public void loadScene(int sceneNum)
    {
        //GSM_script.isPlayerDead = false;

        basicMouseLook_script.registerMouse = true;

        Unpause();

        
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

            
        SceneManager.LoadScene(sceneNum);
        
    }

    // This function will be called automatically when the scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GlobalStateManagerObj = GameObject.FindWithTag("GSO");
        GSM_script = GlobalStateManagerObj.GetComponent<GlobalStateManager>();

        PlayerObj = GameObject.FindWithTag("Player");
        // Set the player's position and rotation after the scene reloads
        PlayerObj.transform.position = GSM_script.level_0_startPos;
        PlayerObj.transform.rotation = GSM_script.level_0_startRot;
        Physics.SyncTransforms();

        GSM_script.restoreBackupData();

        // Unsubscribe from the event to prevent duplicate calls in the future
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0.0f;

        openMainMenu();

    }

    private void DeathPause()
    {
        //deadState = false;
        isPaused = true;
        //GSM_script.shownOnce = false;

        for (int i = 0; i < notePanels.Length; i++)
        {
            notePanels[i].SetActive(false);
        }

        Time.timeScale = 0.0f;

        openDeathMenu();

    }

    private void UnDeathPause()
    {
        //deadState = false;
        isPaused = false;
        //GSM_script.shownOnce = false;

        for (int i = 0; i < notePanels.Length; i++)
        {
            notePanels[i].SetActive(false);
        }

        Time.timeScale = 1.0f;

        closeAllMenus();

    }

    private void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1.0f;

        for(int i = 0; i < notePanels.Length; i++)
        {
            notePanels[i].SetActive(false);
        }
        
        closeAllMenus();
    }

    //Button press callbacks:
    #region 

    public void OnResumeButtonPress()
    {
        //deadState = false;
        SoundManager.instance.PlaySoundEffect(resumeSound, transform, 1.0f);
        Unpause();
    }

    public void OnRespawnButtonPress()
    {
        SoundManager.instance.PlaySoundEffect(respawnSound, transform, 1.0f);
        //deadState = false;
        //SoundManager.instance.PlaySoundEffect(resumeSound, transform, 1.0f);
        //Unpause();
        reloadCurrentScene();
    }

    public void LoadNextScene(int sceneID)
    {
        
        GSM_script.backUpData();
        
        loadScene(sceneID);


    }

    public void OnSettingsButtonPress()
    {
        PlayButtonPressSound();
        openSettingsMenu();
    }

    public void OnSettingsBackButtonPressed()
    {
        PlayButtonPressSound();
        openMainMenu();
    }

    public void OnInventoryButtonPressed()
    {

        for (int ii = 0; ii < notePanels.Length; ii++)
        {
            notePanels[ii].SetActive(false);
        }

        PlayButtonPressSound();
        openInventoryMenu();

    }

    public void OnInventoryNoteButtonPressed()
    {
        PlayButtonPressSound();
        openInventoryNoteMenu();

        for (int ii = 0; ii < notePanels.Length; ii++)
        {
            notePanels[ii].SetActive(false);
        }
    }

    public void OnQuitGameButtonPressed()
    {
        // Exits the game when built and running on a device or standalone build
        UnityEngine.Application.Quit();

        // If running in the Unity Editor, stop play mode (for testing purposes)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OnInventoryItemButtonPressed()
    {
        PlayButtonPressSound();
        openInventoryItemsMenu();
    }

    public void OnAudioButtonPress()
    {
        SoundManager.instance.PlaySoundEffect(reloadSound, transform, 1.0f);
    }

    private void PlayButtonPressSound()
    {
        SoundManager.instance.PlaySoundEffect(buttonPressSound, transform, 1.0f);
    }

    public void OnMainMenuButtonPress()
    {
        PlayButtonPressSound();
        Unpause();
    }

    public void OnEscButtonPress()
    {
        PlayButtonPressSound();
        openMainMenu();
        Pause();
    }

    public void DisplayNoteOnMenu(int i)
    {
        Pause();

        openInventoryNoteMenu_NoSound();

        EventSystem.current.SetSelectedGameObject(pickupSelected[i]);

        notePanels[i].SetActive(true);
    }

    public void DisplayKeyItem(int i)
    {
        Pause();

        openInventoryItemsMenu();

        if(i == 0)
            EventSystem.current.SetSelectedGameObject(childhoodToy_selected);

        if(i == 1)
            EventSystem.current.SetSelectedGameObject(oldKey_selected);

        if(i == 2)
            EventSystem.current.SetSelectedGameObject(CodeBreaker_selected);

        if (i == 3)
            EventSystem.current.SetSelectedGameObject(Torch_selected);

        if (i == 4)
            EventSystem.current.SetSelectedGameObject(vipRationCard_selected);

        if (i == 5)
            EventSystem.current.SetSelectedGameObject(_catTreat_selected);

        if (i == 6)
            EventSystem.current.SetSelectedGameObject(_photo_selected);


        //notePanels[i].SetActive(true);
    }

    public void DisplayNote(int i)
    {

        for (int ii = 0; ii < notePanels.Length; ii++)
        {
            notePanels[ii].SetActive(false);
        }

        if (GSM_script.GetCollectedNoteArray()[i])
        {
            notePanels[i].SetActive(true);
        }

    }

    public void SetLowVolume()
    {
        menuOptions.SetVolume(-10);
        PlayButtonPressSound();
    }

    public void SetMediumVolume()
    {
        menuOptions.SetVolume(0);
        PlayButtonPressSound();
    }

    public void SetHighVolume()
    {
        menuOptions.SetVolume(20);
        PlayButtonPressSound();
    }

    public void SetMute()
    {
        menuOptions.SetVolume(-80);
        PlayButtonPressSound();
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        PlayButtonPressSound();
    }

    public void SetHighFideltyQuality()
    {
        menuOptions.SetQuality(2);
        PlayButtonPressSound();
    }

    public void SetBalancedQuality()
    {
        menuOptions.SetQuality(1);
        PlayButtonPressSound();
    }

    public void SetPerformantQuality()
    {
        menuOptions.SetQuality(0);
        PlayButtonPressSound();
    }

    #endregion
}
