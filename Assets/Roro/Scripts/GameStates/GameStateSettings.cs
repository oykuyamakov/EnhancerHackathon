using Events;
using GameStates.EventImplementations;
using SceneManagement;
using SceneManagement.EventImplementations;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityCommon.Editor.Utility;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStages
{
    public enum GameState
    {
        None,
        Menu,
        Hub,
        Game,
        GameOver,
        Quit
    }

    [CreateAssetMenu(fileName = "GameState")]
    public class GameStateSettings : ScriptableObject
    {
        private static GameStateSettings m_GameStateSettings;

        private static GameStateSettings gameStateSettings
        {
            get
            {
                if (!m_GameStateSettings)
                {
                    m_GameStateSettings = Resources.Load<GameStateSettings>($"GameState");

                    if (!m_GameStateSettings)
                    {
#if UNITY_EDITOR
                        m_GameStateSettings = CreateInstance<GameStateSettings>();
                        var path = "Assets/Resources/GameState.asset";
                        AssetDatabaseHelpers.CreateAssetMkdir(m_GameStateSettings, path);
#else
                        // throw new Exception("Global settings could not be loaded");
#endif
                    }
                }

                return m_GameStateSettings;
            }
        }

        public static void Initialize()
        {
            GEM.AddListener<OnSceneChangeEvent>(OnSceneChange);

            // TODO: temp amelelik
            if (SceneManager.GetActiveScene().name == "BossOne" ||
                SceneManager.GetActiveScene().name == "SideMissionOne")
            {
                gameStateSettings.CurrentGameState = GameState.Game;
            }
        }

        private static void OnSceneChange(OnSceneChangeEvent evt)
        {
            switch (evt.newScene)
            {
                case SceneId.MainMenu:
                    gameStateSettings.CurrentGameState = GameState.Menu;
                    break;
                default:
                    gameStateSettings.CurrentGameState = GameState.Game;
                    break;
            }
        }

        public static GameStateSettings Get()
        {
            return gameStateSettings;
        }

        public GameState CurrentGameState
        {
            get => m_CurrentGameState;
            set
            {
                m_CurrentGameState = value;
                using var evt = GameStateChangedEvent.Get(m_CurrentGameState).SendGlobal();
            }
        }

        [ShowInInspector]
        private GameState m_CurrentGameState = GameState.None;
    }
}