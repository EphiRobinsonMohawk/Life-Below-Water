using UnityEngine;

public class GameManager : MonoBehaviour
{
   //References
   public InputManager inputManager;
   public CameraManager cameraManager;
   public SubMovement subMovement;
    void Start()
    {
        inputManager.state = InputManager.InputState.Menus;       
    }

    private void Update()
    {
        cameraManager.CameraControl();
        subMovement.HercCamControl();
        if (inputManager.state != InputManager.InputState.Hercules)
        {
            inputManager.InputHandling();
        }
       
    }
    void FixedUpdate()
    {
        if (inputManager.state == InputManager.InputState.Hercules)
        {
            inputManager.InputHandling();
        }
    }
}
