using UnityEngine;
using UnityEngine.InputSystem;

public class ArmMovement : MonoBehaviour
{
    // Setup: Shoulder is fixed with hinge to Upper Arm, Upper Arm has hinge to Lower Arm, Lower Arm has hinge to Wrist, 
    // Wrist has HandL and HandR as children
    public float ForceMultiplier = 1f;
    public float MoveSpeed = 1f;
    public Vector3 TargetRelativePosition;
    public float Kp_Position = 100f;
    public float Kd_Position = 10f;
    public float MaxPositionForce = 50f;

    public float Kp_WristLeveling = 100f;
    public float Kd_WristLeveling = 10f;
    public float MaxWristTorque = 50f;

    public float Kp_Shoulder = 100f;
    public float Kd_Shoulder = 10f;
    public float MaxShoulderTorque = 50f;

    // Hand control
    public float TargetOpenness = 0f;
    public float MaxOpenAngle = 45f;
    public Vector3 PivotOffset = Vector3.zero;

    // The shoulder is the base of the arm, attached to the sub
    public Rigidbody Shoulder;

    // The wrist is the main driver of arm movement, the player will control this directly
    public Rigidbody Wrist;

    // The hand is connected to the wrist and will follow its movement, it can also open, close, and rotate
    public Transform HandL;
    public Transform HandR;

    // Store original local transforms for pivot-based rotation
    private Vector3 _handLOriginalPos;
    private Quaternion _handLOriginalRot;
    private Vector3 _handROriginalPos;
    private Quaternion _handROriginalRot;

    // Private _variables for collision detection and gripping
    private HandCollisionDetector _detectorL;
    private HandCollisionDetector _detectorR;

    public Rigidbody _heldObject;
    private FixedJoint _gripJoint;
    private bool _isClosing;
    private Collider _triggerL;
    private Collider _triggerR;

    private bool _wasArmControl = false;
    private Quaternion _targetShoulderRotation;

    public SampleStorage Storage;

    // Define input actions
    public InputActionReference leftStick;
    public InputActionReference rightStick;
    public InputActionReference openHand;
    public InputActionReference closeHand;
    public InputAction toggleArm;

