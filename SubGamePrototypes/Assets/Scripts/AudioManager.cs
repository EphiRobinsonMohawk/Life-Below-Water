using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SFXData[] sfxs;
    public AudioSource sfxSource;
    public AudioSource bgMusicSource;

    public void PlayOneShotSFX(SFXData sfx)
    {
        sfxSource.clip = sfx.clip;
        sfxSource.volume = sfx.volume;
        sfxSource.pitch = sfx.pitch;
        sfxSource.loop = sfx.shouldLoop;
        sfxSource.PlayOneShot(sfxSource.clip);
    }

    public void PlaySFX(SFXData sfx)
    {
        sfxSource.clip = sfx.clip;
        sfxSource.volume = sfx.volume;
        sfxSource.pitch = sfx.pitch;
        sfxSource.loop = sfx.shouldLoop;
        sfxSource.Play();
    }
}
