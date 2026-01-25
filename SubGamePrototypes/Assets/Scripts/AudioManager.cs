using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SFXData[] sfxsData;
    public BGMData[] bgmData;
    public AudioSource sfxSource;
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

    public float PlaySFX(SFXData sfx)
    {
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
