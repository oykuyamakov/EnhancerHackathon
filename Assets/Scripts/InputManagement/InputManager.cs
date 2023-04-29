using System;
using Events;
using InputManagement.EventImplementations;
using Player;
using SettingImplementations;
using UnityCommon.Runtime.Extensions;
using UnityEngine;

namespace InputManagement
{
    public class InputManager : MonoBehaviour
    {
        private GeneralSettings m_Settings => GeneralSettings.Get();
        
        public static Vector2 MovementDirection = new Vector2();

        private float m_Timer;
        private bool m_SpacePressed;

        private void Update()
        {
            MovementDirection = MovementDirection.WithX(Input.GetAxis("Horizontal"));
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!PlayerController.IsGrounded)
                {
                    return;
                }
                m_SpacePressed = true;
            }  
            if (Input.GetKeyUp(KeyCode.Space))
            {
                m_SpacePressed = false;
                m_Timer = 0;
                MovementDirection = MovementDirection.WithY(0);
            }
            
            if(m_SpacePressed)
                m_Timer += Time.deltaTime;
            
            if(m_Timer > m_Settings.JumpDuration)
                MovementDirection = MovementDirection.WithY(0);
            else if(m_SpacePressed)
            {
                MovementDirection = MovementDirection.WithY(1);
            }

        }
    }
}
