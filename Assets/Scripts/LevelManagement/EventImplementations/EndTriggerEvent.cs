using Events;
using UnityEngine;

namespace LevelManagement.EventImplementations
{
    public class EndTriggerEvent :Event<EndTriggerEvent>
    {
        private int m_Dir;

        public static EndTriggerEvent Get(int dir)
        {
            var evt = GetPooledInternal();
            evt.m_Dir = dir;
            return evt;
        }
    }
}
