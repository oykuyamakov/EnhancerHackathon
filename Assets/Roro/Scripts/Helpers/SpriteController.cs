using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using Roro.Scripts.GameManagement.EventImplementations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Roro.Scripts.Helpers
{
    [Serializable]
    public class AnimationBundle
    {
        public string Name;
        public Sprite[] Sprites;
    }
    
    public class SpriteController : MonoBehaviour
    {
        [SerializeField]
        private bool m_Loop;

        [SerializeField]
        private float m_Duration = 1f;

        [SerializeField]
        private float m_Gap = 0.01f;

        [SerializeField]
        private List<AnimationBundle> m_AnimationBundles = new List<AnimationBundle>();
        
        private Dictionary<string,Sprite[]> m_AnimationDictionary = new Dictionary<string, Sprite[]>();

        private SpriteRenderer m_SpriteRenderer => GetComponent<SpriteRenderer>();
        
        private int index = 0;

        private float timer = 0;

        private string m_CurrentLoopAnim;
        
        private void Awake()
        {
            m_CurrentLoopAnim = "Idle";
            SetDictionary();
        }


        public void ChangeAnimation(string newAnim)
        {
            if(m_AnimationDictionary.ContainsKey(newAnim))
                m_CurrentLoopAnim = newAnim;
            else
            {
                Debug.LogError("This animation does not exists" + newAnim);
            }
        }
        private void SetDictionary()
        {
            m_AnimationDictionary = new Dictionary<string, Sprite[]>();
            foreach (var bundle in m_AnimationBundles)
            {
                m_AnimationDictionary.Add(bundle.Name, bundle.Sprites);
            }
        }

        public void ChangeColor(Color color, bool animate = false)
        {
            m_SpriteRenderer.DOColor(color, 0.1f).SetLoops(4,LoopType.Yoyo);
        }
        
        public void PlayAnimation(string name)
        {
            if (m_AnimationDictionary.ContainsKey(name))
            {
                m_Loop = false;
                StartCoroutine(AnimateSplash(m_AnimationDictionary[name],() => { }));
            }
        }

        private void Update()
        {
            if (m_Loop)
            {
                LoopAnim();
            }
        }


        public IEnumerator AnimateSplash(Sprite[] animations,Action oncomplete)
        {
            m_SpriteRenderer.enabled = true;
            for (int i = 0; i < animations.Length; i++)
            {
                m_SpriteRenderer.sprite = animations[i];
                yield return new WaitForSeconds(m_Gap);
            }
            
            m_SpriteRenderer.enabled = false;
            oncomplete.Invoke();
        }

       

        public void LoopAnim()
        {
            if((timer+=Time.deltaTime) >= (m_Duration / m_AnimationDictionary[m_CurrentLoopAnim].Length))
            {
                timer = 0;
                m_SpriteRenderer.sprite = m_AnimationDictionary[m_CurrentLoopAnim][index];
                index = (index + 1) % m_AnimationDictionary[m_CurrentLoopAnim].Length;
            }
        }
    }
}
