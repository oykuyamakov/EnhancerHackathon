using System;
using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Extensions;

public class IntroSequence : MonoBehaviour
{
    public CanvasGroup CanvasGroup;

    public Image DownloadGamePanel;
    public Button TorrentItButton;
    
    public TextMeshProUGUI YouThiefText1;
    public TextMeshProUGUI YouThiefText2;

    public Image RedScreen;
    
    public float DownloadPanelFadeDuration;
    public float ThiefText1Duration;
    public float ThiefText2Duration;
    public float RedScreenTransitionDuration;

    public AudioSource BackgroundMusic;

    private void OnEnable()
    {
        TorrentItButton.onClick.AddListener(OnTorrentIt);       
    }

    private void OnTorrentIt()
    {
        using var soundEvt = SoundPlayEvent.Get(SoundType.Button).SendGlobal();

        Sequence introSequence = DOTween.Sequence();
        
        introSequence.Append(DownloadGamePanel.DOFade(0f, DownloadPanelFadeDuration));
        
        string introText1 = "so you decided to steal our game";
        string introText2 = "YOU THIEF";
        introSequence.Append(YouThiefText1.DOText(introText1, ThiefText1Duration));
        introSequence.Append(YouThiefText2.DOText(introText2, ThiefText2Duration));
        introSequence.Join(RedScreen.DOFade(1f, RedScreenTransitionDuration / 2).SetDelay(ThiefText2Duration / 2));

        introSequence.OnComplete(() =>
        {
            YouThiefText1.DOFade(0, 0f);
            YouThiefText2.DOFade(0, 0f);
            CanvasGroup.Toggle(false, 0.25f);

            BackgroundMusic.Play();
        });
    }
}
