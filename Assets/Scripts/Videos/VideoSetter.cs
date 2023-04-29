using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;
using Videos.EventImplementations;

namespace Videos
{
    public class VideoSetter : MonoBehaviour
    {
        public static bool IsVideoPlaying;

        public VideoUI VideoUi;
        
        [SerializeField]
        private VideoPlayer m_VideoPlayer;

        [SerializeField]
        private RenderTexture m_videoRenderTexture;

        private VideoData m_CurrentVideo;

        private void OnEnable()
        {
            GEM.AddListener<VideoEvent>(OnPlayVideo, Priority.VeryHigh, (int)VideoEventType.Play);
        }

        private void OnPlayVideo(VideoEvent evt)
        {
            PlayVideo(evt.VideoData, evt.OnVideoEnd);
        }

        [Button]
        public void PlayVideo(VideoData data, VideoPlayer.EventHandler onVideoEndReached = null)
        {
            VideoUi.CrackTheScreen();
            
            IsVideoPlaying = true;
            
            // m_VideoPlayer.playOnAwake = false;
            // m_VideoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            // m_VideoPlayer.targetCameraAlpha = 0.5F;
            // m_VideoPlayer.isLooping = true;

            m_VideoPlayer.url = data.VideoPath;
            if (onVideoEndReached != null)
            {
                m_VideoPlayer.loopPointReached += onVideoEndReached;
            }
            m_VideoPlayer.loopPointReached += VideoEndReached;

            // Start playback. This means the VideoPlayer may have to prepare (reserve
            // resources, pre-load a few frames, etc.). To better control the delays
            // associated with this preparation one can use videoPlayer.Prepare() along with
            // its prepareCompleted event.
            m_VideoPlayer.Play();
        }

        private void VideoEndReached(VideoPlayer vp)
        {
            IsVideoPlaying = false;

            m_VideoPlayer.Stop();
            ClearOutRenderTexture(m_videoRenderTexture);
            
            VideoUi.Disappear();
        }

        private void ClearOutRenderTexture(RenderTexture renderTexture)
        {
            RenderTexture rt = RenderTexture.active;
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = rt;
        }

        private void OnDisable()
        {
            GEM.RemoveListener<VideoEvent>(OnPlayVideo);
        }
    }
}