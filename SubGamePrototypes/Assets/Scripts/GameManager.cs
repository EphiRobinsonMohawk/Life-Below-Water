using UnityEngine;

public class GameManager : MonoBehaviour
{
   //References
   public InputManager inputManager;
   public CameraManager cameraManager;
   public SubMovement subMovement;
    void Start()
    {
        Application.targetFrameRate = 30; // Set the target frame rate
        inputManager.state = InputManager.InputState.Menus;       
    }

    private void Update()
    {
        cameraManager.CameraControl();
        subMovement.HercCamControl();
       /* if (inputManager.state != InputManager.InputState.Hercules)
        {
            inputManager.InputHandling();
        }
       */

       
    }
    void FixedUpdate()
    {
        inputManager.InputHandling();
        /* if (inputManager.state == InputManager.InputState.Hercules)
         {
             inputManager.InputHandling();
         }*/
    }
}
