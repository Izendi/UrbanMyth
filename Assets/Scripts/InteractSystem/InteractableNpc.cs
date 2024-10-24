using System.Collections;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.DialogueSystem.Models;
using Assets.Scripts.Events;
using UnityEngine;  

public class InteractableNpc : InteractableObject, IEventHandler<NewDialogueStartNodeEvent>, IEventHandler<DialogueEndedEvent>
{
    [SerializeField]
    protected TextAsset DialogueFile; // The dialogue file to be used for this NPC
    [SerializeField]
    public SpriteRenderer spriteRenderer;
    private Color originalColor => spriteRenderer.color;


    protected Dialogue dialogue;
    public float fadeDuration = 0.05f;

    private string npcName => dialogue.DialogueNodes.FirstOrDefault()?.NpcName;


    private int startNodeId = 1;
    private Vector3? moveToPosition;

    public override void Interact()
    {
        TriggerDialogue();
    }

    private void Start()
    {
        EventAggregator.Instance.Subscribe<NewDialogueStartNodeEvent>(this);
        EventAggregator.Instance.Subscribe<DialogueEndedEvent>(this);
        dialogue = JsonUtility.FromJson<Dialogue>(DialogueFile.text);
        if (string.IsNullOrEmpty(InteractPrompt))
            InteractPrompt = "Press E to talk.";
    }

    public virtual void TriggerDialogue()
    {
        StopAllCoroutines();
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        EventAggregator.Instance.Publish(new DialogueInitiatedEvent { Dialogue = dialogue, StartNodeId = startNodeId});
    }

    public void setDialogueFile(TextAsset df)
    {
        DialogueFile = df;
    }

    public void Handle(NewDialogueStartNodeEvent @event)
    {
        if (@event.NpcName != npcName)
            return;

        if (@event.NewStartNodeId.HasValue)
            startNodeId = @event.NewStartNodeId.Value;
        if (@event.NewPosition.HasValue)
            moveToPosition = @event.NewPosition.Value;
    }

    public void Handle(DialogueEndedEvent @event)
    {
        if (@event.NpcName != npcName)
            return;
        Debug.Log(moveToPosition);
        if (moveToPosition.HasValue)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    void OnDestroy()
    {
        EventAggregator.Instance.Unsubscribe<NewDialogueStartNodeEvent>(this);
        EventAggregator.Instance.Unsubscribe<DialogueEndedEvent>(this);
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (spriteRenderer != null)
        {
            float currentTime = 0f;

            // Gradually fade out
            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1, 0, currentTime / fadeDuration);  // Interpolate alpha
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;  // Wait for the next frame
            }

            // Ensure the sprite is completely invisible
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            
            if(moveToPosition.HasValue)
                transform.position =  moveToPosition.Value;

            moveToPosition = null;

            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        }
    }
}
