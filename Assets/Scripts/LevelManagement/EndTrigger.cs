using System;
using Events;
using LevelManagement.EventImplementations;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LevelManagement
{
    public class EndTrigger : MonoBehaviour
    {
        // [SerializeField]
        // private Direction m_Dir;
        //
        //
        // private void OnTriggerExit2D(Collider2D other)
        // {
        //     if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        //     {
        //         using var evt = EndTriggerEvent.Get(m_Dir);
        //         evt.SendGlobal((int)TriggerEventState.Out);
        //     }
        // }
        //
        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        //     {
        //         using var evt = EndTriggerEvent.Get(m_Dir);
        //         evt.SendGlobal((int)TriggerEventState.In);
        //     }
        // }
    }

    public enum Direction
    {
        Left,
        Right
    }
}
