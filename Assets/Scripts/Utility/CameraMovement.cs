using System;
using UnityCommon.Runtime.Extensions;
using UnityEngine;

namespace Utility
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] 
        private Transform m_Target;

        private float m_YOffset;

        private void Awake()
        {
            m_YOffset = transform.position.y;
        }

        void Update()
        {
            transform.position = transform.position.WithX(m_Target.position.x).WithY(m_YOffset);
        }
    }
}
