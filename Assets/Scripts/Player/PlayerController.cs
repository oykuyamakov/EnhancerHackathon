using System;
using Events;
using InputManagement;
using InputManagement.EventImplementations;
using SettingImplementations;
using UnityCommon.Modules;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D m_Rb;

      
        private GeneralSettings m_Settings => GeneralSettings.Get();

        private float m_PlayerGravity => m_Settings.PlayerGravity;

        private float m_PlayerSpeed => m_Settings.PlayerSpeed;

        private float m_PlayerJumpForce => m_Settings.PlayerJumpForce;

        private void Awake()
        {
            m_Rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            m_Rb.gravityScale = m_PlayerGravity;
            m_Rb.velocity = InputManager.MovementDirection * new Vector2(m_PlayerSpeed, m_PlayerJumpForce);
        }
        
        private void Die()
        {
            
        }

        private void LooseHealth()
        {
            
        }
    }
}
