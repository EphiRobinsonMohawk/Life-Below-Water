using UnityEngine;

public class SubHUD : MonoBehaviour
{
    public SubMovement subMovement;
    public GameObject subHUD;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        subMovement.onEnterHerculesFirstPersonView.AddListener(ShowHUD);
        subMovement.onExitHerculesFirstPersonView.AddListener(HideHUD);
        HideHUD();
    }

    public void ShowHUD()
    {
        subHUD.SetActive(true);
    }

    public void HideHUD()
    {
        subHUD.SetActive(false);
    }

}
