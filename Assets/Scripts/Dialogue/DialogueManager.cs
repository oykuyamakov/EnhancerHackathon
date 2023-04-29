using System.Collections.Generic;
using Dialogue.EventImplementations;
using Events;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityCommon.Modules;
using UnityEngine;
using Videos.EventImplementations;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public List<DialogueNode> DialogueNodes = new List<DialogueNode>();

        public float AutoSkipTime;

        private DialogueNode m_CurrentNode;

        private void OnEnable()
        {
            GEM.AddListener<DialogueEvent>(OnDialogueOptionSelected, Priority.High, (int)DialogueEventType.OptionSelected);
        }

        public void LoadDialogueNode(int index)
        {
            LoadDialogueNode(DialogueNodes[index]);
        }

        [Button]
        public void LoadDialogueNode(DialogueNode dialogueNode)
        {
            m_CurrentNode = dialogueNode;
            
            var dialogueVideo = dialogueNode.DialogueVideo;

            if (dialogueVideo != null)
            {
                using var videoEvt = VideoEvent.Get(dialogueVideo).SendGlobal((int)VideoEventType.Play);
            }
            
            using var dialogueEvent = DialogueEvent.Get(dialogueNode).SendGlobal((int)DialogueEventType.Start);

            if (m_CurrentNode.DialogueOptions.IsNullOrEmpty())
            {
                Conditional.Wait(AutoSkipTime)
                    .Do(() =>
                    {
                        FinishDialogue();
                    });
            }
        }

        public void OnDialogueOptionSelected(DialogueEvent evt)
        {
            // TODO: process selected option

            if (m_CurrentNode.NextNodes.IsNullOrEmpty())
            {
                FinishDialogue();
                return;
            }
            
            LoadDialogueNode(m_CurrentNode.NextNodes[evt.OptionIndex]);
        }

        private void FinishDialogue()
        {
            using var dialogueEvent = DialogueEvent.Get().SendGlobal((int)DialogueEventType.Finish);
        }

        public void SaveGameState()
        {
            
        }

        private void OnDisable()
        {
            GEM.RemoveListener<DialogueEvent>(OnDialogueOptionSelected);
        }
    }
}