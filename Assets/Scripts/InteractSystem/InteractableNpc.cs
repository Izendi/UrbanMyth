using Assets.Scripts;
using Assets.Scripts.Contracts;
using Assets.Scripts.Events;
using UnityEngine;  

public class InteractableNpc : InteractableObject, IEventHandler<NewDialogueStartNodeEvent>, IEventHandler<DialogueEndedEvent>
{
    [SerializeField]
    protected TextAsset DialogueFile; // The dialogue file to be used for this NPC

    private int startNodeId = 1;
    private Vector3? moveToPosition;

    public override void Interact()
    {
        TriggerDialogue();
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(InteractPrompt))
            InteractPrompt = "Press E to talk.";
    }

    public virtual void TriggerDialogue()
    {
        EventAggregator.Instance.Subscribe<NewDialogueStartNodeEvent>(this);
        EventAggregator.Instance.Subscribe<DialogueEndedEvent>(this);
        EventAggregator.Instance.Publish(new DialogueInitiatedEvent { DialogueFile = DialogueFile, StartNodeId = startNodeId});
    }

    public void setDialogueFile(TextAsset df)
    {
        DialogueFile = df;
    }

    public void Handle(NewDialogueStartNodeEvent @event)
    {
        if (@event.NewStartNodeId.HasValue)
            startNodeId = @event.NewStartNodeId.Value;
        if (@event.NewPosition.HasValue)
            moveToPosition = @event.NewPosition.Value;
    }

    public void Handle(DialogueEndedEvent @event)
    {
        EventAggregator.Instance.Unsubscribe<NewDialogueStartNodeEvent>(this);
        EventAggregator.Instance.Unsubscribe<DialogueEndedEvent>(this);

        if(moveToPosition.HasValue)
            transform.position = moveToPosition.Value;
    }

    void OnDestroy()
    {
        EventAggregator.Instance.Unsubscribe<NewDialogueStartNodeEvent>(this);
        EventAggregator.Instance.Unsubscribe<DialogueEndedEvent>(this);
    }
}
