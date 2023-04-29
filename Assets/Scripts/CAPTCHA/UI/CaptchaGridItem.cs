using CAPTCHA.EventImplementations;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace CAPTCHA.UI
{
    public class CaptchaGridItem : MonoBehaviour
    {
        public Image Image;
        public Button SelectionButton;

        private bool m_IsCorrectTile;

        public void SetUI(bool correctTile)
        {
            m_IsCorrectTile = correctTile;
            Image.color = Color.clear;
            Image.DOFade(0.1f, 0f);
        }

        private void OnEnable()
        {
            SelectionButton.onClick.AddListener(OnSelected);
        }

        private void OnSelected()
        {
            Image.color = m_IsCorrectTile ? Color.green : Color.red;
            Image.DOFade(0.1f, 0f);
            using var captchaEvt = CaptchaEvent.Get().SendGlobal(m_IsCorrectTile
                ? (int)CaptchaEventType.CorrectTileSelected
                : (int)CaptchaEventType.WrongTileSelected);
        }

        private void OnDisable()
        {
            SelectionButton.onClick.RemoveListener(OnSelected);
        }
    }
}