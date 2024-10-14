using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton instance;

    void Awake()
    {
        if (instance == null)
        {
            // If no instance exists, this becomes the singleton instance
            instance = this;

            // Prevent this GameObject from being destroyed when loading a new scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
        }
    }
}
