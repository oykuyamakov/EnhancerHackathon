using Events;
using GameStages;

namespace GameStates.EventImplementations
{
    public class GameStateChangedEvent : Event<GameStateChangedEvent>
    {
        public GameState GameState;

        public static GameStateChangedEvent Get(GameState state)
        {
            var evt = GetPooledInternal();
            evt.GameState = state;

            return evt;
        }
    }
}