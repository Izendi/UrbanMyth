using System.Collections.Generic;

namespace Assets.Scripts.DialogueSystem.Models
{
    [System.Serializable]
    public class DialogueNode
    {
        public int DialogueId { get; set; }
        public string NpcName{ get; set; }
        public string Text { get; set; }
        public List<Response> Responses { get; set; }

    }
}