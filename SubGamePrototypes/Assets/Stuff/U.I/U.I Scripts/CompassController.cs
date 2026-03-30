using UnityEngine;

public class CompassController : MonoBehaviour
{
    public Transform player;
    public RectTransform compassContent;

    public float rulerWidth = 2000f;

    void Update()
    {
        float yaw = player.eulerAngles.y;

        float offset = yaw * rulerWidth/360;

        // Wrap position so ruler loops infinitely
        float wrappedOffset = offset % rulerWidth;

        compassContent.anchoredPosition =
            new Vector2(-wrappedOffset, compassContent.anchoredPosition.y);
    }
}