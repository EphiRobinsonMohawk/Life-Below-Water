using TMPro;
using UnityEngine;

public class ExpeditionTimer : MonoBehaviour
{
    public TextMeshPro timerText;

    public void UpdateTimerText(float floatSeconds)
    {
        int totalSeconds = (int)floatSeconds;
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        string time = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = time;
    }

}
