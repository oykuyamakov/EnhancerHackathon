using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LevelManagement
{
    public class PlatformCreator : MonoBehaviour
    {
        [SerializeField]
        private GameObject PlatformPrefab;

        public float m_Offset;

        [SerializeField][HideInInspector]
        private int m_Counter;
        

        [Button]
        public void Reses()
        {
            m_Counter = 0;
        }
        
        // public void Platforming()
        // {
        //     var needed = m_PlatformCount - m_Counter;
        //     for (int i = 0; i < needed; i++)
        //     {
        //         CreatePlatform();
        //     }
        //
        //     for (int i = needed; i > 0; i++)
        //     {
        //         var b = m_Platforms.Last();
        //         m_Platforms.Remove(b);
        //         Destroy(b);
        //     }
        // }
        
        [Button]
        public void CreatePlatform()
        {
            var platform = Instantiate(PlatformPrefab);
            platform.transform.position = transform.position + Vector3.right * (m_Offset * m_Counter);
            platform.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - (1 * m_Counter);
            m_Counter++;
        }
        
    }
}
