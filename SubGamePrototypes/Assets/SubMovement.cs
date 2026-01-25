using UnityEngine;

public class SubMovement : MonoBehaviour
{

    public Rigidbody rb;
    public float moveSpeed = 1f;
    public float rotForce = 0.25f;
    public float ascendForce = 1f;
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Movement
        float h = Input.GetAxis("HorizontalMovement");
        float v = Input.GetAxis("VerticalMovement");
        rb.AddForce(transform.forward * h * moveSpeed);
        rb.AddForce(transform.right * v * moveSpeed);
        //Ascend
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(0, ascendForce, 0);
        }
        //Descend
        if ( Input.GetKey(KeyCode.LeftControl))
        {
            rb.AddRelativeForce(0, -ascendForce, 0);
        }
        //Boost
        if( Input.GetKey(KeyCode.LeftShift))
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
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            rb.linearVelocity -= rb.linearVelocity / 50;
            if(rb.linearVelocity.magnitude < 0.01f)
            {
                rb.linearVelocity = Vector3.zero;
            }
           
        }
        //Rotation
        //todo: add q and e roll
        float rotH = Input.GetAxis("HorizontalCamera");
        float rotV = Input.GetAxis("VerticalCamera");
        
        rb.AddRelativeTorque(Vector3.up * rotH * rotForce);
        rb.AddRelativeTorque(Vector3.forward * rotV * rotForce);

        //roll

        float roll = Input.GetAxis("Roll");
        rb.AddRelativeTorque(Vector3.left * roll * rotForce);





    }
}
