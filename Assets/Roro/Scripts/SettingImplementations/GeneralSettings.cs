using System;
using System.Collections.Generic;
using SceneManagement;
using Sounds;
using UnityCommon.Variables;
using UnityEngine;

namespace SettingImplementations
{
    [CreateAssetMenu(fileName =" GeneralSettings" )]
    public class GeneralSettings : ScriptableObject
    {
        private static GeneralSettings _GeneralSettings;

        private static GeneralSettings generalSettings
        {
            get
            {
                if (!_GeneralSettings)
                {
                    _GeneralSettings = Resources.Load<GeneralSettings>($"Settings/GeneralSettings");

                    if (!_GeneralSettings)
                    {
#if UNITY_EDITOR
                        Debug.Log("General Settings");
                        // _GeneralSettings = CreateInstance<GeneralSettings>();
                        // var path = "Assets/Resources/Settings/GeneralSettings.asset";
                        // AssetDatabaseHelpers.CreateAssetMkdir(_GeneralSettings, path);
#else
 				//		throw new Exception("Global settings could not be loaded");
#endif
                    }
                }

                return _GeneralSettings;
            }
        }
        
        public static GeneralSettings Get()
        {
            return generalSettings;
        }


        public float PlayerGravity = 20;
        public float PlayerSpeed = 10;
        public float PlayerJumpForce = 100;
        public float JumpDuration = 0.5f;

    }

    [Serializable]
    public struct SceneToScene
    {
        public SceneId From;
        public SceneId To;
    }
}
