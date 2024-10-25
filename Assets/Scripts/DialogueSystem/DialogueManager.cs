using System.Collections;
using System;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.DialogueSystem.Models;
using Assets.Scripts.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour, IEventHandler<DialogueInitiatedEvent>
{
    private const float PRINT_SPEED = 0.01f;

    public GameObject GlobalStateManagerObj;
    private GlobalStateManager GSM_script;

    public static bool IsDialogueActive { get; private set; }

    [SerializeField] private GameObject DialogueObject;
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private TextMeshProUGUI CurrentNpcName;
    [SerializeField] private GameObject PressEnterToClosePrefab;
    [SerializeField] private GameObject ResponsePrefab;
    [SerializeField] private Transform ResponseContainer;
    [SerializeField] private Canvas DialogueCanvas;
    [SerializeField] private Canvas MemoryCanvas;
    [SerializeField] private bool ShowMemory;
    [SerializeField] private Button CloseMemoryButton;
    private Dialogue CurrentDialogue;
    private DialogueNode currentDialogueNode;
    private bool isPrinting = false;

    void Awake()
    {
        GlobalStateManagerObj = GameObject.FindWithTag("GSO");
        GSM_script = GlobalStateManagerObj.GetComponent<GlobalStateManager>();
    }

    void Start()
    {
        HideDialogue();
        IsDialogueActive = false;
        EventAggregator.Instance.Subscribe(this);

        if (ShowMemory)
        {
            StartMemory();
        }
        else
        {
            CloseMemory();
        }
    }

    void Update()
    {
        if(GlobalStateManagerObj == null)
        {
            GlobalStateManagerObj = GameObject.FindWithTag("GSO");
            GSM_script = GlobalStateManagerObj.GetComponent<GlobalStateManager>();
        }

        if (!IsDialogueActive)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            CloseDialogue();

        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            CloseMemory();

            if (!currentDialogueNode?.Responses.Any() ?? false)
            {
                CloseDialogue();
            }
            else if (isPrinting)
            {
                isPrinting = false;
                this.StopAllCoroutines();
                DialogueText.text = currentDialogueNode.Text;
            }
        }
    }

    private void OnDestroy()
    {
        EventAggregator.Instance.Unsubscribe<DialogueInitiatedEvent>(this);
        GlobalStateManagerObj = null;
        GSM_script = null;
    }

    private void StartMemory()
    {
        MemoryCanvas.gameObject.SetActive(true);
        DialogueCanvas.gameObject.SetActive(false);
        CloseMemoryButton.onClick.AddListener(CloseMemory);
        IsDialogueActive = true;
    }

    private void CloseMemory()
    {
        ShowMemory = false;
        MemoryCanvas.gameObject.SetActive(false);
        CloseDialogue();
    }

    public void StartDialogue(Dialogue dialogue, int? startNode = 1)
    {
        CurrentDialogue = dialogue;
        IsDialogueActive = true;
        DialogueCanvas.gameObject.SetActive(true);
        MemoryCanvas.gameObject.SetActive(false);
        DialogueObject.SetActive(true);
        StartDialogue(startNode ?? 1);
    }

    public void Handle(DialogueInitiatedEvent @event)
    {
        Debug.Log(@event.StartNodeId);

        StartDialogue(@event.Dialogue, @event.StartNodeId);
    }

    private void StartDialogue(int nodeId)
    {
        if( CurrentDialogue is null)
            return;

        IsDialogueActive = true;
        DialogueObject.SetActive(true);

        PrintDialogueText(nodeId);
    }

    private void HideDialogue()
    {
        DialogueObject.SetActive(false);
        IsDialogueActive = false;
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
        isPrinting = true;

        DialogueText.text = "";
        foreach (char c in currentDialogueNode.Text)
        {
            DialogueText.text += c;
            yield return new WaitForSeconds(PRINT_SPEED);
        }

        isPrinting = false;
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

            button.onClick.AddListener(() => OnResponseSelected(response.NextDialogueId, response.Action)); // Assign action
        }
    }

    private void OnResponseSelected(int nextDialogueId, string actionsString)
    {
        if (isPrinting)
        {
            isPrinting = false;
            this.StopAllCoroutines();
        }


        if (!string.IsNullOrEmpty(actionsString))
        {
            var actions = actionsString.Split(";");

            foreach (var action in actions)
            {
                GSM_script.DoAction(action);

            }
        }
        
        if (nextDialogueId == 0)
            CloseDialogue();
        else
            PrintDialogueText(nextDialogueId);
    }

    private void CloseDialogue()
    {
        IsDialogueActive = false;
        EventAggregator.Instance.Publish(new DialogueEndedEvent{NpcName = currentDialogueNode?.NpcName});
        HideDialogue();
    }
}
