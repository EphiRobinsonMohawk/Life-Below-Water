using UnityEngine;

public enum PlantType
{

}

public class Plant : Species
{
    public PlantType plantType;

    void Start()
    {
        Type = SpeciesType.Plant;
    }
}