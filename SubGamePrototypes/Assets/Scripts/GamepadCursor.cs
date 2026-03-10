using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadCursor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private InputManager inputManager;

    [Header("Cursor Settings")]
    [SerializeField] private float cursorSpeed = 1600f;
    [SerializeField] private float deadzone = 0.15f;

    private Mouse virtualMouse;
    private InputAction moveAction;
    private InputAction clickAction;

    private bool lastPressed;
    private Canvas parentCanvas;

    private void OnEnable()
    {
        // Create virtual mouse if needed
        if (virtualMouse == null)
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        moveAction = playerInput.actions["Navigate"];
        clickAction = playerInput.actions["Submit"];

        Cursor.visible = false;

        parentCanvas = canvasRect.GetComponentInParent<Canvas>();

        InputSystem.onAfterUpdate += UpdateCursor;
    }

    private void OnDisable()
    {
        InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateCursor;
    }

    private void UpdateCursor()
    {
        if (inputManager.state != InputManager.InputState.Menus)
            return;

        if (virtualMouse == null || Gamepad.current == null)
            return;

        float dt = Time.unscaledDeltaTime;

        // --- Read stick input ---
        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input.magnitude < deadzone)
            input = Vector2.zero;

        // --- Update virtual mouse position directly (no smoothing) ---
        Vector2 currentPos = virtualMouse.position.ReadValue();
        Vector2 newPos = currentPos + input * cursorSpeed * dt;

        newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
        newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, newPos);
        InputState.Change(virtualMouse.delta, newPos - currentPos);

        // Sync hardware mouse for UI
        if (Mouse.current != null)
            InputState.Change(Mouse.current.position, newPos);

        HandleClick();
        UpdateCursorUI();
    }

    private void HandleClick()
    {
        bool pressed = clickAction.IsPressed();

        if (pressed != lastPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, pressed);
            InputState.Change(virtualMouse, mouseState);
            lastPressed = pressed;
        }
    }

    private void UpdateCursorUI()
    {
        Camera cam = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            virtualMouse.position.ReadValue(), // always use exact virtual mouse
            cam,
            out Vector2 anchoredPos
        );

        cursorTransform.anchoredPosition = anchoredPos;
    }
}