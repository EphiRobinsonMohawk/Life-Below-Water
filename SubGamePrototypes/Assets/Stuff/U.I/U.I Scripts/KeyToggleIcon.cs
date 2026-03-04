using UnityEngine;
using UnityEngine.UI;

public class KeyToggleIcon : MonoBehaviour
{
    public Image iconImage;

    public Sprite onSprite;
    public Sprite offSprite;

    public KeyCode toggleKey = KeyCode.F;

    private bool isOn = false;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
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