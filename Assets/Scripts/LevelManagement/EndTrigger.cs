using System;
using Events;
using LevelManagement.EventImplementations;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelManagement
{
    public class EndTrigger : MonoBehaviour
    {
        [SerializeField]
        private int m_Dir;
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                using var evt = EndTriggerEvent.Get(m_Dir);
                evt.SendGlobal();
            }
        }
    }
}
