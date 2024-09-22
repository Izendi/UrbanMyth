using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DialogueSystem.Models;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialogueObject;
    public TextMeshProUGUI DialogueText;
    public TextAsset DialogueFile;
    public float PrintSpeed;
    public Dialogue CurrentDialogue;
    private DialogueNode currentDialogueNode;
    private Queue<string> lines;
    // Start is called before the first frame update
    void Start()
    {
        lines = new Queue<string>();
        CurrentDialogue = JsonUtility.FromJson<Dialogue>(DialogueFile.text);
        HideDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartDialogue(1);
        }
    }

    public void StartDialogue(int nodeId)
    {

        Debug.Log(CurrentDialogue.DialogueNodes.Find(x => x.DialogueId == nodeId));

        currentDialogueNode = CurrentDialogue.DialogueNodes.Find(x => x.DialogueId == nodeId);
        Debug.Log(currentDialogueNode.DialogueId);
        Debug.Log(currentDialogueNode.Text);
        ShowDialogue();
    }

    public void HideDialogue()
    {
        DialogueObject.SetActive(false);
    }

    public void ShowDialogue()
    {
        DialogueObject.SetActive(true);

        StartCoroutine(TypeNpcLine());
        //if (currentDialogueNode.Responses.Count > 0)
        //{
        //    for (int i = 0; i < currentDialogueNode.Responses.Count; i++)
        //    {
        //        Debug.Log((i + 1) + ": " + currentDialogueNode.Responses[i].Text);
        //    }
        //}
    }

    private IEnumerator TypeNpcLine()
    {
        DialogueText.text = "";

        Debug.Log(currentDialogueNode.Text);
        foreach (char c in currentDialogueNode.Text)
        {
            DialogueText.text += c;
            yield return new WaitForSeconds(PrintSpeed);
        }
    }
}
