using UnityEngine;

public class GyroScript : MonoBehaviour
{
    public GameObject ROVSub;
    public GameObject GyroSub;

    void Update()
    {
        if (ROVSub != null && GyroSub != null)
        {
            Vector3 rovRotation = ROVSub.transform.localEulerAngles;
            Vector3 gyroRotation = GyroSub.transform.localEulerAngles;

            gyroRotation.x = rovRotation.x;
            gyroRotation.z = rovRotation.z;

            GyroSub.transform.localEulerAngles = gyroRotation;
        }
    }
}