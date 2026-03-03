using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ShutterEffect : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Header("Settings")]
    public float fadeInTime = 0.05f;
    public float fadeOutTime = 0.2f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void TriggerEffect()
    {
        StopAllCoroutines();
        StartCoroutine(ShutterRoutine());
    }

    private IEnumerator ShutterRoutine()
    {
        float timer = 0f;
        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        timer = 0f;

        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
