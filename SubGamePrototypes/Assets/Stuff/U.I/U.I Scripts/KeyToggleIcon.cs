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


    

    [Header("Sync")]
    public SubMovement subMovement;

    private bool isOn = false;

    void Start()
    {
        if (subMovement == null)
        {
            subMovement = FindAnyObjectByType<SubMovement>();
        }

        if (subMovement != null)
        {
            isOn = subMovement.isArmMode;
        }

        UpdateIcon();
    }

    void Update()
    {
        if (subMovement == null) return;

        if (isOn != subMovement.isArmMode)
        {
            isOn = subMovement.isArmMode;
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