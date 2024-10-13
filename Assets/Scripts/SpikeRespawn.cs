using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeRespawn : MonoBehaviour
{
    //[SerializeField] private Transform Player;
    //[SerializeField] private GameObject respawnPoint;

    private GameObject GlobalStateObj;
    private GlobalStateManager GSO_script;


    void Start()
    {
        if (GlobalStateObj == null)
        {
            GlobalStateObj = GameObject.FindWithTag("GSO");
            GSO_script = GlobalStateObj.GetComponent<GlobalStateManager>();
        }
    }

    void Update()
    {
        if (GlobalStateObj == null)
        {
            GlobalStateObj = GameObject.FindWithTag("GSO");
            GSO_script = GlobalStateObj.GetComponent<GlobalStateManager>();
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            GSO_script.isPlayerDead = true;
            GSO_script.shownOnce = true;
        }

        
    }


}
