using UnityEngine;

public class CompassController : MonoBehaviour
{
    public Transform player;
    public RectTransform compassContent;

    public float pixelsPerDegree = 5f;

    float smoothX;

    void Update()
    {
        float yaw = player.eulerAngles.y;
        float target = -yaw * pixelsPerDegree;

        smoothX = Mathf.Lerp(smoothX, target, Time.deltaTime * 5f);

        compassContent.anchoredPosition =
            new Vector2(smoothX, compassContent.anchoredPosition.y);
    }
}
