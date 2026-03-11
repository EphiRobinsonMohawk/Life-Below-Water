using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyToggleIcon : MonoBehaviour
{
    [Header("UI")]
    public Image iconImage;
    public Sprite onSprite;
    public Sprite offSprite;

    [Header("Keyboard Fallback")]
    public KeyCode keyboardKey = KeyCode.LeftShift; 


    

    [Header("Input Actions")]
    public InputActionReference toggleAction;

    private bool isOn = false;

    void Start()
    {
        UpdateIcon();
    }

    void Update()
    {
        bool keyboardPressed = Input.GetKeyDown(keyboardKey);
        bool controllerPressed = toggleAction != null && toggleAction.action.WasPressedThisFrame();

        if (keyboardPressed || controllerPressed)
        {
            isOn = !isOn;
            UpdateIcon();
        }
    }

    void UpdateIcon()
    {
        if (iconImage == null) return;

        iconImage.sprite = isOn ? onSprite : offSprite;

        iconImage.color = isOn
            ? new Color(1f, 1f, 1f, 1f)
            : new Color(1f, 1f, 1f, 0.4f);

        transform.localScale = isOn
            ? Vector3.one * 1.05f
            : Vector3.one;
    }
}