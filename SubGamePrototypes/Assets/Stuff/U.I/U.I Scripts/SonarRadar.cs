using UnityEngine;

public class SonarRadar : MonoBehaviour
{
    public Transform player;
    public RectTransform radarRect;
    public RectTransform sweep;
    public RectTransform blipsContainer;

    public GameObject blipPrefab;

    public float radarRange = 50f;
    public float revealAngleThreshold = 4f;

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Vector3 direction = enemy.transform.position - player.position;

            if (direction.magnitude > radarRange)
                continue;

            float angleToEnemy = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float sweepAngle = sweep.eulerAngles.z;

            float angleDifference = Mathf.DeltaAngle(-angleToEnemy, sweepAngle);

            if (Mathf.Abs(angleDifference) < revealAngleThreshold)
            {
                CreateBlip(direction);
            }
        }
    }

    void CreateBlip(Vector3 worldOffset)
    {
        Vector2 radarPos = new Vector2(worldOffset.x, worldOffset.z);
        radarPos = radarPos / radarRange * (radarRect.sizeDelta.x / 2f);

        GameObject blip = Instantiate(blipPrefab, blipsContainer);
        blip.GetComponent<RectTransform>().anchoredPosition = radarPos;

        Destroy(blip, 0.6f);
    }
}