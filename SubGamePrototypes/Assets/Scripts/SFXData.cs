using System;
using UnityEngine;


public enum SoundEffect { WaterJet,UIPageSwitch, UI_Discovery, UI_Button, UIAddInventory, Sonar_Loop, Shallow_Ambience, Medium_Ambience, ExtraMedium_Ambience, Left_Movement,
    Right_Movement, Forward_Movement}
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

