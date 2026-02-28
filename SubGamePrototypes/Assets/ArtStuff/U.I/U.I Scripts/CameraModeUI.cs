using UnityEngine;
using TMPro;

public class CameraModeUI : MonoBehaviour
{
    public TextMeshProUGUI modeText;

    private int currentMode = 1;
    private int maxModes = 3;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentMode++;

            if (currentMode > maxModes)
                currentMode = 1;

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        modeText.text = currentMode.ToString();
    }
}