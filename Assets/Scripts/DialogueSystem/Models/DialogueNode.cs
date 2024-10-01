using System.Collections.Generic;

namespace Assets.Scripts.DialogueSystem.Models
{
    [System.Serializable]
    public class DialogueNode
    {
        public int DialogueId;
        public string NpcName;
        public string Text;
        public List<Response> Responses;

    }
}