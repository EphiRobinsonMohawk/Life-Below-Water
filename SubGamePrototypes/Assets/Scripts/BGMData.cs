using System;
using UnityEngine;

public enum Song { TemporaryWaterMusic }

[Serializable]
public struct BGMData
{
    public AudioClip clip;
    public Song song;
    public float duration;
    public float volume;

}