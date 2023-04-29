using System.Collections.Generic;
using CAPTCHA.EventImplementations;
using CAPTCHA.UI;
using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CAPTCHA
{
    public class CaptchaManager : MonoBehaviour
    {
        public CaptchaUI CaptchaUI;

        public List<CaptchaData> CaptchaData = new List<CaptchaData>();

        public static bool WrongChosen = false;

        private void OnEnable()
        {
            GEM.AddListener<CaptchaEvent>(OnActivateCaptchaEvent, channel:(int)CaptchaEventType.Activate);
            GEM.AddListener<CaptchaEvent>(OnActivateCaptchaEvent, channel:(int)CaptchaEventType.Restart);
            GEM.AddListener<CaptchaEvent>(OnWrongChosen, channel:(int)CaptchaEventType.WrongTileSelected);
        }

        private void OnActivateCaptchaEvent(CaptchaEvent evt)
        {
            InitializeCaptcha();
        }

        [Button]
        private void InitializeCaptcha()
        {
            WrongChosen = false;
            
            var randIndex = Random.Range(0, CaptchaData.Count);
            CaptchaUI.SetUI(CaptchaData[randIndex]);
        }

        private void OnWrongChosen(CaptchaEvent evt)
        {
            WrongChosen = true;
        }
        
        private void OnDisable()
        {
            GEM.RemoveListener<CaptchaEvent>(OnActivateCaptchaEvent);
        }
    }
}