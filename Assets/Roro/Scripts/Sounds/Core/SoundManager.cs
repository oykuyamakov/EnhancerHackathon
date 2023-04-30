using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Events;
using Roro.Scripts.Utility;
using SceneManagement;
using Sounds;
using Sounds.Helpers;
using UnityCommon.Singletons;
using UnityCommon.Variables;
using UnityEngine;
using Utility;

namespace Roro.Scripts.Sounds.Core
{
	[DefaultExecutionOrder(ExecOrder.SoundManager)]
	[RequireComponent(typeof(AudioSource))]
	public class SoundManager : SingletonBehaviour<SoundManager>
	{
		
		[SerializeField]
		private Transform m_LoopSoundParent;
		
		private List<AudioSource> m_LoopSources => m_LoopSoundParent.GetComponents<AudioSource>().ToList();

		private List<AudioSource> m_AudioSources => GetComponents<AudioSource>().ToList();

		private int m_SourceIndex = 0;
		private int m_LoopSourceIndex = 0;

		private int m_AvailableSourceCount;

		public List<Sound> Sounds = new List<Sound>();

		//private BoolVariable m_SoundsDisabled;

		private void Awake()
		{
			if (!SetupInstance())
				return;

			//m_SoundsDisabled = Var.Get<BoolVariable>("SFXDisabled");

			m_SourceIndex = 0;
			m_LoopSourceIndex = 0;
			
			Reset();

			GEM.AddListener<SoundPlayEvent>(OnSoundPlayEvent);
			GEM.AddListener<OnSceneLoadedEvent>(OnSceneLoadEvent);
			GEM.AddListener<SoundStopEvent>(OnSoundStopEvent);
		}

		private void OnSoundPlayEvent(SoundPlayEvent evt)
		{
			if (evt.Sound == null)
			{
				PlaySoundFromType(evt);
				return;
			}
			
			if (evt.Loop)
			{
				PlayLoop(evt.Sound);
				evt.LoopIndex = m_LoopSourceIndex - 1;
			}
			else
			{				
				PlaySound(evt.Sound);
			}
		}

		private void PlaySoundFromType(SoundPlayEvent evt)
		{
			if (evt.Loop)
			{
				PlayLoop(Sounds[(int)evt.SoundType]);
				evt.LoopIndex = m_LoopSourceIndex - 1;
			}
			else
			{				
				PlaySound(Sounds[(int)evt.SoundType]);
			}
		}

		private void OnSoundStopEvent(SoundStopEvent evt)
		{
			StopSound(evt.LoopIndex);
		}
		public void PlaySound(Sound sound)
		{
			PlayOneShot(sound);
		}

		public void StopSound(int index)
		{
			m_LoopSources[index].Stop();
		}

		public void OnSceneLoadEvent(OnSceneLoadedEvent evt)
		{
			if (!evt.SceneController.IsPermanent)
			{
				Reset();
			}
		}
		
		private AudioSource GetSource()
		{
			var src = m_AudioSources[m_SourceIndex++];
			m_SourceIndex %= m_AudioSources.Count;
			return src;
		}

		private AudioSource GetLoopSource()
		{
			Debug.Log(m_LoopSourceIndex);
			var src = m_LoopSources[m_LoopSourceIndex++];
			m_LoopSourceIndex %= m_AudioSources.Count;
			return src;
		}
		
		public void Reset()
		{
			Debug.Log("Reset");
			//
			// m_AudioSources.ForEach(source =>
			// {
			// 	if (source.isPlaying)
			// 	{
			// 		source.DOFade(0, 0.1f).OnComplete(() =>source.loop = false);
			// 	}
			//
			// 	m_AvailableSourceCount = m_AudioSources.Count;
			// });
		}

		public void PlayOneShot(Sound sound, float volume = 1f, float pitch = 1f)
		{
			// if (m_SoundsDisabled.Value)
			// 	return;
			
			if (!sound || !sound.Clip || sound.Volume < 1e-2f)
			{
				Debug.Log($"Ignoring sound {sound.name}");
				return;
			}

			var src = GetSource();
			src.PlayOneShot(sound, volume, pitch);
			
		}
		public void PlayLoop(Sound sound, float volume = 1f, float pitch = 1f)
		{
			// if (m_SoundsDisabled.Value)
			// 	return;
			
			if (!sound || !sound.Clip || sound.Volume < 1e-2f)
			{
				Debug.Log($"Ignoring sound {sound.name}");
				return;
			}

			var src = GetLoopSource();
			src.PlayOneShot(sound, volume, pitch);
		}
		
		
#if UNITY_EDITOR

		public void GetSounds()
		{
			
		}
#endif

		[Serializable]
		public class SoundPair
		{
			public int id;
			public Sound sound;
		}

	}
}
