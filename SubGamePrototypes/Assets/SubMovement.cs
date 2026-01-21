using UnityEngine;

public class SubMovement : MonoBehaviour
{

    public Rigidbody rb;
    public float moveSpeed = 3f;
    public float rotForce = 1f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0f, v).normalized;
        rb.AddForce(move * moveSpeed);
    }
}
