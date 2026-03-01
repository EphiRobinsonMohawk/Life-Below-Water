using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera controlCam;
    public Camera frontCam;
    public Camera rightCam;
    public Camera leftCam;
    public Camera thirdPersonCam;
    public enum ActiveCamera {  Control, Front, Right, Left, ThirdPerson };
    public ActiveCamera activeCamera;

    public void Start()
    {
       controlCam.depth = 1;
    }
    public void CameraControl()
    {
        switch (activeCamera)
        {
            case ActiveCamera.Control:
                controlCam.depth = 1;
                frontCam.depth = 0;
                rightCam.depth = 0;
                leftCam.depth = 0;
                thirdPersonCam.depth = 0;
                break;
            case ActiveCamera.Front:
                controlCam.depth = 0;
                frontCam.depth = 1;
                rightCam.depth = 0;
                leftCam.depth = 0;
                thirdPersonCam.depth = 0;
                break;
            case ActiveCamera.Right:
                controlCam.depth = 0;
                frontCam.depth = 0;
                rightCam.depth = 1;
                leftCam.depth = 0;
                thirdPersonCam.depth = 0;
                break;
            case ActiveCamera.Left:
                controlCam.depth= 0;
                frontCam.depth= 0;
                rightCam.depth= 0;
                leftCam.depth= 1;
                thirdPersonCam.depth= 0;
                break;
            case ActiveCamera.ThirdPerson:
                controlCam.depth= 0;
                frontCam.depth= 0;
                rightCam.depth= 0;
                leftCam.depth= 0;
                thirdPersonCam.depth= 1;
                break;
            default:
                controlCam.depth = 1;
                frontCam.depth = 0;
                rightCam.depth = 0;
                leftCam.depth = 0;
                thirdPersonCam.depth = 0;
                break;


        }
    }
}
