using UnityEngine;

public class ClamController : MonoBehaviour
{
    public float maxOpenAngle;
    public float openSpeed;
    public bool isOpen;
    public GameObject clamLid;
    public float openTimer;
    public float minOpenTime = 2.5f;
    public float maxOpenTime = 7f;
    public float minCloseTime = 1.5f;
    public float maxCloseTime = 5f;
    private float closeTime = 3f; //how long it takes to close after opening
    private float openTime = 6f; //how long it takes to open after closing
    public ParticleSystem bubbles;

    void Update()
    {
        openTimer += Time.deltaTime;

        if (openTimer > openTime && !isOpen)
        {
            isOpen = true;
            openTimer = 0;
            closeTime = (Random.Range(minCloseTime, maxCloseTime));
            bubbles.Play();
        }
        else if (openTimer > closeTime && isOpen)
        {
            isOpen = false;
            openTimer = 0;
            openTime = (Random.Range(minOpenTime, maxOpenTime));
        }


        if (clamLid == null) return;

        Vector3 rot = clamLid.transform.localEulerAngles;

        if (rot.x > 180f)
        {
            rot.x -= 360f;
        }

        if (isOpen)
        {
            if (rot.x < maxOpenAngle)
            {
                rot.x += openSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (rot.x > -25.75f)
            {
                rot.x -= openSpeed * Time.deltaTime;
            }
        }

        clamLid.transform.localEulerAngles = rot;
    }
}