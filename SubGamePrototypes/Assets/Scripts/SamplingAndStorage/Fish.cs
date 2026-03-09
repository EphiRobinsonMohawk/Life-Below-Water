using UnityEngine;

public enum FishType
{
    BaskingShark,
    Tuna,
}

public class Fish : Species
{
    public FishType fishType = FishType.BaskingShark;

    void Start()
    {
        Type = SpeciesType.Fish;

        switch (fishType)
        {
            case FishType.BaskingShark:
                speciesName = "Basking Shark";
                break;
            case FishType.Tuna:
                speciesName = "Tuna";
                break;
        }
    }
}
