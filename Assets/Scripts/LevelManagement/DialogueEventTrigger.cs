using System;
using Dialogue.EventImplementations;
using Events;
using UnityEngine;

namespace LevelManagement
{
    public class DialogueEventTrigger : MonoBehaviour
    {
        public Dialogue.Dialogue m_Type;

        public void OnTriggerEnter2D(Collider2D col)
        {
            using var dialogueEvt =
                DialogueEvent.Get(m_Type).SendGlobal((int)DialogueEventType.Load);

            dialogueEvt.SendGlobal();
        }
    }
}
