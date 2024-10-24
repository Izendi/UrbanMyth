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
        if (GlobalStateManager.Instance.has_money)
        {
            // Enable NPC2
            gameObject.SetActive(true);

            
        }
    }
}
