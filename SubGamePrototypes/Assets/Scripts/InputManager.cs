using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    UnityEvent onEnterControlRoom = new UnityEvent();
    UnityEvent onEnterHercules = new UnityEvent();

    public enum InputState { Hercules, ControlRoom, Grabber, Suction, Menus }
    
    [SerializeField] private InputState _state;
    public InputState state
    {
        get => _state;
        set
        {
            if (_state == value) return;
            previousState = _state;
            _state = value;
            UpdateActionMaps();
        }
    }

    public InputState previousState;
    public SubMovement subMovement;
    public PlayerMovement playerMovement;
    public Image cursor;

    void Start()
    {
        UpdateActionMaps();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateActionMaps()
    {
        // Disable all gameplay-related action maps
        InputSystem.actions.FindActionMap("Player").Disable();
        InputSystem.actions.FindActionMap("UI").Disable();
        InputSystem.actions.FindActionMap("ROV").Disable();
        InputSystem.actions.FindActionMap("Arm").Disable();
        InputSystem.actions.FindActionMap("Photography").Disable();

        // Toggle UI Input Module to prevent background interactions
        var eventSystem = EventSystem.current;
        if (eventSystem != null)
        {
            var uiModule = eventSystem.GetComponent<InputSystemUIInputModule>();
            if (uiModule != null)
            {
                uiModule.enabled = (_state == InputState.Menus);
            }
        }

        // Enable the appropriate action map based on state
        switch (_state)
        {
            case InputState.Hercules:
                InputSystem.actions.FindActionMap("ROV").Enable();
                InputSystem.actions.FindActionMap("Arm").Enable();
                InputSystem.actions.FindActionMap("Photography").Enable();
                break;
            case InputState.ControlRoom:
                InputSystem.actions.FindActionMap("Player").Enable();
                break;
            case InputState.Menus:
                InputSystem.actions.FindActionMap("UI").Enable();
                break;
            case InputState.Grabber:
            case InputState.Suction:
                break;
        }
    }

    public void InputHandling()
    {
        switch (state)
        {
            case InputState.Hercules:
                subMovement.ControlHercules();
                subMovement.controllingHerc = true;
                // Cursor.visible = false;
                //Cursor.lockState = CursorLockMode.Locked;
                cursor.enabled = false;
                onEnterHercules.Invoke();
                break;
            case InputState.ControlRoom:
                playerMovement.PlayerRotation();
                playerMovement.Interaction();
                //Cursor.visible = false;
                //Cursor.lockState = CursorLockMode.Locked;
                cursor.enabled = false;
                onEnterHercules.Invoke();
                break;
            case InputState.Grabber:
                Debug.Log("Input state: " + state + " is not setup!");
                break;
            case InputState.Suction:
                Debug.Log("Input state: " + state + " is not setup!");
                break;
            case InputState.Menus:
                //Cursor.visible = true;
                cursor.enabled = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }
}
