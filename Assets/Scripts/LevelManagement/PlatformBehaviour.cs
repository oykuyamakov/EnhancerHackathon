using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityCommon.Modules;
using UnityCommon.Runtime.Utility;
using UnityEngine;

namespace LevelManagement
{
    public enum PlatformBehaviourType
    {
        Vertical = 1,
        Horizontal = 2,
        Crash = 4,
        Stairs = 8
    }
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlatformBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float m_MoveLenght = 2f;
        
        [SerializeField]
        private float m_MoveDuration = 2f;
        
        [SerializeField]
        public PlatformBehaviourType m_BehaviourType;
        
        private List<SpriteRenderer> m_SpriteRenderers = new List<SpriteRenderer>();


        private void Awake()
        {
            switch (m_BehaviourType)
            {
                case PlatformBehaviourType.Vertical:
                    VerticalMove();
                    break;
                case PlatformBehaviourType.Horizontal:
                    HorizontalMove();
                    break;
                case PlatformBehaviourType.Crash:
                    Crash();
                    break;
                case PlatformBehaviourType.Stairs:
                    Stair();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void HorizontalMove()
        {
            transform.DOMove(transform.position + Vector3.left * m_MoveLenght, m_MoveDuration).SetLoops(-1,LoopType.Yoyo);
        }
        
        public void VerticalMove()
        {
            transform.DOMove(transform.position + Vector3.up * m_MoveLenght, m_MoveDuration).SetLoops(-1,LoopType.Yoyo);
        }
        
        public void Crash()
        {
            transform.DOShakeRotation(0.5f, 2f, 5, 90f, false).SetLoops(-1,LoopType.Yoyo);
        }

        public void Stair()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(m_BehaviourType != PlatformBehaviourType.Crash)
                return;
            
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
                
                Conditional.Wait(0.1f).Do(() =>
                {
                    m_SpriteRenderers.ForEach(renderer =>
                    { 
                        renderer.DOColor(Color.clear, 0.2f).SetLoops(3).OnComplete(() =>
                        {
                            renderer.enabled = false;
                            GetComponent<Collider2D>().enabled = false;
                        });
                    });
                   
                });
            }
        }
    }
}
