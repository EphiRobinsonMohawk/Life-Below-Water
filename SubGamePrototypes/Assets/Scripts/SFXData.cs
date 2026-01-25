using System;
using UnityEngine;


public enum SoundEffect { WaterJet }
[Serializable]
public struct SFXData
{
    public SoundEffect effect;
    public float volume;
    public float pitch;
    public AudioClip clip;
    public float duration;
    public bool shouldLoop;
}

