using System;
using System.Collections.Generic;
using Dialogue.EventImplementations;
using Events;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityCommon.Modules;
using UnityEngine;
using Videos;
using Videos.EventImplementations;

namespace Dialogue
{
    public enum Dialogue
    {
        UnfinishedLevel = 0,
        NeedsLevel = 1,
        HealthUi = 2,
        PlatformIssue = 3,
        Collider = 4,
        Captcha = 5,
    }
    
    public class DialogueManager : MonoBehaviour
    {
        public List<DialogueNode> DialogueNodes = new List<DialogueNode>();
        
        private DialogueNode m_CurrentNode;

        public Dictionary<Dialogue, bool> DialogueStates = new Dictionary<Dialogue, bool>();

        private void Awake()
        {
            DialogueStates.Add(Dialogue.UnfinishedLevel, false);
            DialogueStates.Add(Dialogue.NeedsLevel, false);
            DialogueStates.Add(Dialogue.HealthUi, false);
            DialogueStates.Add(Dialogue.PlatformIssue, false);
            DialogueStates.Add(Dialogue.Collider, false);
            DialogueStates.Add(Dialogue.Captcha, false);
        }

        private void OnEnable()
        {
            GEM.AddListener<DialogueEvent>(OnLoadDialogue, channel:(int)DialogueEventType.Load);
            GEM.AddListener<DialogueEvent>(OnDialogueOptionSelected, Priority.High, (int)DialogueEventType.OptionSelected);
        }

        public void LoadDialogueNode(int index)
        {
            LoadDialogueNode(DialogueNodes[index]);
        }

        private void OnLoadDialogue(DialogueEvent evt)
        {
            if (DialogueStates[evt.Dialogue])
            {
                return;
            }
            
            DialogueStates[evt.Dialogue] = true;
            LoadDialogueNode(DialogueNodes[(int)evt.Dialogue]);
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
            
            Conditional.If(() => !VideoSetter.IsVideoPlaying)
                .Do(OnVideoComplete);
        }

        private void OnVideoComplete()
        {
            if (m_CurrentNode.NextNodes.IsNullOrEmpty())
            {
                FinishDialogue();
                return;
            }
            
            LoadDialogueNode(m_CurrentNode.NextNodes[0]);
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
            Conditional.Wait(0.2f)
                .Do(() =>
                {
                    using var dialogueEvent = DialogueEvent.Get().SendGlobal((int)DialogueEventType.Finish);
                });
        }

        public void SaveGameState()
        {
            
        }

        private void OnDisable()
        {
            GEM.RemoveListener<DialogueEvent>(OnLoadDialogue);
            GEM.RemoveListener<DialogueEvent>(OnDialogueOptionSelected);
        }
    }
}