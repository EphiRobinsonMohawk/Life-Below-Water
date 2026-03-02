using UnityEngine;

public class SimpleFishSwim : MonoBehaviour
{
    [Header("Movement")]
    public float swimDistance = 5f;
    public float swimSpeed = 2f;

    [Header("Wiggle")]
    public float wiggleAmount = 10f;
    public float wiggleSpeed = 5f;

    private Vector3 startPos;
    private bool movingRight = true;

    private Quaternion baseRotation;

    void Start()
    {
        startPos = transform.position;
        baseRotation = transform.rotation; // Store original rotation
    }

    void Update()
    {
        Move();
        Wiggle();
    }

    void Move()
    {
        float step = swimSpeed * Time.deltaTime;

        if (movingRight)
        {
            transform.position += Vector3.right * step;

            if (transform.position.x >= startPos.x + swimDistance)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.position += Vector3.left * step;

            if (transform.position.x <= startPos.x - swimDistance)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void Wiggle()
    {
        float wiggle = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
        Quaternion wiggleRotation = Quaternion.Euler(0, 0, wiggle);

        transform.rotation = baseRotation * wiggleRotation;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}