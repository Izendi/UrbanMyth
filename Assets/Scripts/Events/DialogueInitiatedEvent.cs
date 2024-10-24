using Assets.Scripts.DialogueSystem.Models;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class DialogueInitiatedEvent : IEvent
    {
        public Dialogue Dialogue { get; set; }
        public int? StartNodeId { get; set; }
    }
}