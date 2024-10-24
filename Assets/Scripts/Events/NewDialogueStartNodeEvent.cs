using UnityEngine;

namespace Assets.Scripts.Events
{
    public class NewDialogueStartNodeEvent : IEvent
    {
        public int? NewStartNodeId { get; set; }
        public Vector3? NewPosition { get; set; }
        public string NpcName { get; set; }
    }
}