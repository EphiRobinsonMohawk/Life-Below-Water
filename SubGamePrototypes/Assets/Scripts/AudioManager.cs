using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SFXData[] sfxsData;
    public BGMData[] bgmData;
    public AudioSource sfxSource;
    public AudioSource movementSource;
    public AudioSource bgmSource;
    public Song lastSong;
    public Song song;
    public int randomSong;

    public void Start()
    {
        RandomSong();
    }
    public float PlayOneShotSFX(SFXData sfx)
    {
        sfxSource.clip = sfx.clip;
        sfxSource.volume = sfx.volume;
        sfxSource.pitch = sfx.pitch;
        sfxSource.loop = sfx.shouldLoop;
        sfxSource.PlayOneShot(sfxSource.clip);
        return sfx.duration;
    }

    public void PlayMovementEffect(SFXData sfx)
    {
        movementSource.clip = sfx.clip;
        movementSource.volume = sfx.volume;
        movementSource.pitch = sfx.pitch;
        movementSource.PlayOneShot(sfx.clip);
    }

    public void PauseMovementEffect()
    {
        movementSource.Pause();
    }

    public float PlaySFX(SFXData sfx)
    {
        //dont use this
        sfxSource.clip = sfx.clip;
        sfxSource.volume = sfx.volume;
        sfxSource.pitch = sfx.pitch;
        sfxSource.loop = sfx.shouldLoop;
        sfxSource.Play();
        return sfx.duration;
    }

    public void RandomSong()
    {
        randomSong = Random.Range(0, bgmData.Length);
        bgmSource.clip = bgmData[randomSong].clip;
        bgmSource.volume = bgmData[randomSong].volume;
        lastSong = bgmData[randomSong].song;
        bgmSource.Play();
    }
}
