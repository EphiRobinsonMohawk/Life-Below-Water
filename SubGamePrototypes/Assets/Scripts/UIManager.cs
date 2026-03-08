using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    // Main UI Canvases
    [Header("Canvas References")]
    public Canvas journalCanvas;
    public Canvas introductionCanvas;
    public Canvas fishJournalCanvas;
    public Canvas plantlifeCanvas;
    public Canvas activeCanvas;

    // Popups
    public TextMeshProUGUI speciesPopUp;

    [Header("First Selection Targets")]
    public GameObject introFirstSelected;
    public GameObject journalFirstSelected;
    public GameObject fishJournalFirstSelected;
    public GameObject plantlifeFirstSelected;

    //References
    [Header("Other References")]
    public Photography photography;
    public InputManager inputManager;
    InputAction cancelAction;

    public void Start()
    {
        journalCanvas.enabled = false;
        fishJournalCanvas.enabled = false;
        plantlifeCanvas.enabled = false;
        introductionCanvas.enabled = true;
        activeCanvas = introductionCanvas;

        SetSelected(introFirstSelected);
        cancelAction = InputSystem.actions.FindAction("UI/Cancel");

        // Connect event listeners
        photography.onSpeciesIdentified.AddListener(ShowSpeciesPopUp);
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

    private void ShowSpeciesPopUp(Dictionary<Species, bool> species)
    {
        HashSet<string> newSpeciesNames = new HashSet<string>(); //HashSet is a like a List but it can't have duplicates
        HashSet<string> knownSpeciesNames = new HashSet<string>();

        foreach (var entry in species)
        {
            if (!entry.Value) // entry.Value is hasBeenRecorded (previous state)
            {
                newSpeciesNames.Add(entry.Key.speciesName);
            }
            else
            {
                knownSpeciesNames.Add(entry.Key.speciesName);
            }
        }

        if (newSpeciesNames.Count > 0)
        {
            speciesPopUp.gameObject.SetActive(true);
            speciesPopUp.text = $"New Species Identified: {string.Join(", ", newSpeciesNames)}";

            CancelInvoke(nameof(HideSpeciesPopUp));
            Invoke(nameof(HideSpeciesPopUp), 5f);
        }
        else if (knownSpeciesNames.Count > 0)
        {
            speciesPopUp.gameObject.SetActive(true);
            speciesPopUp.text = $"Species Already Recorded: {string.Join(", ", knownSpeciesNames)}";

            CancelInvoke(nameof(HideSpeciesPopUp));
            Invoke(nameof(HideSpeciesPopUp), 5f);
        }
    }

    private void HideSpeciesPopUp()
    {
        speciesPopUp.gameObject.SetActive(false);
    }
}
