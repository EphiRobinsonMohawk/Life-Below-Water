using UnityEngine;

public class SimpleFishSwim : MonoBehaviour
{
    [Header("References")]
    public Transform fishVisual;

    [Header("Movement")]
    public float swimDistance = 5f;
    public float swimSpeed = 2f;

    [Header("Wiggle")]
    public float wiggleAmount = 10f;
    public float wiggleSpeed = 5f;

    private Vector3 startPos;
    private bool movingForward = true;

    private Quaternion visualBaseRotation;

    void Start()
    {
        startPos = transform.position;

        if (fishVisual != null)
            visualBaseRotation = fishVisual.localRotation;
    }

    void Update()
    {
        Move();
        Wiggle();
    }

    void Move()
    {
        float step = swimSpeed * Time.deltaTime;

        if (movingForward)
        {
            transform.position += transform.forward * step;

            if (Vector3.Distance(startPos, transform.position) >= swimDistance)
            {
                movingForward = false;
                Flip();
            }
        }
        else
        {
            transform.position -= transform.forward * step;

            if (Vector3.Distance(startPos, transform.position) <= 0.2f)
            {
                movingForward = true;
                Flip();
            }
        }
    }

    void Wiggle()
    {
        if (fishVisual == null) return;

        float wiggle = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
        fishVisual.localRotation = visualBaseRotation * Quaternion.Euler(0f, wiggle, 0f);
    }

    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }
}