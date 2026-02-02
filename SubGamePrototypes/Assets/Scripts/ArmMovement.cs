using UnityEngine;
using UnityEngine.InputSystem;

public class ArmMovement : MonoBehaviour
{
    // Setup: Shoulder is fixed with hinge to Upper Arm, Upper Arm has hinge to Lower Arm, Lower Arm has hinge to Wrist, 
    // Wrist has one hinge to HandL, one hinge to HandR
    public float MoveForce = 10f;
    public float Kp_WristLeveling = 100f;
    public float Kd_WristLeveling = 10f;

    // Hand control
    public float TargetOpenness = 0f;
    public float HandKp = 100f;
    public float HandKd = 10f;
    public float MaxOpenAngle = 45f;

    // The wrist is the main driver of arm movement, the player will control this directly
    public Rigidbody Wrist;

    // The hand is connected to the wrist and will follow its movement, it can also open, close, and rotate
    public Rigidbody HandL;
    public Rigidbody HandR;

    // Private _variables for collision detection and gripping
    private HandCollisionDetector _detectorL;
    private HandCollisionDetector _detectorR;

    private Rigidbody _heldObject;
    private FixedJoint _gripJoint;

    // Define input actions
    InputAction leftStick;
    InputAction rightStick;
    InputAction openHand;
    InputAction closeHand;

    Vector3 NextMove = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftStick = InputSystem.actions.FindAction("Arm/LeftStick");
        rightStick = InputSystem.actions.FindAction("Arm/RightStick");
        openHand = InputSystem.actions.FindAction("Arm/OpenHand");
        closeHand = InputSystem.actions.FindAction("Arm/CloseHand");

        _detectorL = HandL.gameObject.AddComponent<HandCollisionDetector>();
        _detectorR = HandR.gameObject.AddComponent<HandCollisionDetector>();
    }

    // Update is called once per frame - get inputs
    void Update()
    {
        // Movement input
        Vector2 moveInput = leftStick.ReadValue<Vector2>();
        Vector2 lookInput = rightStick.ReadValue<Vector2>();

        NextMove = new Vector3(moveInput.x, lookInput.y, moveInput.y);

        // Hand openness input
        float openValue = openHand.ReadValue<float>();
        float closeValue = closeHand.ReadValue<float>();
        TargetOpenness += (openValue - closeValue) * Time.deltaTime;
    }

    // FixedUpdate is independent of frame rate - apply physics
    void FixedUpdate()
    {
        Wrist.AddForce(NextMove * MoveForce);
        ApplyLevelingTorque();
        ApplyHandControl();
        UpdateGrip();
    }


    public Vector3 CalculatePD(Vector3 error, Vector3 currentVelocity, float kp, float kd)
    {
        // PD Control: output = Kp * error - Kd * derivative (velocity in this context)
        return (kp * error) - (kd * currentVelocity);
    }

    private void ApplyLevelingTorque()
    {
        Vector3 currentUp = Wrist.transform.up;
        Vector3 targetUp = Vector3.up;

        Vector3 error = Vector3.Cross(currentUp, targetUp);

        Vector3 torque = CalculatePD(error, Wrist.angularVelocity, Kp_WristLeveling, Kd_WristLeveling);

        Wrist.AddTorque(torque);
    }

    private void ApplyHandControl()
    {
        if (HandL == null || HandR == null) return;

        float angle = TargetOpenness * MaxOpenAngle;

        ApplyHandTorque(HandL, -angle);
        ApplyHandTorque(HandR, angle);
    }

    private void ApplyHandTorque(Rigidbody hand, float targetAngle)
    {
        Quaternion localRot = Quaternion.Inverse(Wrist.rotation) * hand.rotation;

        float currentAngle = localRot.eulerAngles.y;
        if (currentAngle > 180) currentAngle -= 360;

        float angleError = targetAngle - currentAngle;

        Vector3 torqueAxis = Wrist.transform.up;
        Vector3 torque = CalculatePD(torqueAxis * angleError * Mathf.Deg2Rad, hand.angularVelocity - Wrist.angularVelocity, HandKp, HandKd);

        hand.AddTorque(torque);
        Wrist.AddTorque(-torque); // Reaction torque on wrist
    }

    private void UpdateGrip()
    {
        if (_heldObject == null)
        {
            // Check for common object in contact with both hands
            foreach (var rbL in _detectorL.CollidingBodies)
            {
                if (_detectorR.CollidingBodies.Contains(rbL))
                {
                    // Found a candidate!
                    GrabObject(rbL);
                    break;
                }
            }
        }
        else
        {
            // Check if we should release
            if (!_detectorL.CollidingBodies.Contains(_heldObject) || !_detectorR.CollidingBodies.Contains(_heldObject))
            {
                ReleaseObject();
            }
        }
    }

    private void GrabObject(Rigidbody target)
    {
        _heldObject = target;
        _gripJoint = Wrist.gameObject.AddComponent<FixedJoint>();
        _gripJoint.connectedBody = _heldObject;
        _gripJoint.breakForce = Mathf.Infinity;
        _gripJoint.breakTorque = Mathf.Infinity;

        Debug.Log($"Grabbed {_heldObject.name}");
    }

    private void ReleaseObject()
    {
        if (_heldObject != null)
        {
            Debug.Log($"Released {_heldObject.name}");
        }

        if (_gripJoint != null)
        {
            Destroy(_gripJoint);
        }

        _heldObject = null;
        _gripJoint = null;
    }
}
