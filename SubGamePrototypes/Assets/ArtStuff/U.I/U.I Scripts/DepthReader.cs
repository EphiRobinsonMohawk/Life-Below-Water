using UnityEngine;

public class DepthReader : MonoBehaviour
{
    public Transform player;
    public RectTransform depthContent;

    public float pixelsPerMeter = 4f;

    void Update()
    {
        float depth = -player.position.y;

        float offset = depth * pixelsPerMeter;

        depthContent.anchoredPosition = new Vector2(0, offset);
    }
}