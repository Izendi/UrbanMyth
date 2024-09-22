namespace Assets.Scripts.DialogueSystem.Models
{
    [System.Serializable]
    public class Response
    {
        public string Text { get; set; }
        public int NextDialogueId { get; set; }
    }
}