using System.Collections.Generic;
using UnityEngine;

public class JournalManager : MonoBehaviour
{
    public static JournalManager Instance { get; private set; }

    [Header("Identified Species")]
    public List<FishType> identifiedFish = new List<FishType>();
    public List<InvertebrateType> identifiedInvertebrates = new List<InvertebrateType>();
    public List<PlantType> identifiedPlants = new List<PlantType>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // persist between scenes
    }

    public void RecordSpecies(Species species)
    {
        if (species is Fish fish)
        {
            if (!identifiedFish.Contains(fish.fishType))
            {
                identifiedFish.Add(fish.fishType);
                Debug.Log($"Journal: Added new Fish: {fish.fishType}");
            }
        }
        else if (species is Invertebrate invertebrate)
        {
            if (!identifiedInvertebrates.Contains(invertebrate.invertebrateType))
            {
                identifiedInvertebrates.Add(invertebrate.invertebrateType);
                Debug.Log($"Journal: Added new Invertebrate: {invertebrate.invertebrateType}");
            }
        }
        else if (species is Plant plant)
        {
            if (!identifiedPlants.Contains(plant.plantType))
            {
                identifiedPlants.Add(plant.plantType);
                Debug.Log($"Journal: Added new Plant: {plant.plantType}");
            }
        }
    }

    public bool IsSpeciesIdentified(Species species)
    {
        if (species is Fish fish)
        {
            return identifiedFish.Contains(fish.fishType);
        }
        else if (species is Invertebrate invertebrate)
        {
            return identifiedInvertebrates.Contains(invertebrate.invertebrateType);
        }
        else if (species is Plant plant)
        {
            return identifiedPlants.Contains(plant.plantType);
        }
        return false;
    }
}
