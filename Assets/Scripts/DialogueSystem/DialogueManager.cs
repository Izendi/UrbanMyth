using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DialogueSystem.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialogueObject;
    public TextMeshProUGUI DialogueText;
    public GameObject ResponsePrefab;
    public Transform ResponseContainer;
    public TextAsset DialogueFile;
    public float PrintSpeed = 0.05f;
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
        currentDialogueNode = CurrentDialogue.DialogueNodes.Find(x => x.DialogueId == nodeId);
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
        DisplayResponses();


    }

    private IEnumerator TypeNpcLine()
    {
        DialogueText.text = "";
        foreach (char c in currentDialogueNode.Text)
        {
            DialogueText.text += c;
            yield return new WaitForSeconds(PrintSpeed);
        }

    }

    private void DisplayResponses()
    {
        foreach (Transform child in ResponseContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var response in currentDialogueNode.Responses)
        {
            GameObject option = Instantiate(ResponsePrefab, ResponseContainer);
            Button button = option.GetComponent<Button>();
            TextMeshProUGUI buttonText = option.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = response.Text; // Set button text
            // button.onClick.AddListener(() => OnOptionSelected(response.NextDialogueId)); // Assign action
        }
    }

    private void OnOptionSelected(int nextDialogueId)
    {
        // Handle what happens when the option is selected, e.g., load next dialogue
        Debug.Log("Selected option for dialogue ID: " + nextDialogueId);
    }
}
