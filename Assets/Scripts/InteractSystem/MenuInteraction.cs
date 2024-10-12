using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour
{
    public GameObject GlobalStateManagerObj;
    private GlobalStateManager GSM_script;
    private GameObject PlayerObj;

    public bool MenuOpenCloseInput { get; private set; }

    public string[] noteNames = { "Dear Sister", "17/12/2046", "Hidden Key", "Trust Me" };

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
    private GameObject[] notePanels;

    private bool isPaused = false;

    private void Awake()
    {
        PlayerObj = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);



        GSM_script = GlobalStateManagerObj.GetComponent<GlobalStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerObj == null)
        {
            PlayerObj = GameObject.FindWithTag("Player");
        }

        if (Input.GetKeyUp(menuKey) && isPaused == false)
        {
            GSM_script.isGamePaused = true;
            Pause();
        }
        else if (Input.GetKeyUp(menuKey) && isPaused == true)
        {
            GSM_script.isGamePaused = false;
            Unpause();
        }
    }

    public void DoAction(string actionName)
    {
        Debug.Log( $"action: {actionName}");
    }

    void activateCollectedNotes()
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

    void activateCollectedItems()
    {
        if(GSM_script.has_ChildhoodToy)
        {
            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("0");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

            buttonText.text = "Childhood Toy";
        }
        if (GSM_script.has_codeBreaker)
        {
            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("1");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

            buttonText.text = "Code Breaker";
        }
        if (GSM_script.has_oldKey)
        {
            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("2");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

            buttonText.text = "Old Key";
        }
        if (GSM_script.has_torch)
        {
            Transform buttonTransform = _inventoryItemsCanvas.transform.Find("3");
            Button b = buttonTransform.GetComponent<Button>();

            TMP_Text buttonText = b.GetComponentInChildren<TMP_Text>();

            buttonText.text = "Torch";
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

    void giveAway_codeBreaker()
    {
        GSM_script.has_codeBreaker = false;
        GSM_script.givenAway_codeBreaker = true;

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

        EventSystem.current.SetSelectedGameObject(_mainMenuFirstSelected);
    }

    private void openSettingsMenu()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(true);
        _inventoryMenuCanvas.SetActive(false);
        _inventoryNoteCanvas.SetActive(false);
        _inventoryItemsCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_settingsMenuFirstSelected);
    }

    private void openInventoryMenu()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        _inventoryMenuCanvas.SetActive(true);
        _inventoryNoteCanvas.SetActive(false);
        _inventoryItemsCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_inventoryMenuFirstSelected);
    }

    private void openInventoryNoteMenu()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        _inventoryMenuCanvas.SetActive(false);
        _inventoryNoteCanvas.SetActive(true);
        _inventoryItemsCanvas.SetActive(false);

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

        EventSystem.current.SetSelectedGameObject(_inventoryItemsFirstSelected);
    }

    public void reloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // Get the build index (ID) of the active scene
        int sceneID = currentScene.buildIndex;

        loadScene(sceneID);
    }

    public void loadScene(int sceneNum)
    {
        Unpause();
        Scene reloadScene;

        if (sceneNum == 0)
        {
            PlayerObj.transform.position = GSM_script.level_0_startPos;
            PlayerObj.transform.rotation = GSM_script.level_0_startRot;
            reloadScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(sceneNum);
        }
        else
        {
            //Do nothing!

            //reloadScene = SceneManager.GetActiveScene();
            //SceneManager.LoadScene(reloadScene.name);
        }
        
    }

    private void closeAllMenus()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        _inventoryMenuCanvas.SetActive(false);
        _inventoryNoteCanvas.SetActive(false);
        _inventoryItemsCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0.0f;

        openMainMenu();

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
        SoundManager.instance.PlaySoundEffect(resumeSound, transform, 1.0f);
        Unpause();
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

    public void DisplayNoteOnMenu(int i)
    {
        Pause();

        openInventoryNoteMenu_NoSound();

        EventSystem.current.SetSelectedGameObject(pickupSelected[i]);

        notePanels[i].SetActive(true);
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

    #endregion
}
