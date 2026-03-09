using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    // Main UI Canvases
    [Header("Canvas References")]
    public Canvas galleryCanvas;
    public Canvas introductionCanvas;
    public Canvas fishJournalCanvas;
    public Canvas plantlifeCanvas;
    public Canvas activeCanvas;
    public Canvas fundCanvas;
    public Canvas expeditionCanvas;
    public Canvas settingsCanvas;

    // Popups
    public TextMeshProUGUI speciesPopUp;

    [Header("First Selection Targets")]
    public GameObject introFirstSelected;
    public GameObject galleryFirstSelected;
    public GameObject fishJournalFirstSelected;
    public GameObject plantlifeFirstSelected;
    public GameObject fundFirstSelected;
    public GameObject settingsFirstSelected;
    public GameObject expeditionFirstSelected;

    //References
    [Header("Other References")]
    public GalleryDisplay galleryDisplay;
    public Photography photography;
    public InputManager inputManager;
    InputAction cancelAction;
    public AudioManager audioManager;

    public void Start()
    {
        galleryCanvas.enabled = false;
        fishJournalCanvas.enabled = false;
        plantlifeCanvas.enabled = false;
        fundCanvas.enabled = false;
        settingsCanvas.enabled = false;
        expeditionCanvas.enabled = false;

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

    public void OpenCheckbook()
    {
        activeCanvas.enabled = false;
        fundCanvas.enabled = true;
        activeCanvas = fundCanvas;
        SetSelected(fundFirstSelected);
        audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
    }

    public void OpenSettings()
    {
        activeCanvas.enabled = false;
        settingsCanvas.enabled = true;
        activeCanvas = settingsCanvas;
        SetSelected(settingsFirstSelected);
        audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
    }

    public void OpenExpeditions()
    {
        activeCanvas.enabled = false;
        expeditionCanvas.enabled = true;
        activeCanvas = expeditionCanvas;
        SetSelected(expeditionFirstSelected);
        audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
    }
    public void OpenFishJournal()
    {
        activeCanvas.enabled = false;
        fishJournalCanvas.enabled = true;
        activeCanvas = fishJournalCanvas;
        SetSelected(fishJournalFirstSelected);
        audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
    }

    public void OpenFishPage(FishType fish)
    {

    }

    public void OpenPlantlifeJournal()
    {
        activeCanvas.enabled = false;
        plantlifeCanvas.enabled = true;
        activeCanvas = plantlifeCanvas;
        SetSelected(plantlifeFirstSelected);
        audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
    }

    public void OpenGallery()
    {
        if (activeCanvas != null) activeCanvas.enabled = false;
        galleryCanvas.enabled = true;
        activeCanvas = galleryCanvas;
        SetSelected(galleryFirstSelected);
        audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
        galleryDisplay.CloseFullscreen();
        galleryDisplay.LoadPendingPhotos();
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
