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
    public Canvas lockedTunaCanvas;
    public Canvas unlockedTunaCanvas;
    public Canvas lockedBaskingSharkCanvas;
    public Canvas unlockedBaskingSharkCanvas;
    public Canvas lockedGiantSunflowerCanvas;
    public Canvas unlockedGiantSunflowerCanvas;
    public Canvas lockedColonialWormCanvas;
    public Canvas unlockedColonialWormCanvas;
    public Canvas lockedVesicomyidClamCanvas;
    public Canvas unlockedVesicomyidClamCanvas;
    public Canvas lockedGracefulCrabCanvas;
    public Canvas unlockedGracefulCrabCanvas;

    // Popups
    public TextMeshProUGUI popUpNotification;

    [Header("First Selection Targets")]
    public GameObject introFirstSelected;
    public GameObject galleryFirstSelected;
    public GameObject fishJournalFirstSelected;
    public GameObject plantlifeFirstSelected;
    public GameObject fundFirstSelected;
    public GameObject settingsFirstSelected;
    public GameObject expeditionFirstSelected;
    public GameObject lockedTunaFirstSelected;
    public GameObject unlockedTunaFirstSelected;
    public GameObject lockedBaskingSharkFirstSelected;
    public GameObject unlockedBaskingSharkFirstSelected;
    public GameObject lockedGiantSunflowerFirstSelected;
    public GameObject lockedColonialWormFirstSelected;
    public GameObject lockedVesicomyidClamFirstSelected;
    public GameObject lockedGracefulCrabFirstSelected;
    public GameObject unlockedGiantSunflowerFirstSelected;
    public GameObject unlockedColonialWormFirstSelected;
    public GameObject unlockedVesicomyidClamFirstSelected;
    public GameObject unlockedGracefulCrabFirstSelected;

    //References
    [Header("Other References")]
    public GalleryDisplay galleryDisplay;
    public Photography photography;
    public InputManager inputManager;
    InputAction cancelAction;
    public AudioManager audioManager;
    public SampleStorage sampleStorage;

    public void Start()
    {
        galleryCanvas.enabled = false;
        fishJournalCanvas.enabled = false;
        plantlifeCanvas.enabled = false;
        fundCanvas.enabled = false;
        settingsCanvas.enabled = false;
        expeditionCanvas.enabled = false;
        lockedTunaCanvas.enabled = false;
        unlockedTunaCanvas.enabled = false;
        lockedBaskingSharkCanvas.enabled = false;
        unlockedBaskingSharkCanvas.enabled = false;
        lockedGiantSunflowerCanvas.enabled = false;
        unlockedGiantSunflowerCanvas.enabled = false;
        lockedColonialWormCanvas.enabled = false;
        unlockedColonialWormCanvas.enabled = false;
        lockedVesicomyidClamCanvas.enabled = false;
        unlockedVesicomyidClamCanvas.enabled = false;
        lockedGracefulCrabCanvas.enabled = false;
        unlockedGracefulCrabCanvas.enabled = false;

    introductionCanvas.enabled = true;
        activeCanvas = introductionCanvas;

        SetSelected(introFirstSelected);
        cancelAction = InputSystem.actions.FindAction("UI/Cancel");

        // Connect event listeners
        photography.onSpeciesIdentified.AddListener(ShowSpeciesPopUp);
        sampleStorage.OnSampleStored.AddListener(ShowSamplePopUp);
        
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

    public void OpenUnlockedFishPage(FishType fish)
    {
        switch (fish)
        {
            case FishType.Tuna:
                activeCanvas.enabled = false;
                unlockedTunaCanvas.enabled = true;
                activeCanvas = unlockedTunaCanvas;
                SetSelected(unlockedTunaFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            case FishType.BaskingShark:
                activeCanvas.enabled = false;
                unlockedBaskingSharkCanvas.enabled = true;
                activeCanvas = unlockedBaskingSharkCanvas;
                SetSelected(unlockedBaskingSharkFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            default:
                Debug.Log("fish type not recognized, UI navigation failed");
                break;
        }
    }

    public void OpenLockedFishPage(FishType fish)
    {
        switch (fish)
        {
            case FishType.Tuna:
                activeCanvas.enabled = false;
                lockedTunaCanvas.enabled = true;
                activeCanvas = lockedTunaCanvas;
                SetSelected(lockedTunaFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            case FishType.BaskingShark:
                activeCanvas.enabled = false;
                lockedBaskingSharkCanvas.enabled = true;
                activeCanvas = lockedBaskingSharkCanvas;
                SetSelected(lockedBaskingSharkFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            default:
                Debug.Log("fish type not recognized, UI navigation failed");
                break;
        }
    }

    public void OpenUnlockedInvertebratePage(InvertebrateType invertebrate)
    {
        switch (invertebrate)
        {
            case InvertebrateType.Crab:
                activeCanvas.enabled = false;
                unlockedGracefulCrabCanvas.enabled = true;
                activeCanvas = unlockedGracefulCrabCanvas;
                SetSelected(unlockedGracefulCrabFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            case InvertebrateType.Clam:
                activeCanvas.enabled = false;
                unlockedVesicomyidClamCanvas.enabled = true;
                activeCanvas = unlockedVesicomyidClamCanvas;
                SetSelected(unlockedVesicomyidClamFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            case InvertebrateType.TubeWorm:
                activeCanvas.enabled = false;
                unlockedColonialWormCanvas.enabled = true;
                activeCanvas = unlockedColonialWormCanvas;
                SetSelected(unlockedColonialWormFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            case InvertebrateType.Starfish:
                activeCanvas.enabled = false;
                unlockedGiantSunflowerCanvas.enabled = true;
                activeCanvas = unlockedGiantSunflowerCanvas;
                SetSelected(unlockedGiantSunflowerFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            default:
                Debug.Log("Invertebrate type not recognized, UI navigation failed");
                break;
        }
    }

    public void OpenLockedInvertebratePage(InvertebrateType invertebrate)
    {
        switch (invertebrate)
        {
            case InvertebrateType.Crab:
                activeCanvas.enabled = false;
                lockedGracefulCrabCanvas.enabled = true;
                activeCanvas = lockedGracefulCrabCanvas;
                SetSelected(lockedGracefulCrabFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            case InvertebrateType.Clam:
                activeCanvas.enabled = false;
                lockedVesicomyidClamCanvas.enabled = true;
                activeCanvas = lockedVesicomyidClamCanvas;
                SetSelected(lockedVesicomyidClamFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            case InvertebrateType.TubeWorm:
                activeCanvas.enabled = false;
                lockedColonialWormCanvas.enabled = true;
                activeCanvas = lockedColonialWormCanvas;
                SetSelected(lockedColonialWormFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            case InvertebrateType.Starfish:
                activeCanvas.enabled = false;
                lockedGiantSunflowerCanvas.enabled = true;
                activeCanvas = lockedGiantSunflowerCanvas;
                SetSelected(lockedGiantSunflowerFirstSelected);
                audioManager.PlayOneShotSFX(audioManager.sfxsData[1]);
                break;
            default:
                Debug.Log("Invertebrate type not recognized, UI navigation failed");
                break;
        }
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
            popUpNotification.gameObject.SetActive(true);
            popUpNotification.text = $"New Species Identified: {string.Join(", ", newSpeciesNames)}";

            CancelInvoke(nameof(HideSpeciesPopUp));
            Invoke(nameof(HideSpeciesPopUp), 5f);
        }
        else if (knownSpeciesNames.Count > 0)
        {
            popUpNotification.gameObject.SetActive(true);
            popUpNotification.text = $"Species Already Recorded: {string.Join(", ", knownSpeciesNames)}";

            CancelInvoke(nameof(HideSpeciesPopUp));
            Invoke(nameof(HideSpeciesPopUp), 5f);
        }
    }

    private void HideSpeciesPopUp()
    {
        popUpNotification.gameObject.SetActive(false);
    }

    private void ShowSamplePopUp(Invertebrate invertebrate)
    {
        popUpNotification.gameObject.SetActive(true);
        popUpNotification.text = $"Sample Stored: {invertebrate.speciesName}";

        CancelInvoke(nameof(HideSamplePopUp));
        Invoke(nameof(HideSamplePopUp), 5f);
    }

    private void HideSamplePopUp()
    {
        popUpNotification.gameObject.SetActive(false);
    }
}
