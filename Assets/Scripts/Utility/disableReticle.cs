using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableReticle : MonoBehaviour
{

    private Canvas reticleCanvas;
    void Start()
    {
        // Find the canvas in the scene by tag or name (if it is lost after reload)
        GameObject canvasObject = GameObject.FindWithTag("RetCanvas"); // Or use GameObject.Find("ReticleCanvasName")
        if (canvasObject != null)
        {
            reticleCanvas = canvasObject.GetComponent<Canvas>();
        }
        else
        {
            Debug.LogError("Canvas with the specified tag or name not found.");
        }
    }

    public void disable()
    {
        GameObject canvasObject = GameObject.FindWithTag("RetCanvas");
        //reticleCanvas = canvasObject.GetComponent<Canvas>();

        if (reticleCanvas != null)
        {
            canvasObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Canvas reference is null!");
        }
    }

    public void enable()
    {
        GameObject canvasObject = GameObject.FindWithTag("RetCanvas");
        reticleCanvas = canvasObject.GetComponent<Canvas>();

        if (reticleCanvas != null)
        {
            canvasObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Canvas reference is null!");
        }
    }
}
