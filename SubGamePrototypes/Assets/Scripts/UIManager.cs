using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public Canvas journalCanvas;
    public Canvas introductionCanvas;
    public Canvas fishJournalCanvas;
    public Canvas plantlifeCanvas;
    public Canvas fundCanvas;
    public Checkbook checkbookScript;
    public InputManager inputManager;
    public Canvas activeCanvas;
    public ScoreManager scoreManager;


    [Header("First Selection Targets")]
    public GameObject introFirstSelected;
    public GameObject journalFirstSelected;
    public GameObject fishJournalFirstSelected;
    public GameObject plantlifeFirstSelected;
    public GameObject fundFirstSelected;

    InputAction cancelAction;

    public void Start()
    {
        journalCanvas.enabled = false;
        fishJournalCanvas.enabled = false;
        plantlifeCanvas.enabled = false;
        fundCanvas.enabled = false;
        introductionCanvas.enabled = true;
        activeCanvas = introductionCanvas;

        SetSelected(introFirstSelected);
        cancelAction = InputSystem.actions.FindAction("UI/Cancel");
    }

    void Update()
    {
        if (inputManager.state == InputManager.InputState.Menus)
        {
            if (cancelAction.WasPressedThisFrame())
            {
                ExitCurrentUI();
            }
        }
    }

    //Timer UI



    //UI Open and Close functions
    public void ExitCurrentUI()
    {
        if (activeCanvas != null) activeCanvas.enabled = false;
        inputManager.state = InputManager.InputState.ControlRoom;
    }

    public void OpenFishJournal()
    {
        activeCanvas.enabled = false;
        fishJournalCanvas.enabled = true;
        activeCanvas = fishJournalCanvas;
        SetSelected(fishJournalFirstSelected);
    }

    public void OpenPlantlifeJournal()
    {
        activeCanvas.enabled = false;
        plantlifeCanvas.enabled = true;
        activeCanvas = plantlifeCanvas;
        SetSelected(plantlifeFirstSelected);
    }

    public void OpenFundJournal()
    {
        activeCanvas.enabled = false;
        fundCanvas.enabled = true;
        activeCanvas = fundCanvas;
        checkbookScript.UpdateCheckbook();
        SetSelected(fundFirstSelected);
    }

    public void OpenJournal()
    {
        if (activeCanvas != null) activeCanvas.enabled = false;
        journalCanvas.enabled = true;
        activeCanvas = journalCanvas;
        SetSelected(journalFirstSelected);
    }

    private void SetSelected(GameObject target)
    {
        if (target == null) return;
        EventSystem.current.SetSelectedGameObject(target);
    }
    
}
