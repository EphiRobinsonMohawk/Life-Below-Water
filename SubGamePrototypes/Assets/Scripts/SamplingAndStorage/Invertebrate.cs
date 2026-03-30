using Unity.VisualScripting;
using UnityEngine;

public enum InvertebrateType
{
    Clam,
    Starfish,
    Crab,
    TubeWorm,
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
                speciesName = "Vesicomyid Clam";
                break;
            case InvertebrateType.Starfish:
                speciesName = "Giant Sunflower Star";
                break;
            case InvertebrateType.Crab:
                speciesName = "Graceful Decorator Crab";
                break;
            case InvertebrateType.TubeWorm:
                speciesName = "Colonial Tube Worm";
                break;
        }
    }
}
