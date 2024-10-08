using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public bool IsPaused {  get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0.0f;

        //GlobalStateManager.inputActions.SwitchCurrent
    }

    public void UnpauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1.0f;
    }
}
