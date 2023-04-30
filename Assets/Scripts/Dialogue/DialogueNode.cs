using System.Collections.Generic;
using UnityEngine;
using Videos;

namespace Dialogue
{
    public enum DialogueActor
    {
        Player = 0,
        Designer = 1,
        Developer = 2,
        Artist = 3,
        All = 4
    }
    
    [CreateAssetMenu(menuName = "Dialogue/Dialogue Node")]
    public class DialogueNode : ScriptableObject
    {
        public DialogueActor Speaker;
        public string DialogueLine;
        
        public List<string> DialogueOptions = new List<string>();
        public List<DialogueNode> NextNodes = new List<DialogueNode>();
        
        public VideoData DialogueVideo;
    }

    [CreateAssetMenu(menuName = "Dialogue/Dialogue Tree")]
    public class DialogueTree : ScriptableObject
    {
        public List<DialogueNode> DialogueNodes = new List<DialogueNode>();
    }
}
