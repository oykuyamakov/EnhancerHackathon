using System;
using DG.Tweening;
using UnityEngine;

namespace LevelManagement
{
    public class Coin : MonoBehaviour
    {
        private SpriteRenderer m_Renderer => GetComponent<SpriteRenderer>();

        private void Awake()
        {
            VerticalMove();
        }
        
        public void VerticalMove()
        {
            transform.DOMove(transform.position + Vector3.up * 1.5f, 0.5f).SetLoops(-1,LoopType.Yoyo);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                transform.DOKill();
                GetComponent<Collider2D>().enabled = false;

                transform.DOScale(Vector3.zero, 0.8f);
                m_Renderer.DOFade(0, 0.5f);
            }
        }
    }
}
