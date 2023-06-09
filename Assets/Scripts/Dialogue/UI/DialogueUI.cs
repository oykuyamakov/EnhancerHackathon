using System;
using System.Collections.Generic;
using DG.Tweening;
using Dialogue.EventImplementations;
using Events;
using TMPro;
using UnityEngine;
using Utility.Extensions;

namespace Dialogue.UI
{
    public class DialogueUI : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        
        public TextMeshProUGUI DialogueText;
        
        public Transform DialogueOptionsPanel;
        
        public DialogueOptionUI DialogueOptionPrefab;
        public List<DialogueOptionUI> DialogueOptionUis = new List<DialogueOptionUI>();

        private void Awake()
        {
            CreateDialogueOptionUis(5);
        }

        private void OnEnable()
        {
            GEM.AddListener<DialogueEvent>(OnStartDialogue, Priority.High, (int)DialogueEventType.Start);
            GEM.AddListener<DialogueEvent>(OnFinishDialogue, Priority.High, (int)DialogueEventType.Finish);
        }

        private void OnStartDialogue(DialogueEvent evt)
        {
            CanvasGroup.Toggle(true, 0.2f);
            SetUI(evt.DialogueNode);
        }
        
        private void OnFinishDialogue(DialogueEvent evt)
        {
            CanvasGroup.Toggle(false, 0.2f);
        }
        
        private void SetUI(DialogueNode node)
        {
            if (node.DialogueLine == " ")
            {
                return;
            }
            
            DialogueText.DOText($"{node.Speaker.ToString()} : {node.DialogueLine}", 1f);
            SetDialogueOptionUis(node.DialogueOptions);
        }

        private void CreateDialogueOptionUis(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var optionUi = Instantiate(DialogueOptionPrefab, DialogueOptionsPanel);
                DialogueOptionUis.Add(optionUi);
                
                optionUi.gameObject.SetActive(false);
            }
        }

        private void SetDialogueOptionUis(List<string> optionTexts)
        {
            for (var i = 0; i < DialogueOptionUis.Count; i++)
            {
                if (i >= optionTexts.Count)
                {
                    DialogueOptionUis[i].gameObject.SetActive(false);
                    continue;
                }
                
                DialogueOptionUis[i].gameObject.SetActive(true);
                DialogueOptionUis[i].SetUI(i, optionTexts[i]);
            }
        }
    }
}