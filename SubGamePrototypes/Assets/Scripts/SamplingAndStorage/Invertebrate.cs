using UnityEngine;

public enum InvertebrateType
{
    Clam,
    Starfish,
}

public class Invertebrate : Species
{
    public InvertebrateType invertebrateType = InvertebrateType.Clam;
}
