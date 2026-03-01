using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Canvas journalCanvas;
    public Canvas introductionCanvas;
    public Canvas fishJournalCanvas;
    public Canvas plantlifeCanvas;
    public InputManager inputManager;
    public Canvas activeCanvas;

    public void Start()
    {
        journalCanvas.enabled = false;
        fishJournalCanvas.enabled = false;
        plantlifeCanvas.enabled = false;
        introductionCanvas.enabled = true;
        activeCanvas = introductionCanvas;
    }
    public void ExitCurrentUI()
    {
        activeCanvas.enabled = false;
        inputManager.state = InputManager.InputState.ControlRoom;
    }

    public void  OpenFishJournal()
    {
        activeCanvas.enabled = false;
        fishJournalCanvas.enabled = true;
        activeCanvas = fishJournalCanvas;
    }

    public void OpenPlantlifeJournal()
    {
        activeCanvas.enabled = false;
        plantlifeCanvas.enabled = true;
        activeCanvas = plantlifeCanvas;
    }
    
}
