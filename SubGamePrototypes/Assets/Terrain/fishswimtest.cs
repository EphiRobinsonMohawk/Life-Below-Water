using UnityEngine;

public class SimpleFishSwim : MonoBehaviour
{
    [Header("Movement")]
    public float swimDistance = 5f;
    public float swimSpeed = 2f;

    [Header("Turning")]
    public float turnSpeed = 120f;

    [Header("Body Wiggle")]
    public float wiggleAmount = 10f;
    public float wiggleSpeed = 5f;

    [Header("Fins")]
    public Transform pectoralFin;
    public Transform dorsalFin;
    public Transform analFin;

    [Header("Fin Animation")]
    public float finSpeed = 8f;
    public float pectoralAmount = 25f;
    public float dorsalAmount = 5f;
    public float analAmount = 5f;

    private bool turning = false;

    private float turnTargetY;
    private float finOffset;
    private float traveledDistance;

    private Quaternion baseRotation;

    private Quaternion pectoralStartRotation;
    private Quaternion dorsalStartRotation;
    private Quaternion analStartRotation;

    void Start()
    {
        baseRotation = transform.rotation;
        finOffset = Random.Range(0f, 10f);
        traveledDistance = 0f;

        if (pectoralFin != null)
            pectoralStartRotation = pectoralFin.localRotation;

        if (dorsalFin != null)
            dorsalStartRotation = dorsalFin.localRotation;

        if (analFin != null)
            analStartRotation = analFin.localRotation;
    }

    void Update()
    {
        if (turning)
            Turn();
        else
            Move();

        Wiggle();
        AnimateFins();
    }

    void Move()
    {
        float step = swimSpeed * Time.deltaTime;
        transform.position += transform.forward * step;
        traveledDistance += step;

        if (traveledDistance >= swimDistance)
        {
            traveledDistance = 0f;
            StartTurn();
        }
    }

    void StartTurn()
    {
        turning = true;
        turnTargetY = transform.eulerAngles.y + 180f;
    }

    void Turn()
    {
        float newY = Mathf.MoveTowardsAngle(
            transform.eulerAngles.y,
            turnTargetY,
            turnSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(0f, newY, 0f);

        if (Mathf.Abs(Mathf.DeltaAngle(newY, turnTargetY)) < 1f)
        {
            transform.rotation = Quaternion.Euler(0f, turnTargetY, 0f);
            baseRotation = transform.rotation;
            turning = false;
        }
    }

    void Wiggle()
    {
        if (turning) return;

        float wiggle = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
        Quaternion wiggleRotation = Quaternion.Euler(0f, wiggle, 0f);

        transform.rotation = baseRotation * wiggleRotation;
    }

    void AnimateFins()
    {
        float finWave = Mathf.Sin((Time.time + finOffset) * finSpeed);

        if (pectoralFin != null)
        {
            pectoralFin.localRotation =
                pectoralStartRotation * Quaternion.Euler(finWave * pectoralAmount, 0f, 0f);
        }

        if (dorsalFin != null)
        {
            dorsalFin.localRotation =
                dorsalStartRotation * Quaternion.Euler(0f, 0f, finWave * dorsalAmount);
        }

        if (analFin != null)
        {
            analFin.localRotation =
                analStartRotation * Quaternion.Euler(0f, 0f, -finWave * analAmount);
        }
    }
}