using UnityEngine;
using UnityEngine.UI;

public class SpeciesButton : MonoBehaviour
{
    Image image;
    Species species; // Reference to the species this button represents
    JournalManager journalManager; // Reference to the JournalManager to check species unlock status
    UIManager uiManager; //Reference UI Manager to open appropriate UI page
    Fish fish; //Reference to Fish script to get fish type;

    public Sprite lockedImage; // Image to show when the species is locked
    public Sprite unlockedImage; // Image to show when the species is unlocked

    private void Start()
    {
        image = GetComponent<Image>();
        species = GetComponent<Species>();
        fish = GetComponent<Fish>();
        uiManager = FindFirstObjectByType<UIManager>();
        Debug.Log(fish.fishType);

        //Debug.Log(image != null ? "SpeciesButton: Image component found." : "SpeciesButton: Image component NOT found.");

        journalManager = JournalManager.Instance; // Get the singleton instance of JournalManager
        journalManager.onJournalUpdated.AddListener(UpdateButtonAppearance); // Subscribe to journal updates
    }

    // Check if the species is unlocked and update the button's appearance accordingly
    public void UpdateButtonAppearance()
    {
        bool isUnlocked = journalManager.IsSpeciesIdentified(species);

        if (isUnlocked)
        {
            image.sprite = unlockedImage;
        }
        else
        {
            image.sprite = lockedImage;
        }
    }

    //When button is pressed open the unlocked or locked page respectively
    public void OnFishButtonPress()
    {
        Debug.Log("Fish button pressed");
        bool isUnlocked = journalManager.IsSpeciesIdentified(species);
        if (isUnlocked)
        {
            uiManager.OpenUnlockedFishPage(fish.fishType);
        }
        else
        {
            uiManager.OpenLockedFishPage(fish.fishType);
        }
    }
}
