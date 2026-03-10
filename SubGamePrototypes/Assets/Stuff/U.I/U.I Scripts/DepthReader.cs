using UnityEngine;
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
    public bool invertTapeDirection = false;

    [Header("Depth Text")]
    public TextMeshProUGUI depthText;

    [Header("Camera Label")]
    public CameraManager cameraManager;
    public TextMeshProUGUI cameraText;

    [Header("Canvas / Camera")]
    public Canvas targetCanvas;

    [Header("Assign Cameras Here")]
    public Camera controlCamera;
    public Camera frontCamera;
    public Camera rightCamera;
    public Camera leftCamera;
    public Camera thirdPersonCamera;

    [Header("Debug")]
    public bool logDepth = false;
    public float logInterval = 0.5f;

    private float smoothOffset;
    private float currentDepth;
    private float logTimer;
    private CameraManager.ActiveCamera lastCamera;

    void Start()
    {
        UpdateDepth();
        UpdateDepthTape(true);
        UpdateDepthText();
        UpdateCameraText(true);
        UpdateCanvasCamera();
    }

    void Update()
    {
        UpdateDepth();
        UpdateDepthTape(false);
        UpdateDepthText();
        UpdateCameraText(false);
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

    void UpdateDepthTape(bool instant)
    {
        if (depthContent == null)
            return;

        float targetOffset = currentDepth * pixelsPerMeter;

        if (!invertTapeDirection)
            targetOffset *= -1f;

        if (instant)
        {
            smoothOffset = targetOffset;
        }
        else
        {
            smoothOffset = Mathf.Lerp(smoothOffset, targetOffset, Time.deltaTime * smoothSpeed);
        }

        depthContent.anchoredPosition = new Vector2(depthContent.anchoredPosition.x, smoothOffset);
    }

    void UpdateDepthText()
    {
        if (depthText == null)
            return;

        depthText.text = currentDepth.ToString("F1") + " m";
    }

    void UpdateCameraText(bool forceUpdate)
    {
        if (cameraManager == null || cameraText == null)
            return;

        if (!forceUpdate && cameraManager.activeCamera == lastCamera)
            return;

        lastCamera = cameraManager.activeCamera;

        switch (lastCamera)
        {
            case CameraManager.ActiveCamera.Control:
                cameraText.text = "C";
                break;

            case CameraManager.ActiveCamera.Front:
                cameraText.text = "1";
                break;

            case CameraManager.ActiveCamera.Right:
                cameraText.text = "2";
                break;

            case CameraManager.ActiveCamera.Left:
                cameraText.text = "3";
                break;

            case CameraManager.ActiveCamera.ThirdPerson:
                cameraText.text = "4";
                break;

            default:
                cameraText.text = "CAM";
                break;
        }
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