    Vector3 NextMove = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Wrist != null)
        {
            TargetRelativePosition = transform.InverseTransformPoint(Wrist.position);
        }

        if (Shoulder != null)
        {
            _targetShoulderRotation = Shoulder.transform.localRotation;
        }

        // Store original local transforms relative to wrist
        _handLOriginalPos = HandL.localPosition;
        _handLOriginalRot = HandL.localRotation;
        _handROriginalPos = HandR.localPosition;
        _handROriginalRot = HandR.localRotation;

        _detectorL = HandL.gameObject.GetComponent<HandCollisionDetector>();
        if (_detectorL == null) _detectorL = HandL.gameObject.AddComponent<HandCollisionDetector>();
        
        _detectorR = HandR.gameObject.GetComponent<HandCollisionDetector>();
        if (_detectorR == null) _detectorR = HandR.gameObject.AddComponent<HandCollisionDetector>();

        _triggerL = GetTrigger(HandL);
        _triggerR = GetTrigger(HandR);

        toggleArm = InputSystem.actions.FindAction("ROV/ToggleArm");
    }

    private Collider GetTrigger(Transform hand)
    {
        foreach (var col in hand.GetComponents<Collider>())
        {
            if (col.isTrigger) return col;
        }
        return null;
    }

    // Update is called once per frame - get inputs
    void Update()
    {
        bool isArmControl = toggleArm != null && toggleArm.IsPressed();

        if (isArmControl)
        {
            // Movement input
            Vector2 moveInput = leftStick.action.ReadValue<Vector2>();
            Vector2 lookInput = rightStick.action.ReadValue<Vector2>();

            NextMove = new Vector3(moveInput.x, lookInput.y, moveInput.y);

            // Hand openness input
            float openValue = openHand.action.ReadValue<float>();
            float closeValue = closeHand.action.ReadValue<float>();

            float delta = (openValue - closeValue) * Time.deltaTime;
            _isClosing = delta < 0;

            // If we are holding an object, don't allow closing further
            if (_heldObject != null && delta < 0)
            {
                delta = 0;
            }

            TargetOpenness += delta;
            TargetOpenness = Mathf.Clamp01(TargetOpenness);
        }
        else
        {
            NextMove = Vector3.zero;
            _isClosing = false;
        }

        if (!isArmControl && _wasArmControl)
        {
            if (Wrist != null)
            {
                TargetRelativePosition = transform.InverseTransformPoint(Wrist.position);
            }

            if (Shoulder != null)
            {
                _targetShoulderRotation = Shoulder.transform.localRotation;
            }
        }

        _wasArmControl = isArmControl;
    }

    // FixedUpdate is independent of frame rate - apply physics
    void FixedUpdate()
    {
        bool isArmControl = toggleArm != null && toggleArm.IsPressed();

        if (isArmControl)
        {
            if (Wrist != null && NextMove.sqrMagnitude > 0)
            {
                Vector3 force = transform.TransformDirection(NextMove) * MoveSpeed;
                Wrist.AddForce(force * ForceMultiplier);
            }
        }
        else
        {
            ApplyMovementPD();
            ApplyShoulderStabilization();
        }

        ApplyLevelingTorque();
        ApplyHandControl();
        UpdateGrip();

        //Try to store the sample if it's pulled behind and under the camera
        CheckForStorage();
    }

    private void CheckForStorage()
    {
        if (_heldObject == null || Storage == null) return;

        Sample sample = _heldObject.GetComponent<Sample>();
        if (sample == null) return;

        Camera mainCam = Camera.main;
        if (mainCam == null) return;

        // Below the view
        Vector3 viewportPos = mainCam.WorldToViewportPoint(_heldObject.position);
        bool isBelowFrustum = viewportPos.y < 0;

        // Z-axis check: Between arm position and camera
        float sampleDepth = mainCam.transform.InverseTransformPoint(_heldObject.position).z;
        float armBaseDepth = mainCam.transform.InverseTransformPoint(transform.position).z;

        // Between means sampleDepth is greater than camera (0) but less than arm base (assuming arm is forward)
        // Or simply between the two values.
        bool isBetweenArmAndCam = false;
        if (armBaseDepth > 0)
        {
            isBetweenArmAndCam = sampleDepth > 0 && sampleDepth < armBaseDepth;
        }
        else
        {
            // If arm is behind camera for some reason
            isBetweenArmAndCam = sampleDepth < 0 && sampleDepth > armBaseDepth;
        }

        if (isBelowFrustum && isBetweenArmAndCam)
        {
            if (Storage.TryStoreSample(sample))
            {
                GameObject sampleObj = _heldObject.gameObject;
                ReleaseObject();
                sampleObj.SetActive(false);
            }
        }
    }


    public Vector3 CalculatePD(Vector3 error, Vector3 currentVelocity, float kp, float kd)
    {
        // PD Control: output = Kp * error - Kd * derivative (velocity in this context)
        return (kp * error) - (kd * currentVelocity);
    }

    private void ApplyMovementPD()
    {
        if (Wrist == null) return;
        
        Vector3 targetWorldPosition = transform.TransformPoint(TargetRelativePosition);
        Vector3 error = targetWorldPosition - Wrist.position;
        Vector3 force = CalculatePD(error, Wrist.linearVelocity, Kp_Position, Kd_Position);
        force = Vector3.ClampMagnitude(force, MaxPositionForce);
        
        Wrist.AddForce(force * ForceMultiplier);
    }

    private void ApplyShoulderStabilization()
    {
        if (Shoulder == null) return;

        Quaternion currentLocalRot = Shoulder.transform.localRotation;
        
        // Calculate rotational error
        Quaternion errorQuaternion = _targetShoulderRotation * Quaternion.Inverse(currentLocalRot);
        errorQuaternion.ToAngleAxis(out float angle, out Vector3 axis);

        if (angle > 180f) angle -= 360f;
        
        if (Mathf.Abs(angle) > 0.001f)
        {
            Vector3 worldAxis = Shoulder.transform.TransformDirection(axis);
            Vector3 torqueError = worldAxis * (angle * Mathf.Deg2Rad);
            
            Vector3 torque = CalculatePD(torqueError, Shoulder.angularVelocity, Kp_Shoulder, Kd_Shoulder);
            torque = Vector3.ClampMagnitude(torque, MaxShoulderTorque);
            
            Shoulder.AddTorque(torque * ForceMultiplier);
        }
    }

    private void ApplyLevelingTorque()
    {
        Vector3 currentUp = Wrist.transform.up;
        Vector3 targetUp = transform.up;

        Vector3 error = Vector3.Cross(currentUp, targetUp);

        Vector3 torque = CalculatePD(error, Wrist.angularVelocity, Kp_WristLeveling, Kd_WristLeveling);
        torque = Vector3.ClampMagnitude(torque, MaxWristTorque);

        Wrist.AddTorque(torque * ForceMultiplier);
    }

    private void ApplyHandControl()
    {
        if (HandL == null || HandR == null) return;

        float angle = TargetOpenness * MaxOpenAngle;

        // Rotate HandL about PivotOffset relative to Wrist
        Quaternion rotL = Quaternion.Euler(0, -angle, 0);
        HandL.localPosition = PivotOffset + rotL * (_handLOriginalPos - PivotOffset);
        HandL.localRotation = rotL * _handLOriginalRot;

        // Rotate HandR about PivotOffset relative to Wrist
        Quaternion rotR = Quaternion.Euler(0, angle, 0);
        HandR.localPosition = PivotOffset + rotR * (_handROriginalPos - PivotOffset);
        HandR.localRotation = rotR * _handROriginalRot;

        // Disable triggers when fully open
        bool triggersEnabled = TargetOpenness < 0.98f;
        if (_triggerL != null) _triggerL.enabled = triggersEnabled;
        if (_triggerR != null) _triggerR.enabled = triggersEnabled;
    }

    private void UpdateGrip()
    {
        if (_heldObject == null)
        {
            // Only grab while closing
            if (!_isClosing) return;

            // Check for common object in contact with both hands
            foreach (var rbL in _detectorL.CollidingBodies)
            {
                if (_detectorR.CollidingBodies.Contains(rbL))
                {
                    // Found a candidate!
                    if (rbL.gameObject.activeInHierarchy)
                    {
                        GrabObject(rbL);
                        break;
                    }
                }
            }
        }
        else
        {
            // Release if hand is fully open
            if (TargetOpenness > 0.98f)
            {
                ReleaseObject();
                return;
            }

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
