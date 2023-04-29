using Events;

namespace CAPTCHA.EventImplementations
{
    public enum CaptchaEventType
    {
        CorrectTileSelected = 0,
        WrongTileSelected = 1,
        Activate = 2,
        Restart = 3,
        Finish = 4,
    }
    
    public class CaptchaEvent : Event<CaptchaEvent>
    {
        public static CaptchaEvent Get()
        {
            return GetPooledInternal();
        }
    }
}