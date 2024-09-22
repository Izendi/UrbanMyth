namespace Assets.Scripts.DialogueSystem.Models
{
    [System.Serializable]
    public class DialogueNode
    {
        public int DialogueId { get; set; }
        public string npcName{ get; set; }
        public string Text { get; set; }
        public Response[] Responses { get; set; }

    }
}