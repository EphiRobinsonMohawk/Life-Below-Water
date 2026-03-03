using UnityEngine;

public class PopupUI : MonoBehaviour
{
    public void OpenPopup()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
