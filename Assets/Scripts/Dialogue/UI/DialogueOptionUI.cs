using Dialogue.EventImplementations;
using Events;
using TMPro;
using UnityCommon.Modules;
using UnityEngine;
using UnityEngine.UI;
using Videos;

namespace Dialogue.UI
{
    public class DialogueOptionUI : MonoBehaviour
    {
        public int OptionIndex = -1;
        
        public TextMeshProUGUI OptionText;
        public Button SelectionButton;

        public void SetUI(int optionIndex, string optionText)
        {
            OptionIndex = optionIndex;
            OptionText.text = optionText;

            if (VideoSetter.IsVideoPlaying)
            {
                SelectionButton.interactable = false;
                Conditional.If(() => !VideoSetter.IsVideoPlaying)
                    .Do(() =>
                    {
                        SelectionButton.interactable = true;
                    });
            }
        }
        
        private void OnEnable()
        {
            SelectionButton.onClick.AddListener(OnSelected);
        }
        
        private void OnSelected()
        {
            using var selectionEvt = DialogueEvent.Get(OptionIndex).SendGlobal((int)DialogueEventType.OptionSelected);
        }

        private void OnDisable()
        {
            SelectionButton.onClick.RemoveListener(OnSelected);
        }
    }
}