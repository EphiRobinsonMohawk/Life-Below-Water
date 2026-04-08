using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupBoarderFollower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetText;

    private Image borderImage;

    private void Awake()
    {
        borderImage = GetComponent<Image>();
    }

    private void Start()
    {
        if (borderImage != null)
            borderImage.enabled = false;
    }

    private void Update()
    {
        if (targetText == null || borderImage == null) return;

        bool shouldShow =
            targetText.gameObject.activeSelf &&
            !string.IsNullOrWhiteSpace(targetText.text);

        borderImage.enabled = shouldShow;
    }
}