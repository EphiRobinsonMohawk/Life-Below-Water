using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UnderwaterVisuals : MonoBehaviour
{
    // set this to your main cam (or leave blank and it will try grab Camera.main)
    public Transform cam;

    // your underwater post fx volume (weight gets lerped 0 -> 1)
    public Volume underwaterVolume;

    // optional UI overlay (CanvasGroup). alpha gets lerped too
    public CanvasGroup overlay;

    // water surface height in world space
    public float waterY = 0f;

    // how "thick" the blend area is near the surface
    public float blendRange = 2.5f;

    // fog stuff
    public bool controlFog = true;
    public Color fogAbove = new Color(0.7f, 0.85f, 0.95f, 1f);
    public float fogDensityAbove = 0.002f;
    public Color fogUnder = new Color(0.05f, 0.35f, 0.45f, 1f);
    public float fogDensityUnder = 0.03f;

    // ambient (optional)
    public bool controlAmbient = true;
    public Color ambientAbove = new Color(0.9f, 0.9f, 0.9f, 1f);
    public Color ambientUnder = new Color(0.1f, 0.25f, 0.25f, 1f);

    // higher = snappier, lower = smoother
    public float lerpSpeed = 6f;

    float blend; // 0 above, 1 underwater

    void Reset()
    {
        if (Camera.main) cam = Camera.main.transform;
    }

    void Awake()
    {
        if (!cam && Camera.main) cam = Camera.main.transform;

        // if we are controlling fog, just force it on so it actually shows up
        if (controlFog) RenderSettings.fog = true;

        if (underwaterVolume) underwaterVolume.weight = 0f;
        if (overlay) overlay.alpha = 0f;
    }

    void Update()
    {
        if (!cam) return;

        float target = GetBlend01();

        // smoother than normal lerp, feels less "swimmy"
        blend = Mathf.Lerp(blend, target, 1f - Mathf.Exp(-lerpSpeed * Time.deltaTime));

        if (controlFog)
        {
            RenderSettings.fogColor = Color.Lerp(fogAbove, fogUnder, blend);
            RenderSettings.fogDensity = Mathf.Lerp(fogDensityAbove, fogDensityUnder, blend);
        }

        if (controlAmbient)
        {
            RenderSettings.ambientLight = Color.Lerp(ambientAbove, ambientUnder, blend);
        }

        if (underwaterVolume)
        {
            underwaterVolume.weight = blend;
        }

        if (overlay)
        {
            overlay.alpha = blend;
        }
    }

    float GetBlend01()
    {
        // blend band is from waterY+range (fully above) to waterY-range (fully under)
        float y = cam.position.y;
        float t = Mathf.InverseLerp(waterY + blendRange, waterY - blendRange, y);
        return Mathf.Clamp01(t);
    }
}
