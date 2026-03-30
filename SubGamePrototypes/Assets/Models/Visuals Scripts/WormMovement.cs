using UnityEngine;

public class WormMovement : MonoBehaviour
{

    public bool isUp = false;
    public float moveSpeed = 1f;
    public Transform startPOS;
    public Transform endPOS;
    public GameObject worm; 

    void Update()
    {
        // Determine the target based on the isUp flag
        Vector3 targetPosition = isUp ? endPOS.position : startPOS.position;

        // Calculate the step for this frame (speed * deltaTime) to make movement independent of frame rate
        float step = moveSpeed * Time.deltaTime;

        //Ephi here, this is a quick AI script for testing :)
        worm.transform.position = Vector3.MoveTowards(worm.transform.position, targetPosition, step);
    }
}