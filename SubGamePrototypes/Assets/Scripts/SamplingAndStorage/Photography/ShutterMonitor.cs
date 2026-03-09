using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ShutterMonitor : MonoBehaviour
{
    private Material targetMaterial;
    private Color originalColor;

    [Header("Settings")]
    public Color tintColor = Color.black;
    public float fadeInTime = 0.05f;
    public float fadeOutTime = 0.2f;

    private void Awake()
    {
        // Accessing .material automatically creates a local instance 
        // so we don't accidentally modify the shared project asset.
        targetMaterial = GetComponent<Renderer>().material;

        // Store the starting color (supports both URP/HDRP and Standard shaders)
        if (targetMaterial.HasProperty("_BaseColor"))
            originalColor = targetMaterial.GetColor("_BaseColor");
        else
            originalColor = targetMaterial.color;
    }

    public void TriggerEffect()
    {
        StopAllCoroutines();
        StartCoroutine(TintRoutine());
    }

    private IEnumerator TintRoutine()
    {
        float timer = 0f;

        // Fade to the tint color (black)
        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            SetMaterialColor(Color.Lerp(originalColor, tintColor, timer / fadeInTime));
            yield return null;
        }

        SetMaterialColor(tintColor);
        timer = 0f;

        // Fade back to the original color
        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            SetMaterialColor(Color.Lerp(tintColor, originalColor, timer / fadeOutTime));
            yield return null;
        }

        SetMaterialColor(originalColor);
    }

    // Helper method to apply color properly based on the active render pipeline
    private void SetMaterialColor(Color color)
    {
        if (targetMaterial.HasProperty("_BaseColor"))
            targetMaterial.SetColor("_BaseColor", color);
        else
            targetMaterial.color = color;
    }

    private void OnDestroy()
    {
        // Prevent memory leaks by destroying the instanced material when the object dies
        if (targetMaterial != null)
        {
            Destroy(targetMaterial);
        }
    }
}