using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catSetup : MonoBehaviour
{
    private GameObject GSM;
    private GlobalStateManager GSM_script;

    private GameObject LOI;
    private LoadOnInteract LOI_script;

    public TextAsset NoCatTreatDialogue;
    public TextAsset HasCatTreatDialogue;

    IEnumerator Start()
    {
        // Wait until the end of the frame to ensure all Start methods have been called
        yield return new WaitForEndOfFrame();

        GSM = GameObject.FindGameObjectWithTag("GSO");
        GSM_script = GSM.GetComponent<GlobalStateManager>();

        // Modify other game objects here
        ModifyOtherObjects();

    }

    void ModifyOtherObjects()
    {
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("NPC");

        InteractableNpc catScript;

        for (int i = 0; i < NPCs.Length; i++)
        {
            if (NPCs[i].name == "Cat")
            {
                

                catScript = NPCs[i].GetComponent<InteractableNpc>();

                if (GSM_script.has_catTreat)
                {
                    catScript.DialogueFile = HasCatTreatDialogue;
                }
                else
                {
                    catScript.DialogueFile = NoCatTreatDialogue;
                }
            };

        }




        // You can modify specific objects using their tags, names, or components as well
    }

}


