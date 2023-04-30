using System;
using System.Collections.Generic;
using Events;
using UnityCommon.Runtime.Variables;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.UI;
using Utility.Extensions;

namespace Player
{
    public class PlayerHealthUI : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;

        public Image HealthImage; 
        
        public List<Sprite> HealthSprites = new List<Sprite>();

        private IntVariable m_Health;
        
        private void Awake()
        {
            DisableUI();
            
            m_Health = Variable.Get<IntVariable>("PlayerHealth");
            m_Health.AddListener<ValueChangedEvent<int>>(OnHealthModified);
        }

        public void EnableUI()
        {
            CanvasGroup.Toggle(true, 0.25f);
        }

        public void DisableUI()
        {
            CanvasGroup.Toggle(false, 0.25f);
        }

        private void OnHealthModified(ValueChangedEvent<int> evt)
        {
            HealthImage.sprite = HealthSprites[Mathf.Clamp(m_Health, 0, HealthSprites.Count - 1)];
        }
    }
}