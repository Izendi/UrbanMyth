namespace Dialogue.Models;

[Serializable]
public class DialogueNode
{
    public int DialogueId;
    public string NpcName;
    public string Text;
    public List<Response> Responses;

}
