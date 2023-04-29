using Events;
using UnityEngine;

namespace LevelManagement.EventImplementations
{
    // public enum TriggerEventState
    // {
    //     In = 0,
    //     Out = 1
    // }
    
    public class EndTriggerEvent :Event<EndTriggerEvent>
    {
        // public Direction m_Dir;
        //
        // public static EndTriggerEvent Get(Direction dir)
        // {
        //     var evt = GetPooledInternal();
        //     evt.m_Dir = dir;
        //     return evt;
        // }
    }
}
