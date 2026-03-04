using UnityEngine;

public class DepthReader : MonoBehaviour
{
    public Transform player;
    public RectTransform depthContent;
    public float pixelsPerMeter = 4f;

    float smoothOffset;

    void Update()
    {
        float depth = -player.position.y;
        float targetOffset = -depth * pixelsPerMeter;

        smoothOffset = Mathf.Lerp(smoothOffset, targetOffset, Time.deltaTime * 5f);

        depthContent.anchoredPosition = new Vector2(0, smoothOffset);
    }
}