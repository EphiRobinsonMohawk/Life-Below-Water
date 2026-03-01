using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public Rigidbody playerRb;
    public RectTransform needle;
    public TextMeshProUGUI speedText;

    public float maxSpeed = 50f;
    public float minAngle = -130f;
    public float maxAngle = 130f;

    void Update()
    {
        float speed = playerRb.linearVelocity.magnitude * 3.6f;

        // Rotate needle
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);
        float angle = Mathf.Lerp(minAngle, maxAngle, normalizedSpeed);
        needle.localRotation = Quaternion.Euler(0, 0, angle);

        // Update integer text
        int speedInt = Mathf.RoundToInt(speed);
        speedText.text = speedInt.ToString();
    }
}