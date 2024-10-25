using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalStateManager.Instance == null)
        {
            Debug.LogError("GlobalStateManager instance is null");
            //return; // Exit if the GlobalStateManager is not initialized
        }

 

        if (GlobalStateManager.Instance.has_money==true)
        {
            // Enable NPC2
            Debug.Log("GET MONEY");
            gameObject.SetActive(true);

            
        }
    }
}
