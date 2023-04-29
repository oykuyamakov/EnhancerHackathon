using Events;
using UnityEngine.Video;

namespace Videos.EventImplementations
{
    public enum VideoEventType
    {
        Play = 0,
        Stop = 1,
    }
    
    public class VideoEvent : Event<VideoEvent>
    {
        public VideoData VideoData;
        public VideoPlayer.EventHandler OnVideoEnd;
        
        public static VideoEvent Get(VideoData data, VideoPlayer.EventHandler onVideoEnd = null)
        {
            var evt = GetPooledInternal();
            evt.VideoData = data;
            evt.OnVideoEnd = onVideoEnd;
            
            return evt;
        }
    }
}