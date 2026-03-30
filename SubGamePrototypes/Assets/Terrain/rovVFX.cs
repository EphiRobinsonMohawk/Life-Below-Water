using UnityEngine;
using UnityEngine.InputSystem;

public class RovThrusterVFX : MonoBehaviour
{
    [Header("Propeller Pivots")]
    public Transform[] forwardPropellers;   // spins when strafing (A/D)
    public Transform[] strafePropellers;    // spins when moving forward/back (W/S)
    public Transform[] verticalPropellers;  // spins when ascending/descending

    [Header("Particles")]
    public ParticleSystem[] forwardParticles;
    public ParticleSystem[] strafeParticles;
    public ParticleSystem[] verticalParticles;

    [Header("Spin Settings")]
    public float spinSpeed = 1500f;
    public float deadzone = 0.1f;
    public Vector3 spinAxis = Vector3.forward;

    private InputAction move;
    private InputAction vMove;

    void Start()
    {
        move = InputSystem.actions.FindAction("ROV/Move");
        vMove = InputSystem.actions.FindAction("ROV/VMove");

        if (move == null) Debug.LogWarning("Missing InputAction: ROV/Move");
        if (vMove == null) Debug.LogWarning("Missing InputAction: ROV/VMove");
    }

    void Update()
    {
        Vector2 moveInput = move != null ? move.ReadValue<Vector2>() : Vector2.zero;
        float verticalInput = vMove != null ? vMove.ReadValue<float>() : 0f;

        // Swapped like you asked:
        float forwardInput = moveInput.x;
        float strafeInput = moveInput.y;

        bool forwardActive = Mathf.Abs(forwardInput) > deadzone;
        bool strafeActive = Mathf.Abs(strafeInput) > deadzone;
        bool verticalActive = Mathf.Abs(verticalInput) > deadzone;

        if (strafeActive)
            SpinProps(forwardPropellers);

        if (forwardActive)
            SpinProps(strafePropellers);

        if (verticalActive)
            SpinProps(verticalPropellers);

        SetParticles(forwardParticles, strafeActive);
        SetParticles(strafeParticles, forwardActive);
        SetParticles(verticalParticles, verticalActive);
    }

    void SpinProps(Transform[] props)
    {
        if (props == null) return;

        foreach (Transform prop in props)
        {
            if (prop == null) continue;
            prop.Rotate(spinAxis * spinSpeed * Time.deltaTime, Space.Self);
        }
    }

    void SetParticles(ParticleSystem[] systems, bool active)
    {
        if (systems == null) return;

        foreach (ParticleSystem ps in systems)
        {
            if (ps == null) continue;

            if (active)
            {
                if (!ps.isPlaying)
                    ps.Play();
            }
            else
            {
                if (ps.isPlaying)
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }
}