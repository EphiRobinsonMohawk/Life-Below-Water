using UnityEngine;

public class RadarSweep : MonoBehaviour
{
    public float rotationSpeed = 60f; // degrees per second
    void Update()
    {
        float rotation = rotationSpeed * Time.deltaTime;
        transform.localRotation *= Quaternion.Euler(0f, 0f, -rotation);
    }
}