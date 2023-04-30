using Events;

namespace Dialogue.EventImplementations
{
    public enum DialogueEventType
    {
        Start = 0,
        Finish = 1,
        OptionSelected = 2,
        Load = 3,
    }
    
    public class DialogueEvent : Event<DialogueEvent>
    {
        public DialogueNode DialogueNode;
        public Dialogue Dialogue;
        public int OptionIndex;
        
        public static DialogueEvent Get(DialogueNode node)
        {
            var evt = GetPooledInternal();
            evt.DialogueNode = node;

            return evt;
        }

        public static DialogueEvent Get(int optionIndex)
        {
            var evt = GetPooledInternal();
            evt.OptionIndex = optionIndex;

            return evt;
        }

        public static DialogueEvent Get(Dialogue dialogue)
        {
            var evt = GetPooledInternal();
            evt.Dialogue = dialogue;

            return evt;
        }
    }
}