using UnityEngine;
using System.Collections;

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

    [Header("Movement Fade")]
    public float movementFadeDuration = 0.25f;

    private Coroutine movementFadeCoroutine;
    private float movementTargetVolume = 1f;

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
        if (movementFadeCoroutine != null)
        {
            StopCoroutine(movementFadeCoroutine);
            movementFadeCoroutine = null;
        }

        movementSource.clip = sfx.clip;
        movementTargetVolume = sfx.volume;
        movementSource.volume = movementTargetVolume;
        movementSource.pitch = sfx.pitch;
        movementSource.loop = sfx.shouldLoop;

        // avoids stacking the same sound 900 times a second
        if (movementSource.clip != sfx.clip)
        {
            movementSource.clip = sfx.clip;
        }

        if (!movementSource.isPlaying)
        {
            movementSource.Play();
        }
    }

    public void PauseMovementEffect()
    {
        if (movementFadeCoroutine != null)
        {
            StopCoroutine(movementFadeCoroutine);
        }

        movementFadeCoroutine = StartCoroutine(FadeOutMovement());
    }

    private IEnumerator FadeOutMovement()
    {
        float startVolume = movementSource.volume;
        float time = 0f;

        while (time < movementFadeDuration)
        {
            time += Time.deltaTime;
            movementSource.volume = Mathf.Lerp(startVolume, 0f, time / movementFadeDuration);
            yield return null;
        }

        movementSource.volume = 0f;
        movementSource.Stop();

        // reset so next play starts at normal volume
        movementSource.volume = movementTargetVolume;
        movementFadeCoroutine = null;
    }

    public float PlaySFX(SFXData sfx)
    {
        // dont use this
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