using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Events;
using JetBrains.Annotations;
using SettingImplementations;
using UnityCommon.Modules;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class SceneController : MonoBehaviour
    {
        public bool NeedsCamera = false;

        public bool IsPermanent;

        [SerializeField] 
        private bool stayEnabledOnLoad = false;

        [Header("Permanent Scene Objects")] [SerializeField]
        private List<GameObject> m_DestroyWhenAdditive = new();
        [SerializeField] 
        private List<CanvasGroup> m_CanvasGroups = new();
        [SerializeField] 
        [HideInInspector] 
        private List<Canvas> m_Canvases;
        [SerializeField]
        private List<GameObject> m_GameObjects = new();
        [SerializeField]
        private List<MonoBehaviour> m_Components = new();
        [SerializeReference]
        private List<ISceneActivationObject> m_ActivationObjects = new();
        [SerializeField]
        private List<AudioSource> m_Sources;
        
        private GeneralSettings m_Settings => GeneralSettings.Get();
        
        public string SceneName => gameObject.scene.name;

        public SceneId SceneId;

        private bool m_Active;


#if UNITY_EDITOR
        private void OnValidate()
        {
            m_Canvases = m_CanvasGroups.Select(cg => cg.GetComponent<Canvas>()).ToList();
        }
#endif

        private void Start()
        {
            SceneId = (SceneId) Enum.Parse(typeof(SceneId), SceneName);
            
            // if (!IsPermanent)
            // {
            //     Conditional.Wait(1).Do(() =>
            //     {
            //         using var sceneLoadedEvt = OnSceneLoadedEvent.Get(this);
            //         sceneLoadedEvt.SendGlobal();
            //     });
            //
            //     stayEnabledOnLoad = true;
            // }
            //
            // //if no need to stay enabled for the scene objects and there is more than one scene
            // if (!stayEnabledOnLoad && SceneManager.sceneCount > 1)
            // {
            //     OnDontStayEnabledAdditively();
            // }
        }

        // public void OnDontStayEnabledAdditively()
        // {
        //     for (var i = 0; i < m_DestroyWhenAdditive.Count; i++)
        //     {
        //         Destroy(m_DestroyWhenAdditive[i]);
        //     }
        //
        //     TogglePermanentScene(false, false);
        // }

        public void TogglePermanentScene(bool activate, bool animate = true)
        {
            if(m_Active == activate)
                return;

            m_Active = activate;
            
            // for (int i = 0; i < m_Sources.Count; i++)
            // {
            //     var val = activate ? 1 : 0;
            //     //fix here by playing sound elsewhere later
            //     if(activate && SceneId != SceneId.BossOne)
            //         m_Sources[i].Play();
            //     m_Sources[i].DOFade(val, 0.2f);
            // }
            
            if (!IsPermanent)
            {
                // if (activate)
                // {
                //     var scene = SceneManager.GetSceneByName(SceneName);
                //     SceneManager.SetActiveScene(scene);
                // }
                // else
                // {
                //    
                // }
            }
            else
            {

                var ease = activate ? Ease.InQuad : Ease.OutQuad;
                var alpha = activate ? 1f : 0f;

                var settings = GeneralSettings.Get();
                
                //Debug.Log(SceneName + "is permanent and now visibility will be" + activate);

                //var duration = SceneId == SceneId.Loading & activate ? 0.45f : animate ? settings.SceneTransitionDuration : 0.5f;
                var duration = SceneId == SceneId.Loading ? activate ? 
                    2 : 1: activate ? 0.35f : 0.45f;
                
                for (var i = 0; i < m_CanvasGroups.Count; i++)
                {
                    var cg = m_CanvasGroups[i];
                    var c = m_Canvases[i];

                    if (activate)
                    {
                        c.enabled = true;
                        cg.blocksRaycasts = true;
                    }

                    cg.DOKill();
                    // Conditional.WaitFrames(1).Do(() =>
                    // {
                        cg.DOFade(alpha, duration).SetEase(ease).OnComplete(() =>
                        {
                            if (!activate)
                            {
                                c.enabled = false;
                                cg.blocksRaycasts = false;
                            }
                        });
                    //});
                }

                for (var i = 0; i < m_GameObjects.Count; i++)
                {
                    var go = m_GameObjects[i];
                    go.SetActive(activate);
                }

                for (var i = 0; i < m_Components.Count; i++)
                {
                    m_Components[i].enabled = activate;
                }

                for (var i = 0; i < m_ActivationObjects.Count; i++)
                {
                    if (activate)
                        m_ActivationObjects[i].OnActivated();
                    else
                        m_ActivationObjects[i].OnDeactivated();
                }
            }
        }
    }
}