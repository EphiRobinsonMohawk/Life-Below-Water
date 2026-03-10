using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyToggleIcon : MonoBehaviour
{
    public Image iconImage;

    public Sprite onSprite;
    public Sprite offSprite;

    public KeyCode keyboardKey = KeyCode.F;

    bool isOn = false;

    void Update()
    {
        bool keyboardPressed = Input.GetKeyDown(keyboardKey);
        bool controllerPressed = Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame;

        if (keyboardPressed || controllerPressed)
        {
            isOn = !isOn;
            UpdateIcon();
        }
    }

    void UpdateIcon()
    {
        iconImage.sprite = isOn ? onSprite : offSprite;

        iconImage.color = isOn
            ? new Color(1f, 1f, 1f, 1f)
            : new Color(1f, 1f, 1f, 0.4f);

        transform.localScale = isOn
            ? Vector3.one * 1.05f
            : Vector3.one;
    }
}