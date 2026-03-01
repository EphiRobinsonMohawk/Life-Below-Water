using UnityEngine;

public class TestplayerControler4UI : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 20f;
    public float strafeForce = 15f;
    public float verticalForce = 15f;

    [Header("Rotation")]
    public float yawSpeed = 60f;
    public float pitchSpeed = 40f;

    [Header("Camera Modes")]
    public int currentCameraMode = 1;
    public int maxCameraModes = 3;
    public CameraModeUI cameraUI;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateCameraUI();
    }

    void Update()
    {
        HandleRotation();
        HandleCameraSwitch();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float forward = Input.GetAxis("Vertical");      // W/S
        float strafe = Input.GetAxis("Horizontal");     // A/D

        float ascend = 0f;
        if (Input.GetKey(KeyCode.Space)) ascend = 1f;
        if (Input.GetKey(KeyCode.LeftControl)) ascend = -1f;

        Vector3 force =
            transform.forward * forward * moveForce +
            transform.right * strafe * strafeForce +
            transform.up * ascend * verticalForce;

        rb.AddForce(force, ForceMode.Acceleration);
    }

    void HandleRotation()
    {
        float yaw = Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;
        float pitch = -Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime;

        transform.Rotate(pitch, yaw, 0f);
    }

    void HandleCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCameraMode++;

            if (currentCameraMode > maxCameraModes)
                currentCameraMode = 1;

            UpdateCameraUI();
        }
    }

    void UpdateCameraUI()
    {
        if (cameraUI != null)
        {
            cameraUI.SetMode(currentCameraMode);
        }
    }
}