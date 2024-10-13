using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    private static DontDestroyOnLoadScript instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates if one already exists
        }
    }
}
