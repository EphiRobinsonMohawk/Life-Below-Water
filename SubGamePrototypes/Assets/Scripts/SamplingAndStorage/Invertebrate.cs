using Unity.VisualScripting;
using UnityEngine;

public enum InvertebrateType
{
    Clam,
    Starfish,
    Crab,
}

public class Invertebrate : Species
{
    public InvertebrateType invertebrateType = InvertebrateType.Clam;

    void Start()
    {
        Type = SpeciesType.Invertebrate;

        switch (invertebrateType)
        {
            case InvertebrateType.Clam:
                speciesName = "Clam";
                break;
            case InvertebrateType.Starfish:
                speciesName = "Starfish";
                break;
            case InvertebrateType.Crab:
                speciesName = "Crab";
                break;
        }
    }
}
