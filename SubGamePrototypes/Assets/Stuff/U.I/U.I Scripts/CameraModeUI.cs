using UnityEngine;
using TMPro;

public class CameraModeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI modeText;

    void Awake()
    {
        if (modeText == null)
        {
            Debug.LogWarning("ModeText is not assigned in CameraModeUI.");
        }
    }

    public void SetMode(int mode)
    {
        if (modeText != null)
        {
            modeText.text = mode.ToString();
            StopAllCoroutines();
            StartCoroutine(PopEffect());
        }
    }

    private System.Collections.IEnumerator PopEffect()
    {
        modeText.transform.localScale = Vector3.one * 1.2f;
        yield return new WaitForSeconds(0.1f);
        modeText.transform.localScale = Vector3.one;
    }
}