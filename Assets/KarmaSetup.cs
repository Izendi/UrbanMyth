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

        LOI_script.NoSceneToLoad = 8;

        if (GSM_script.KarmaLevel < 0)
            {
                for (int i = 0; i < NPCs.Length; i++)
                {
                    if (NPCs[i].name == "TheWitch")
                    {
                        //Set Script A
                    }
                    if (NPCs[i].name == "Hansel")
                    {
                        //Set Script B
                    }

                    LOI_script.NoSceneToLoad = 9;
            }
            }
            else if (GSM_script.KarmaLevel > 2 && GSM_script.has_fireEscapePlan && GSM_script.has_oldKey && GSM_script.hasShownPhoto)
            {
                // set up scene for best ending

            }
            else if (GSM_script.KarmaLevel > 2)
            {

            }
        

        // You can modify specific objects using their tags, names, or components as well
    }

}
