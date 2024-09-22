using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DialogueSystem.Models
{
    [System.Serializable]
    public class Dialogue
    {
        public string npcName;
        
        public List<DialogueNode> dialogueNodes;
    }
}

