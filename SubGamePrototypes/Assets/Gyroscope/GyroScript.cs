using UnityEngine;

public class GyroScript : MonoBehaviour
{
    public GameObject ROVSub;
    public GameObject GyroSub;

    void Update()
    {
        if (ROVSub == null) return;
        if (GyroSub == null) return;

        Quaternion rov = ROVSub.transform.localRotation;
        Quaternion gyro = GyroSub.transform.localRotation;

        Quaternion rovYaw = TwistAroundAxis(rov, Vector3.up);
        Quaternion rovTilt = rov * Quaternion.Inverse(rovYaw);

        Quaternion gyroYaw = TwistAroundAxis(gyro, Vector3.up);

        GyroSub.transform.localRotation = rovTilt * gyroYaw;
    }

    Quaternion TwistAroundAxis(Quaternion q, Vector3 axis)
    {
        axis.Normalize();
        Vector3 v = new Vector3(q.x, q.y, q.z);
        float d = Vector3.Dot(v, axis);
        Quaternion twist = new Quaternion(axis.x * d, axis.y * d, axis.z * d, q.w);
        return Quaternion.Normalize(twist);
    }

    //Twists fix the face switching issue when over 180 degrees, I don't get them ngl I got this fix from georgina

}