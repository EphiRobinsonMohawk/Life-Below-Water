using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum InputState {  Hercules, ControlRoom, Grabber, Suction, Menus }
    public InputState state;
    public InputState previousState;
    public SubMovement subMovement;
    public PlayerMovement playerMovement;


    void Start()
    {
        state = InputState.Menus;
    }

    public void InputHandling()
    {
        switch (state)
        {
            case InputState.Hercules:
                subMovement.ControlHercules();
                subMovement.controllingHerc = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case InputState.ControlRoom:
                playerMovement.PlayerRotation();
                playerMovement.Interaction();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case InputState.Grabber:
                Debug.Log("Input state: " + state + " is not setup!");
                break;
            case InputState.Suction:
                Debug.Log("Input state: " + state + " is not setup!");
                break;
            case InputState.Menus:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }
}
