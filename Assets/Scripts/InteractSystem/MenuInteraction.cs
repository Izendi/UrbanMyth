using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuInteraction : MonoBehaviour
{
    public bool MenuOpenCloseInput { get; private set; }

    [SerializeField]
    private GameObject _mainMenuCanvas;

    [SerializeField]
    private GameObject _settingsMenuCanvas;

    [SerializeField]
    private GameObject _mainMenuFirstSelected;

    [SerializeField]
    private GameObject _settingsMenuFirstSelected;

    [SerializeField]
    private KeyCode menuKey = KeyCode.Z;

    [SerializeField]
    private AudioClip reloadSound;

    [SerializeField]
    private AudioClip buttonPressSound;

    [SerializeField]
    private AudioClip resumeSound;

    private bool isPaused = false;

    private void Start()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(menuKey) && isPaused == false)
        {
            Pause();
        }
        else if (Input.GetKeyUp(menuKey) && isPaused == true)
        {
            Unpause();
        }
    }

    private void openMainMenu()
    {
        _mainMenuCanvas.SetActive(true);
        _settingsMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirstSelected);
    }

    private void openSettingsMenu()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_settingsMenuFirstSelected);
    }

    private void closeAllMenus()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);

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

    #endregion
}
