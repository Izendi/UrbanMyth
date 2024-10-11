using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour
{
    public GameObject GlobalStateManagerObj;
    private GlobalStateManager GSM_script;
    public bool MenuOpenCloseInput { get; private set; }

    public string[] noteNames = { "Dear Sister", "17/12/2046", "Hidden Key", "Trust Me"};

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

    

    private void Start()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);

        GSM_script = GlobalStateManagerObj.GetComponent<GlobalStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(menuKey) && isPaused == false)
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
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        _inventoryMenuCanvas.SetActive(false);
        _inventoryNoteCanvas.SetActive(false);
        _inventoryItemsCanvas.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_inventoryItemsFirstSelected);
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
