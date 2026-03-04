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
    }
}
