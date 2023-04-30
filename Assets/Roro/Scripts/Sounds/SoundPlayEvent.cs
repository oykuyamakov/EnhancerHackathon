using System.Collections;
using System.Collections.Generic;
using Events;
using Sounds;
using UnityEngine;

public enum SoundType
{
    Walking = 0,
    Jumping = 1,
    Dying = 2,
    Button = 3,
    ScreenCrack = 4,
    Coin = 5,
    Background = 6,
    Intro = 7,
}

public class SoundPlayEvent : Event<SoundPlayEvent>
{
    public Sound Sound;
    public bool Loop;
    public int LoopIndex;

    public SoundType SoundType;
    
    public static SoundPlayEvent Get(Sound sound, bool loop = false)
    {
        var evt = GetPooledInternal();
        evt.Sound = sound;
        evt.Loop = loop;
        return evt;
    }
    
    public static SoundPlayEvent Get(SoundType soundType, bool loop = false)
    {
        var evt = GetPooledInternal();
        evt.SoundType = soundType;
        evt.Loop = loop;
        return evt;
    }
}
