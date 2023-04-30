using System;
using CAPTCHA.EventImplementations;
using Dialogue.EventImplementations;
using Events;
using InputManagement;
using InputManagement.EventImplementations;
using LevelManagement;
using Roro.Scripts.Helpers;
using SettingImplementations;
using UnityCommon.Modules;
using UnityCommon.Runtime.Extensions;
using UnityCommon.Runtime.Utility;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.Animations;

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

        private ParentConstraint m_ParentConstraint;

        public PlayerHealthUI PlayerHealthUI;
        private IntVariable m_Health;
        private bool m_FirstDamageTaken;
        
        private TimedAction m_WalkingSound;

        private void Awake()
        {
            m_SpriteController = GetComponent<SpriteController>();
            m_ParentConstraint = GetComponent<ParentConstraint>();
            m_InitialScale = transform.localScale;
            m_Rb = GetComponent<Rigidbody2D>();

            m_Health = Variable.Get<IntVariable>("PlayerHealth");

            m_WalkingSound = new TimedAction(PlayWalkingSoundEffect, 0f, 1f);
        }

        private void FixedUpdate()
        {
            OnDeathZone = Physics2D.Raycast(m_RcPoint.position, Vector2.down, m_Distancce,
                LayerMask.GetMask("DeathZone"));

            if (OnDeathZone && !m_Dead)
            {
                Die();
            }

            if (m_Dead)
                return;

            IsGrounded = Physics2D.Raycast(m_RcPoint.position, Vector2.down, m_Distancce, LayerMask.GetMask("Ground"));

            if (IsGrounded)
            {
                m_LastGroundedPosition = transform.position - transform.forward;
            }

            if (InputManager.MovementDirection.x > 0)
                transform.localScale = new Vector3(m_InitialScale.x, m_InitialScale.y, 1);
            else if (InputManager.MovementDirection.x < 0)
                transform.localScale = new Vector3(-m_InitialScale.x, m_InitialScale.y, 1);

            if (m_OnHorizontalPlace && InputManager.MovementDirection.magnitude < 0.1)
            {
                m_ParentConstraint.constraintActive = true;
            }
            else
            {
                m_ParentConstraint.constraintActive = false;
            }

            if (IsGrounded)
            {
                RaycastHit2D hit = Physics2D.Raycast(m_RcPoint.position, Vector2.down, m_Distancce,
                    LayerMask.GetMask("Ground"));

                if (hit.transform.TryGetComponent<PlatformBehaviour>(out var platformBehaviour))
                {
                    if (platformBehaviour.m_BehaviourType == PlatformBehaviourType.Horizontal)
                    {
                        m_OnHorizontalPlace = true;
                        if (m_ParentConstraint.sourceCount == 0)
                        {
                            m_ParentConstraint.AddSource(new ConstraintSource
                            {
                                sourceTransform = platformBehaviour.transform,
                                weight = 1
                            });

                            m_ParentConstraint.locked = true;
                        }
                    }
                    else if (m_OnHorizontalPlace)
                    {
                        m_OnHorizontalPlace = false;
                        m_ParentConstraint.RemoveSource(0);
                    }
                }
                else if (m_OnHorizontalPlace)
                {
                    m_OnHorizontalPlace = false;
                    m_ParentConstraint.RemoveSource(0);
                }
            }
            else if (m_OnHorizontalPlace)
            {
                m_OnHorizontalPlace = false;
                m_ParentConstraint.RemoveSource(0);
            }

            Move();
        }

        private bool m_OnHorizontalPlace;
        private bool m_Jumped;
        
        private void Move()
        {
            m_Rb.gravityScale = m_PlayerGravity;
            m_Rb.velocity = InputManager.MovementDirection * new Vector2(m_PlayerSpeed, m_PlayerJumpForce);

            if (InputManager.MovementDirection.x != 0)
            {
                m_WalkingSound.Update(Time.deltaTime);
                m_SpriteController.ChangeAnimation("Run");
            }
            else
            {
                m_SpriteController.ChangeAnimation("Idle");
            }

            if (InputManager.MovementDirection.y != 0)
            {
                if (!m_Jumped)
                {
                    m_Jumped = true;
                    PlayJumpingSoundEffect();
                }
                
                m_SpriteController.ChangeAnimation("Jump");
            }

            var stairs = Physics2D.Raycast(m_RcPoint.position, m_RcPoint.forward,
                25f, LayerMask.GetMask("Stairs"));

            if (stairs)
            {
                m_Rb.velocity = Vector2.zero;
                m_Rb.position = m_LastGroundedPosition.WithZ(0) + Vector3.up;
            }
        }

        private Vector3 m_LastGroundedPosition;

        private void Die()
        {
            if (m_Dead)
                return;

            m_Dead = true;
            
            using var soundEvt = SoundPlayEvent.Get(SoundType.Dying).SendGlobal();

            m_SpriteController.ChangeAnimation("Idle");
            m_Rb.constraints = RigidbodyConstraints2D.FreezeAll;
            m_SpriteController.ChangeColor(Color.red, true);

            if (!m_FirstDamageTaken)
            {
                m_FirstDamageTaken = true;
                TriggerHealthUiEvent();
            }
            else
            {
                LooseHealth();
            }
        }

        private void LooseHealth(bool wait = true)
        {
            m_Health.Add(-1);

            if (m_Health.Value <= 0)
            {
                using var captchaEvt = CaptchaEvent.Get().SendGlobal((int)CaptchaEventType.Activate);
                GEM.AddListener<CaptchaEvent>(OnCaptchaComplete, channel: (int)CaptchaEventType.Finish);
            }
            else
            {
                if (wait)
                {
                    Conditional.Wait(1f).Do(Reborn);
                }
                else
                {
                    Reborn();
                }
            }
        }

        private void Reborn()
        {
            m_Rb.velocity = Vector2.zero;
            m_Rb.position = m_LastGroundedPosition.WithZ(0) + Vector3.up;
            m_Rb.velocity = Vector2.zero;
            m_Rb.constraints = RigidbodyConstraints2D.None;
            m_Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_SpriteController.ChangeColor(Color.white, true);

            Conditional.WaitFrames(10).Do(() => { m_Dead = false; });
        }

        private void OnCaptchaComplete(CaptchaEvent evt)
        {
            GEM.RemoveListener<CaptchaEvent>(OnCaptchaComplete, channel: (int)CaptchaEventType.Finish);

            m_Health.Value = 5;
            Reborn();
        }

        private void TriggerHealthUiEvent()
        {
            using var dialogueEvt =
                DialogueEvent.Get(Dialogue.Dialogue.HealthUi).SendGlobal((int)DialogueEventType.Load);

            GEM.AddListener<DialogueEvent>(HealthUiDialogueComplete, channel: (int)DialogueEventType.Finish);
        }

        private void HealthUiDialogueComplete(DialogueEvent evt)
        {
            PlayerHealthUI.EnableUI();
            LooseHealth();

            GEM.RemoveListener<DialogueEvent>(HealthUiDialogueComplete, channel: (int)DialogueEventType.Finish);
        }

        private void PlayWalkingSoundEffect()
        {
            using var soundEvt = SoundPlayEvent.Get(SoundType.Walking).SendGlobal();
        }

        private void PlayJumpingSoundEffect()
        {
            using var soundEvt = SoundPlayEvent.Get(SoundType.Jumping).SendGlobal();

            Conditional.Wait(1)
                .Do(() =>
                {
                    m_Jumped = false;
                });
        }
    }
}