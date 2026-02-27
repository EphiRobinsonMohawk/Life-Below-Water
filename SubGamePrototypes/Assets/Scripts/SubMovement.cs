using UnityEngine;
using UnityEngine.InputSystem;

public class SubMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 1f;
    public float rotForce = 0.25f;
    public float ascendForce = 1f;
    public AudioManager audioManager;
    public InputManager inputManager;
    public CameraManager cameraManager;
    public float jetAudioCooldown = 1f;
    public float jetTimerMax = 1f;
    public bool jetTimer;
    public bool cameraView = false;
    public bool controllingHerc = false;
    public InputAction hMovement;
    public InputAction vMovement;
    public InputAction roll;
    public InputAction pitch;
    public InputAction yaw;
    public InputAction brake;
    public InputAction frontCam;
    public InputAction leftCam;
    public InputAction rightCam;
    public InputAction thirdpersonCam;
    public InputAction camView;
    public InputAction exit;
    public InputAction stabilize;



    void Start()
    {
        hMovement = InputSystem.actions.FindAction("ROV/Move");
        vMovement = InputSystem.actions.FindAction("ROV/VMove");
        roll = InputSystem.actions.FindAction("ROV/Roll");
        pitch = InputSystem.actions.FindAction("ROV/Pitch");
        yaw = InputSystem.actions.FindAction("ROV/Yaw");
        brake = InputSystem.actions.FindAction("ROV/Brake");
        frontCam = InputSystem.actions.FindAction("ROV/FrontCam");
        leftCam = InputSystem.actions.FindAction("ROV/LeftCam");
        rightCam = InputSystem.actions.FindAction("ROV/RightCam");
        thirdpersonCam = InputSystem.actions.FindAction("ROV/ThirdPersonCam");
        camView = InputSystem.actions.FindAction("ROV/CamView");
        exit = InputSystem.actions.FindAction("ROV/Exit");
        stabilize = InputSystem.actions.FindAction("ROV/Stabilize");
    }
    
    public void ControlHercules()
    {
        //Movement
        Vector2 hVector = hMovement.ReadValue<Vector2>();
        float h = hVector.x;
        float v = hVector.y;
        rb.AddForce(transform.forward * -h * moveSpeed);
        rb.AddForce(transform.right * v * moveSpeed);
        //Ascend
        float vFloat = vMovement.ReadValue<float>();
        Debug.Log(vFloat);
        rb.AddRelativeForce(transform.up * vFloat * ascendForce);
          
        //Brakes
        if (brake.IsPressed())
        {
            Debug.Log("Braking!");
            rb.linearVelocity -= rb.linearVelocity / 50;
            if (rb.linearVelocity.magnitude < 0.01f)
            {
                rb.linearVelocity = Vector3.zero;
            }
            rb.angularVelocity -= rb.angularVelocity / 50;
            if (rb.angularVelocity.magnitude < 0.01f)
            {
                rb.angularVelocity = Vector3.zero;
            }

        }
        //Stabilize
        if(stabilize.IsPressed())
        {
            this.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 15 * Time.deltaTime);
        }

        //Rotation
        float rotH = Input.GetAxis("HorizontalCamera");
        float rotV = Input.GetAxis("VerticalCamera");
        rb.AddRelativeTorque(Vector3.up * rotH * rotForce);
        rb.AddRelativeTorque(Vector3.forward * rotV * rotForce);

        //roll
        float roll = Input.GetAxis("Roll");
        rb.AddRelativeTorque(Vector3.left * roll * rotForce);

        /*
        //SFX
        if (Input.GetAxis("HorizontalMovement") != 0 && !jetTimer || Input.GetAxis("VerticalMovement") != 0 && !jetTimer)
        {
            jetTimer = true;
            audioManager.PlayOneShotSFX(audioManager.sfxsData[0]);
        }

        //Timers
        //Movement audio timer.
        if (jetTimer)
        {
            jetAudioCooldown -= Time.deltaTime;
            if (jetAudioCooldown < 0)
            {
                jetAudioCooldown = 1;
                jetTimer = false;

            }
        } */
    }


    public void HercCamControl()
    {
        if(controllingHerc)
        {
            //Exit Hercules
            if (exit.WasPerformedThisFrame())
            {
                controllingHerc = false;
                inputManager.state = InputManager.InputState.ControlRoom;
                cameraManager.activeCamera = CameraManager.ActiveCamera.Control;
            }

            //Change Hercules Camera View
            if (frontCam.WasPerformedThisFrame() && cameraView)
            {
                cameraManager.activeCamera = CameraManager.ActiveCamera.Front;
                Debug.Log(" 1 pressed ");
            }
            if (rightCam.WasPerformedThisFrame() && cameraView)
            {
                cameraManager.activeCamera = CameraManager.ActiveCamera.Right;
            }
            if (leftCam.WasPerformedThisFrame() && cameraView)
            {
                cameraManager.activeCamera = CameraManager.ActiveCamera.Left;
            }
            if (thirdpersonCam.WasPerformedThisFrame() && cameraView)
            {
                cameraManager.activeCamera = CameraManager.ActiveCamera.ThirdPerson;
            }

            //Enter/Exit Camera View
            if (camView.WasPerformedThisFrame())
            {
                if (!cameraView)
                {
                    cameraManager.activeCamera = CameraManager.ActiveCamera.Front;
                    cameraView = true;
                }
                else
                {
                    cameraManager.activeCamera = CameraManager.ActiveCamera.Control;
                    cameraView = false;
                }

            }


           
            
        }
        
    }
}
