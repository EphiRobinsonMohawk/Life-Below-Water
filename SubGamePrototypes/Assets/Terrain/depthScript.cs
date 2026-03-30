using UnityEngine;
using TMPro; // remove if you are not using TextMeshPro

public class DepthMeter : MonoBehaviour
{
    [Header("References")]
    public Transform surface;        // The object representing the water surface
    public Transform rov;            // Your submarine / ROV

    [Header("Optional UI")]
    public TextMeshProUGUI depthText;

    float currentDepth;

    void Update()
    {
        CalculateDepth();
        DisplayDepth();
    }

    void CalculateDepth()
    {
        // Depth is the vertical distance between surface and ROV
        currentDepth = surface.position.y - rov.position.y;

        // Prevent negative values if ROV goes above surface
        if (currentDepth < 0)
            currentDepth = 0;
    }

    void DisplayDepth()
    {
        // Debug log
        Debug.Log("Depth: " + currentDepth.ToString("F1") + " m");

        // UI display
        if (depthText != null)
        {
            depthText.text = "Depth: " + currentDepth.ToString("F1") + " m";
        }
    }
}