using UnityEngine;
using UnityEngine.UI;

public class KeyToggleIconV2 : MonoBehaviour
{
    [Header("UI")]
    public Image iconImage;
    public Sprite onSprite;
    public Sprite offSprite;
    public Sprite holdingSprite;
    public Sprite ROVIcon;

    [Header("Sync")]
    public SubMovement subMovement;
    public ArmMovement armMovement;

    private bool isOn = false;
    private bool isHolding = false;

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

        if (armMovement == null)
        {
            armMovement = FindAnyObjectByType<ArmMovement>();
        }

        if (armMovement != null)
        {
            isHolding = armMovement._heldObject != null;
        }

        UpdateIcon();
    }

    private void OnEnable()
    {
        if (subMovement != null)
        {
            subMovement.onArmModeToggled.AddListener(SetArmMode);
        }

        if (armMovement != null)
        {
            armMovement.onHoldingObjectToggled.AddListener(SetHoldingState);
        }
    }

    private void OnDisable()
    {
        if (subMovement != null)
        {
            subMovement.onArmModeToggled.RemoveListener(SetArmMode);
        }

        if (armMovement != null)
        {
            armMovement.onHoldingObjectToggled.RemoveListener(SetHoldingState);
        }
    }

    private void SetArmMode(bool enabled)
    {
        isOn = enabled;
        UpdateIcon();
    }

    private void SetHoldingState(bool holding)
    {
        isHolding = holding;
        UpdateIcon();
    }

    void UpdateIcon()
    {
        if (iconImage == null) return;
        
        if (!isOn)
        {
            iconImage.sprite = offSprite;
            iconImage.color = new Color(1f, 1f, 1f, 0.4f);
            transform.localScale = Vector3.one;
        }
        else if (isHolding)
        {
            iconImage.sprite = holdingSprite;
            iconImage.color = new Color(1f, 1f, 1f, 1f);
            transform.localScale = Vector3.one * 1.05f;
        }
        else
        {
            iconImage.sprite = onSprite;
            iconImage.color = new Color(1f, 1f, 1f, 1f);
            transform.localScale = Vector3.one * 1.05f;
        }
    }
}