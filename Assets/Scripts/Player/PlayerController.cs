using System;
using Events;
using InputManagement;
using InputManagement.EventImplementations;
using Roro.Scripts.Helpers;
using SettingImplementations;
using UnityCommon.Modules;
using UnityCommon.Runtime.Extensions;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] 
        private Transform m_RcPoint;
        
        private SpriteController m_SpriteController;
        
        private Rigidbody2D m_Rb;
        private GeneralSettings m_Settings => GeneralSettings.Get();
        private float m_PlayerGravity => m_Settings.PlayerGravity;
        private float m_PlayerSpeed => m_Settings.PlayerSpeed;
        private float m_PlayerJumpForce => m_Settings.PlayerJumpForce;

        public static bool IsGrounded;
        
        public static bool OnDeathZone;

        private Vector3 m_InitialScale;

        [SerializeField]
        private float m_Distancce;

        private bool m_Dead;

        private void Awake()
        {
            m_SpriteController = GetComponent<SpriteController>();
            m_InitialScale = transform.localScale;
            m_Rb = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate()
        {
            OnDeathZone = Physics2D.Raycast(m_RcPoint.position, Vector2.down, m_Distancce, LayerMask.GetMask("DeathZone"));

            if (OnDeathZone && !m_Dead)
            {
                Debug.Log("die");
                m_Dead = true;
                Die();
            }
            
            if(m_Dead)
                return;
            
            IsGrounded = Physics2D.Raycast(m_RcPoint.position, Vector2.down, m_Distancce, LayerMask.GetMask("Ground"));

            if (IsGrounded)
            {
                m_LastGroundedPosition = transform.position - transform.forward;
            }
            
            if(InputManager.MovementDirection.x > 0)
                transform.localScale = new Vector3(m_InitialScale.x, m_InitialScale.y, 1);
            else if(InputManager.MovementDirection.x < 0)
                transform.localScale = new Vector3(-m_InitialScale.x, m_InitialScale.y, 1);

            Move();
        }

        private void Move()
        {
            m_Rb.gravityScale = m_PlayerGravity;
            m_Rb.velocity = InputManager.MovementDirection * new Vector2(m_PlayerSpeed, m_PlayerJumpForce);

            if (InputManager.MovementDirection.x != 0)
            {
                m_SpriteController.ChangeAnimation("Run");
            }
            else
            {
                m_SpriteController.ChangeAnimation("Idle");
            }

            if (InputManager.MovementDirection.y != 0)
            {
                m_SpriteController.ChangeAnimation("Jump");
            }
            
            var stairs = Physics2D.Raycast(m_RcPoint.position, m_RcPoint.forward, 
                25f, LayerMask.GetMask("Stairs"));

            if (stairs)
            {
                m_Rb.velocity += Vector2.up * 100f;
            }
            
        }
        
        private Vector3 m_LastGroundedPosition;
        
        private void Die()
        {
            m_SpriteController.ChangeAnimation("Idle");
            m_Rb.constraints = RigidbodyConstraints2D.FreezeAll;
            m_SpriteController.ChangeColor(Color.red,true);

            Conditional.Wait(1f).Do(() =>
            {
                m_Rb.velocity = Vector2.zero;
                m_Rb.position = m_LastGroundedPosition.WithZ(0) + Vector3.up;
                m_Rb.velocity = Vector2.zero;
                m_Rb.constraints = RigidbodyConstraints2D.None;
                m_Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                m_SpriteController.ChangeColor(Color.white, true);

                m_Dead = false;
            });
        }

        private void LooseHealth()
        {
            
        }
    }
}
