using UnityEngine;

public enum PlantType
{

}

public class Plant : Species
{
    public PlantType fishType;

    void Start()
    {
        Type = SpeciesType.Plant;
    }
}