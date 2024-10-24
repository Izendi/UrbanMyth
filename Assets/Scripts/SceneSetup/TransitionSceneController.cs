using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public GameObject[] transitionImages;  // Array to store multiple images (Img1, Img2, Img3)
    public float displayDuration = 3.0f;   // Duration to show each image
    private int currentImageIndex = 0;     // Track the current image
    private bool canMoveToNextLevel = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject img in transitionImages)
        {
            img.SetActive(false);
        }

        // Start by showing the first image
        if (transitionImages.Length > 0)
        {
            ShowNextImage();
        }
    }

    private void ShowNextImage()
    {
        if (currentImageIndex < transitionImages.Length)
        {
            // Activate the current image
            transitionImages[currentImageIndex].SetActive(true);

            // Schedule the next image to be shown after the current one has been displayed
            Invoke(nameof(HideCurrentAndShowNext), displayDuration);
        }
        else
        {
            // After all images are shown, allow the player to move to the next level
            AllowPlayerToMove();
        }
    }

    private void HideCurrentAndShowNext()
    {
        // Deactivate the current image
        transitionImages[currentImageIndex].SetActive(false);

        // Move to the next image
        currentImageIndex++;

        // Show the next image in sequence
        ShowNextImage();
    }

    // Allow the player to move after all images are shown
    private void AllowPlayerToMove()
    {
        canMoveToNextLevel = true;
    }
}

    // Update is called once per frame
    //void Update()
    //{

    //}

