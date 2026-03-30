using System;
using UnityEngine;

public enum Song { TemporaryWaterMusic,UnderWaterMotifV2 }

[Serializable]
public struct BGMData
{
    public AudioClip clip;
    public Song song;
    public float duration;
    public float volume;

}