using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.DialogueSystem.Models;
using Assets.Scripts.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    private const float PRINT_SPEED = 0.01f;

    public static DialogueManager Instance;

    public static bool IsDialogueActive => Instance?.isDialogueActive ?? false;

    private bool isDialogueActive = false;
   
    [SerializeField] private GameObject DialogueObject;
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private TextMeshProUGUI CurrentNpcName;
    [SerializeField] private GameObject PressEnterToClosePrefab;
    [SerializeField] private GameObject ResponsePrefab;
    [SerializeField] private Transform ResponseContainer; 
    private Dialogue CurrentDialogue;
    private DialogueNode currentDialogueNode;

    public disableReticle dr;
    public Canvas reticleCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject); // Destroy duplicates when reloading
        }
    }

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

            dr.enable(ref reticleCanvas);
        }

        foreach (var response in currentDialogueNode.Responses)
        {
            GameObject responseObject = Instantiate(ResponsePrefab, ResponseContainer);
            var button = responseObject.GetComponent<Button>();
            var buttonText = responseObject.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = response.Text; // Set button text
            button.onClick.AddListener(() => OnResponseSelected(response.NextDialogueId)); // Assign action
        }
    }

    private void OnResponseSelected(int nextDialogueId)
    {
        PrintDialogueText(nextDialogueId);
    }

    private void CloseDialogue()
    {
        isDialogueActive = false;
        EventAggregator.Instance.Publish(new DialogueEndedEvent());
        HideDialogue();
    }
}
