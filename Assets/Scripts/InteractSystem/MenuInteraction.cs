using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInteraction : MonoBehaviour
{
    public bool MenuOpenCloseInput { get; private set; }

    [SerializeField]
    private GameObject _mainMenuCanvas;

    [SerializeField]
    private GameObject _settingsMenuCanvas;

    [SerializeField]
    private KeyCode menuKey = KeyCode.Z;

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

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0.0f;

        _mainMenuCanvas.SetActive(true);
    }

    private void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1.0f;

        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
    }
}
