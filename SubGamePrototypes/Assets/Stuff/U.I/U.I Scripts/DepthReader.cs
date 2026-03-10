using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DepthUIController : MonoBehaviour
{
    [Header("Depth References")]
    public Transform surface;
    public Transform rov;

    [Header("Depth Tape")]
    public RectTransform depthContent;
    public float pixelsPerMeter = 4f;
    public float smoothSpeed = 5f;

    [Header("Depth Text")]
    public TextMeshProUGUI depthText;
    public bool logDepth = false;
    public float logInterval = 0.5f;

    [Header("Canvas / Camera")]
    public Canvas targetCanvas;
    public CameraManager cameraManager;

    [Header("Assign Cameras Here")]
    public Camera controlCamera;
    public Camera frontCamera;
    public Camera rightCamera;
    public Camera leftCamera;
    public Camera thirdPersonCamera;

    private float smoothOffset;
    private float currentDepth;
    private float logTimer;

    void Update()
    {
        UpdateDepth();
        UpdateDepthTape();
        UpdateDepthText();
        UpdateCanvasCamera();
        HandleDebugLog();
    }

    void UpdateDepth()
    {
        if (surface == null || rov == null)
            return;

        currentDepth = surface.position.y - rov.position.y;

        if (currentDepth < 0f)
            currentDepth = 0f;
    }

    void UpdateDepthTape()
    {
        if (depthContent == null)
            return;

        float targetOffset = -currentDepth * pixelsPerMeter;
        smoothOffset = Mathf.Lerp(smoothOffset, targetOffset, Time.deltaTime * smoothSpeed);

        depthContent.anchoredPosition = new Vector2(depthContent.anchoredPosition.x, smoothOffset);
    }

    void UpdateDepthText()
    {
        if (depthText == null)
            return;

        depthText.text = currentDepth.ToString("F1") + " m";
    }

    void UpdateCanvasCamera()
    {
        if (targetCanvas == null || cameraManager == null)
            return;

        Camera activeCam = GetActiveCamera();

        if (activeCam != null && targetCanvas.worldCamera != activeCam)
        {
            targetCanvas.worldCamera = activeCam;
        }
    }

    Camera GetActiveCamera()
    {
        switch (cameraManager.activeCamera)
        {
            case CameraManager.ActiveCamera.Control:
                return controlCamera;

            case CameraManager.ActiveCamera.Front:
                return frontCamera;

            case CameraManager.ActiveCamera.Right:
                return rightCamera;

            case CameraManager.ActiveCamera.Left:
                return leftCamera;

            case CameraManager.ActiveCamera.ThirdPerson:
                return thirdPersonCamera;
        }

        return null;
    }

    void HandleDebugLog()
    {
        if (!logDepth)
            return;

        logTimer += Time.deltaTime;

        if (logTimer >= logInterval)
        {
            Debug.Log("Depth: " + currentDepth.ToString("F1") + " m");
            logTimer = 0f;
        }
    }
}