using UnityEngine;
using UnityEngine.UI;

public class DpadKeyToggleIcon : MonoBehaviour
{
    public Image iconImage;

    public KeyCode key1 = KeyCode.W;
    public Sprite sprite1;

    public KeyCode key2 = KeyCode.S;
    public Sprite sprite2;

    public KeyCode key3 = KeyCode.A;
    public Sprite sprite3;

    public KeyCode key4 = KeyCode.D;
    public Sprite sprite4;

    void Update()
    {
        if (Input.GetKeyDown(key1))
        {
            SetSprite(sprite1);
        }
        else if (Input.GetKeyDown(key2))
        {
            SetSprite(sprite2);
        }
        else if (Input.GetKeyDown(key3))
        {
            SetSprite(sprite3);
        }
        else if (Input.GetKeyDown(key4))
        {
            SetSprite(sprite4);
        }
    }

    void SetSprite(Sprite newSprite)
    {
        iconImage.sprite = newSprite;
        iconImage.color = new Color(1f, 1f, 1f, 1f);
        transform.localScale = Vector3.one * 1.05f;
    }
}