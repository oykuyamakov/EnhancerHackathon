using System;
using Events;
using InputManagement;
using LevelManagement.EventImplementations;
using UnityEngine;

namespace LevelManagement
{
    public class LevelManager : MonoBehaviour
    {
        // [SerializeField] 
        // private int m_Speed;
        //
        // public Transform m_Level;
        //
        // private Vector2 m_LevelMoveDir;
        //
        // private Direction m_CurrentDirection;
        //
        // private bool m_IsOnBorder;
        // private bool m_IsPressing;

        private void Awake()
        {
            // GEM.AddListener<EndTriggerEvent>(MovePlatform,channel:(int)TriggerEventState.In);
            // GEM.AddListener<EndTriggerEvent>(StopPlatform,channel:(int)TriggerEventState.Out);
        }

        // private void MovePlatform(EndTriggerEvent evt)
        // {
        //     m_LevelMoveDir = evt.m_Dir == Direction.Left ? Vector2.right : Vector2.left;
        //     m_CurrentDirection = evt.m_Dir;
        // }
        //
        // private void StopPlatform(EndTriggerEvent evt)
        // {
        //     m_LevelMoveDir = Vector2.zero;
        // }
        
        private void Update()
        {
            // if (m_LevelMoveDir != Vector2.zero)
            // {
            //     if (m_CurrentDirection == Direction.Left)
            //     {
            //         if(InputManager.MovementDirection.x >= 0)
            //             m_LevelMoveDir = Vector2.zero;
            //         else
            //         {
            //             m_LevelMoveDir = Vector2.right;
            //         }
            //     
            //     }
            //     if (m_CurrentDirection == Direction.Right)
            //     {
            //         if(InputManager.MovementDirection.x <= 0)
            //             m_LevelMoveDir = Vector2.zero;
            //         else
            //         {
            //             m_LevelMoveDir = Vector2.left;
            //         }
            //     }
            //     
            //     m_Level.Translate(m_LevelMoveDir * Time.deltaTime * m_Speed);
            // }
        }
    }
}
