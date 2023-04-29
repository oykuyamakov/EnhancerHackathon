using Events;
using UnityEngine;

namespace InputManagement.EventImplementations
{
    public class SpaceButtonPressedEvent : Event<SpaceButtonPressedEvent>
    {
        public static SpaceButtonPressedEvent Get()
        {
            var evt = GetPooledInternal();
            return evt;
        }
    }
}
