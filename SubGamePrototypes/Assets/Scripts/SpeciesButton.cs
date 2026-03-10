using UnityEngine;
using UnityEngine.UI;

public class SpeciesButton : MonoBehaviour
{
    Image image;
    Species species; // Reference to the species this button represents
    JournalManager journalManager; // Reference to the JournalManager to check species unlock status
    UIManager uiManager; //Reference UI Manager to open appropriate UI page
    Fish fish; //Reference to Fish script to get fish type;
    Invertebrate invertebrate;

    public Sprite lockedImage; // Image to show when the species is locked
    public Sprite unlockedImage; // Image to show when the species is unlocked

    private void Start()
    {
        image = GetComponent<Image>();
        species = GetComponent<Species>();
        fish = GetComponent<Fish>();
        invertebrate = GetComponent<Invertebrate>();
        uiManager = FindFirstObjectByType<UIManager>();
        if(fish != null) Debug.Log(fish.fishType);
        if (invertebrate != null)  Debug.Log(invertebrate.invertebrateType);

        //Debug.Log(image != null ? "SpeciesButton: Image component found." : "SpeciesButton: Image component NOT found.");

        journalManager = JournalManager.Instance; // Get the singleton instance of JournalManager
        journalManager.onJournalUpdated.AddListener(UpdateButtonAppearance); // Subscribe to journal updates
        Debug.Log(journalManager);
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

    public void OnInvertebrateButtonPress()
    {
        Debug.Log("Invertebrate button pressed" + this);
        Debug.Log(journalManager);
        Debug.Log(journalManager.IsSpeciesIdentified(species));
        bool isUnlocked = journalManager.IsSpeciesIdentified(species);
        if (isUnlocked)
        {
            uiManager.OpenUnlockedInvertebratePage(invertebrate.invertebrateType);
        }
        else
        {
            uiManager.OpenLockedInvertebratePage(invertebrate.invertebrateType);
        }
    }
}
