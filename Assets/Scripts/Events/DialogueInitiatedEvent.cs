using UnityEngine;

namespace Assets.Scripts.Events
{
    public class DialogueInitiatedEvent : IEvent
    {
        public TextAsset DialogueFile { get; set; }
        public int? StartNodeId { get; set; }
    }
}