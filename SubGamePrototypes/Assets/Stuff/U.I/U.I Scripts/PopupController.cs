using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private PopupUI Popup;
    [SerializeField] private PopupUI CPopup;
    [SerializeField] private KeyCode mapKey = KeyCode.M;
    [SerializeField] private KeyCode controllsKey = KeyCode.N;

    void Update()
    {
        if (Input.GetKeyDown(mapKey))
        {
            if (Popup.gameObject.activeSelf)
                Popup.ClosePopup();
            else
                Popup.OpenPopup();
        }

        if (Input.GetKeyDown(controllsKey))
        {
            if (CPopup.gameObject.activeSelf)
                CPopup.ClosePopup();
            else
                CPopup.OpenPopup();
        }
    }
}