using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

namespace Videos.Scripts
{
    public class VideoSetter : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer m_VideoPlayer;

        [SerializeField]
        private RenderTexture m_videoRenderTexture;

        [Button]
        public void PlayVideo(VideoData data, VideoPlayer.EventHandler onVideoEndReached = null)
        {
            // m_VideoPlayer.playOnAwake = false;
            // m_VideoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            // m_VideoPlayer.targetCameraAlpha = 0.5F;
            // m_VideoPlayer.isLooping = true;

            m_VideoPlayer.url = data.VideoPath;
            if (onVideoEndReached == null)
            {
                m_VideoPlayer.loopPointReached += VideoEndReached;
            }
            else
            {
                m_VideoPlayer.loopPointReached += onVideoEndReached;
            }

            // Start playback. This means the VideoPlayer may have to prepare (reserve
            // resources, pre-load a few frames, etc.). To better control the delays
            // associated with this preparation one can use videoPlayer.Prepare() along with
            // its prepareCompleted event.
            m_VideoPlayer.Play();
        }

        private void VideoEndReached(VideoPlayer vp)
        {
            m_VideoPlayer.Stop();
            ClearOutRenderTexture(m_videoRenderTexture);
        }

        private void ClearOutRenderTexture(RenderTexture renderTexture)
        {
            RenderTexture rt = RenderTexture.active;
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = rt;
        }
    }
}