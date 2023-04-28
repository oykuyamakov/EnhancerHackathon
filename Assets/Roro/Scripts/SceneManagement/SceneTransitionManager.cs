using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using Roro.Scripts.Serialization;
using Roro.Scripts.Utility;
using SceneManagement.EventImplementations;
using SettingImplementations;
using Sirenix.OdinInspector;
using UnityCommon.Modules;
using UnityCommon.Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    [System.Serializable]
    public enum SceneId
    {
        MainMenu = 0,
        Game = 2,
        Shared = 4,
        BeatTest = 8,
        Loading = 64,
        None = 128,
    }

    public static class SceneExtensions
    {
        public static Scene GetScene(this SceneId id)
        {
            return SceneManager.GetSceneByName(id.ToString());
        }

        public static string GetName(this SceneId id)
        {
            return id.ToString();
        }
    }

    public static class Shared
    {
        public static Camera MainCamera => m_Cam == null ? m_Cam = Camera.main : m_Cam;
        private static Camera m_Cam;
    }

    [DefaultExecutionOrder(ExecOrder.SceneManager)]
    public class SceneTransitionManager : SingletonBehaviour<SceneTransitionManager>
    {
        [ShowInInspector] public SceneId CurrentSceneId => m_CurrentSceneId;
        private SceneId m_CurrentSceneId;
        private SceneId m_NextSceneId;
        private SceneId m_SceneToUnload;

        private SerializationWizard m_SerializationContext;

        private Conditional m_CameraDisableTimer;

        private bool m_NextTempSceneLoaded;
        private bool m_LoadingTimePassed;
        private bool m_NextSceneActivated;

        private GeneralSettings m_Settings;

        private void Awake()
        {
            if (!SetupInstance(false))
                return;

            m_Settings = GeneralSettings.Get();
            m_NextTempSceneLoaded = false;
            m_SerializationContext = SerializationWizard.Default;

            GEM.AddListener<SceneChangeRequestEvent>(OnSceneChangeRequest);
        }


        private void OnSceneChangeRequest(SceneChangeRequestEvent evt)
        {
            m_SerializationContext.Push();

            OnSceneChange(evt.sceneId);
        }

        private void OnSceneChange(SceneId sceneId)
        {
            var oldScene = m_CurrentSceneId;
            if (sceneId == SceneId.Game)
            {
                SceneManager.LoadScene("Game");
                // SceneManager.LoadScene("Beat", LoadSceneMode.Additive);

                Conditional.Wait(1).Do(() =>
                {
                    if (SceneManager.GetSceneByName(oldScene.ToString()).IsValid())
                    {
                        SceneManager.UnloadSceneAsync(oldScene.ToString());
                    }
                });

                m_CurrentSceneId = sceneId;
            }
            else
            {
                if (oldScene == SceneId.Game)
                {
                    SceneManager.UnloadSceneAsync("Game");
                    // SceneManager.UnloadSceneAsync("Beat");
                }

                SceneManager.LoadScene(sceneId.ToString(), LoadSceneMode.Additive);

                m_CurrentSceneId = sceneId;
            }
        }

        public IEnumerator LoadScene(SceneId sceneId, bool waitForLoadingScene)
        {
            var sceneToLoad = sceneId.GetScene();

            if (sceneToLoad.IsValid())
            {
                StartCoroutine(OnSceneLoaded(sceneId, waitForLoadingScene));
                yield break;
            }

            yield return null;

            yield return new WaitForSeconds(m_Settings.SceneTransitionDuration / 3f);

            var sceneName = sceneId.GetName();
            var asyncOp = SceneManager.LoadSceneAsync(
                sceneName, new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.None));

            while (!asyncOp.isDone)
            {
                yield return null;
            }

            yield return null;

            yield return StartCoroutine(OnSceneLoaded(sceneId, waitForLoadingScene));
        }

        private IEnumerator OnSceneLoaded(SceneId sceneId, bool waitForLoadingScene)
        {
            m_NextTempSceneLoaded = true;

            if (waitForLoadingScene)
                yield break;

            SceneManager.SetActiveScene(sceneId.GetScene());

            var evt2 = GetSceneControllerEvent.Get(sceneId).SendGlobal();
            if (evt2.Controller != null)
            {
                evt2.Controller.TogglePermanentScene(true);
            }
        }

        public IEnumerator UnLoadScene(SceneId sceneId)
        {
            if (sceneId != SceneId.None)
            {
                var sceneToLoad = sceneId.GetScene();
                if (!sceneToLoad.IsValid())
                {
                }
                else
                {
                    yield return null;

                    var evt = GetSceneControllerEvent.Get(sceneId).SendGlobal();
                    if (evt.Controller != null)
                    {
                        evt.Controller.TogglePermanentScene(false);
                    }

                    SceneManager.UnloadSceneAsync(sceneToLoad);

                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }

        public void ChangeScene(SceneId sceneId)
        {
            StartCoroutine(UnLoadScene(m_CurrentSceneId));

            LoadScene(sceneId, false);

            using var sceneChangedEvt = OnSceneChangeEvent.Get(sceneId, m_CurrentSceneId).SendGlobal();
            m_CurrentSceneId = sceneId;
        }
    }
}