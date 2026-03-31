using UnityEngine;

public class CompassController : MonoBehaviour
{
    public Transform player;
    public Transform homeBase;
    public RectTransform compassContent;
    public RectTransform homeBaseMarker;

    public float rulerWidth = 2000f;

    void Update()
    {
        float yaw = player.eulerAngles.y;

        float offset = yaw * rulerWidth / 360;

        // Wrap position so ruler loops infinitely
        float wrappedOffset = offset % rulerWidth;

        compassContent.anchoredPosition =
            new Vector2(-wrappedOffset, compassContent.anchoredPosition.y);

        // Update home base marker
        if (homeBase != null && homeBaseMarker != null)
        {
            Vector3 directionToHome = homeBase.position - player.position;
            directionToHome.y = 0;

            if (directionToHome.sqrMagnitude > 0.001f)
            {
                if (!homeBaseMarker.gameObject.activeSelf) homeBaseMarker.gameObject.SetActive(true);

                float angleToHome = Mathf.Atan2(directionToHome.x, directionToHome.z) * Mathf.Rad2Deg;
                float relativeAngle = Mathf.DeltaAngle(yaw, angleToHome);

                float xPos = relativeAngle * (rulerWidth / 360f);
                homeBaseMarker.anchoredPosition = new Vector2(xPos, homeBaseMarker.anchoredPosition.y);
            }
            else
            {
                // We are at home base
                homeBaseMarker.gameObject.SetActive(false);
            }
        }
        else if (homeBaseMarker != null && homeBase == null)
        {
            homeBaseMarker.gameObject.SetActive(false);
        }
    }
}