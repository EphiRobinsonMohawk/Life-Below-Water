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
    public InputAction Movement;

    void Start()
    {
        Movement = InputSystem.actions.FindAction("ROV/Move");
    }
    
    public void ControlHercules()
    {
        //Movement
        Vector2 hv = Movement.ReadValue<Vector2>();

        float h = hv.x;
        float v = hv.y;
        rb.AddForce(transform.forward * -h * moveSpeed);
        rb.AddForce(transform.right * v * moveSpeed);
        //Ascend
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(0, ascendForce, 0);
        }
        //Descend
        if (Input.GetKey(KeyCode.LeftControl))
        {
            rb.AddRelativeForce(0, -ascendForce, 0);
        }
        //Boost
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 2f;
            ascendForce = 2f;
        }
        else
        {
            moveSpeed = 1f;
            ascendForce = 1f;
        }
        //Brakes
        if (Input.GetKey(KeyCode.LeftAlt))
        {
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
        //Rotation
        float rotH = Input.GetAxis("HorizontalCamera");
        float rotV = Input.GetAxis("VerticalCamera");
        rb.AddRelativeTorque(Vector3.up * rotH * rotForce);
        rb.AddRelativeTorque(Vector3.forward * rotV * rotForce);

        //roll
        float roll = Input.GetAxis("Roll");
        rb.AddRelativeTorque(Vector3.left * roll * rotForce);


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
        }
    }


    public void HercCamControl()
    {
        if(controllingHerc)
        {
            //Exit Hercules
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                controllingHerc = false;
                inputManager.state = InputManager.InputState.ControlRoom;
            }

            //Change Hercules Camera View
            if (Input.GetKeyDown(KeyCode.Alpha1) && cameraView)
            {
                cameraManager.activeCamera = CameraManager.ActiveCamera.Front;
                Debug.Log(" 1 pressed ");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && cameraView)
            {
                cameraManager.activeCamera = CameraManager.ActiveCamera.Right;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && cameraView)
            {
                cameraManager.activeCamera = CameraManager.ActiveCamera.Left;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && cameraView)
            {
                cameraManager.activeCamera = CameraManager.ActiveCamera.ThirdPerson;
            }

            //Enter/Exit Camera View
            if (Input.GetKeyDown(KeyCode.Tab))
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
