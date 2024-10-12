﻿namespace Assets.Scripts.DialogueSystem.Models
{
    [System.Serializable]
    public class Response
    {
        public string Text;
        public int NextDialogueId;
        public int ResponseNiceness; // 0 = neutral, >0 = nice, <0 mean
        public string Action;
    }
}