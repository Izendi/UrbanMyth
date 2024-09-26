using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DialogueSystem.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    private const float PRINT_SPEED = 0.01f;

    public static DialogueManager Instance;

    public bool IsDialogueActive => isDialogueActive;

    private bool isDialogueActive = false;
   
    [SerializeField] private GameObject DialogueObject;
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private TextMeshProUGUI CurrentNpcName;
    [SerializeField] private GameObject PressEnterToClosePrefab;
    [SerializeField] private GameObject ResponsePrefab;
    [SerializeField] private Transform ResponseContainer; 
    private Dialogue CurrentDialogue;
    private DialogueNode currentDialogueNode;

    void Start()
    {
        HideDialogue();
    }

    void Update()
    {
        if (!isDialogueActive)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            CloseDialogue();

        if (Input.GetKeyDown(KeyCode.Return) && !currentDialogueNode.Responses.Any())
        {
           CloseDialogue();
        }
    }

    public void StartDialogue(TextAsset dialogueFile, int startNode = 1)
    {
        CurrentDialogue = JsonUtility.FromJson<Dialogue>(dialogueFile.text);
        isDialogueActive = true;
        StartDialogue(startNode);
    }

    private void StartDialogue(int nodeId)
    {
        if( CurrentDialogue is null)
            return;

        isDialogueActive = true;
        DialogueObject.SetActive(true);

        PrintDialogueText(nodeId);
    }

    private void HideDialogue()
    {
        DialogueObject.SetActive(false);
    }

    private void PrintDialogueText(int nodeId)
    {
        currentDialogueNode = CurrentDialogue.DialogueNodes.Find(x => x.DialogueId == nodeId);
        CurrentNpcName.text = currentDialogueNode.NpcName;
        StartCoroutine(TypeNpcLine());
        DisplayResponses();
    }

    private void Awake()
    {
        Instance = this; // Set the singleton instance
    }

    private IEnumerator TypeNpcLine()
    {
        DialogueText.text = "";
        foreach (char c in currentDialogueNode.Text)
        {
            DialogueText.text += c;
            yield return new WaitForSeconds(PRINT_SPEED);
        }

    }

    private void DisplayResponses()
    {
        foreach (Transform child in ResponseContainer)
        {
            Destroy(child.gameObject);
        }

        if (!currentDialogueNode.Responses?.Any() ?? true)
        {
            GameObject textObject = Instantiate(PressEnterToClosePrefab, ResponseContainer);
            var text = textObject.GetComponent<TextMeshProUGUI>();

            var stringToPrint = "Press Enter to close...";
            text.text = stringToPrint;
        }

        foreach (var response in currentDialogueNode.Responses)
        {
            GameObject responseObject = Instantiate(ResponsePrefab, ResponseContainer);
            var button = responseObject.GetComponent<Button>();
            var buttonText = responseObject.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = response.Text; // Set button text
            button.onClick.AddListener(() => OnResponseSelected(response.NextDialogueId)); // Assign action
            Debug.Log("AddListener");
        }
    }

    private void OnResponseSelected(int nextDialogueId)
    {
        Debug.Log("Response Clicked");
        PrintDialogueText(nextDialogueId);
    }

    private void CloseDialogue()
    {
        isDialogueActive = false;
        HideDialogue();
    }
}
