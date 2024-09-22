using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialogueObject;
    private Queue<string> lines;
    // Start is called before the first frame update
    void Start()
    {
        lines = new Queue<string>();
        HideDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartDialogue(new Dialogue{npcName = "hans"});
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation with " + dialogue.npcName);
        ShowDialogue();

    }

    public void HideDialogue()
    {
        DialogueObject.SetActive(false);
    }

    public void ShowDialogue()
    {
        DialogueObject.SetActive(true);
    }
}
