using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KarmaSetup : MonoBehaviour
{
    private GameObject GSM;
    private GlobalStateManager GSM_script;

    private GameObject LOI;
    private LoadOnInteract LOI_script;

    IEnumerator Start()
    {
        // Wait until the end of the frame to ensure all Start methods have been called
        yield return new WaitForEndOfFrame();

        GSM = GameObject.FindGameObjectWithTag("GSO");
        GSM_script = GSM.GetComponent<GlobalStateManager>();

        LOI = GameObject.FindGameObjectWithTag("LoadDoor");
        LOI_script = LOI.GetComponent<LoadOnInteract>();

        // Modify other game objects here
        ModifyOtherObjects();

    }

    void ModifyOtherObjects()
    {
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("NPC");

        LOI_script.NoSceneToLoad = 13;

        if (GSM_script.KarmaLevel < 0) //Thrown in oven no matter how many stuffs
        {
            LOI_script.NoSceneToLoad = 12;
            GSM_script.DoAction("Hans|9.57,0,20.93|301");
            Debug.Log("Oven");

        }
        else if (GSM_script.KarmaLevel >= 2 && GSM_script.has_fireEscapePlan && GSM_script.has_oldKey && GSM_script.has_photo)
        {
            // set up scene for best ending
            GSM_script.DoAction("Hans|9.57,0,20.93|1");
            LOI_script.NoSceneToLoad = 11;
            Debug.Log("Best");

        }
        else if (GSM_script.KarmaLevel >= 1 && GSM_script.has_fireEscapePlan) //Escape Alone
        {
            GSM_script.DoAction("Hans|9.57,0,20.93|101");
            LOI_script.NoSceneToLoad = 9;
            Debug.Log("Alone");
        }
        else //Locked up
        {
            GSM_script.DoAction("Hans|9.57,0,20.93|201");
            LOI_script.NoSceneToLoad = 10;
            Debug.Log("Locked Up");
        }
        

        // You can modify specific objects using their tags, names, or components as well
    }

}
