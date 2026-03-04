using UnityEngine;

public enum FishType
{
    BaskingShark,
    Tuna,
}

public class Fish : Species
{
    public FishType fishType = FishType.BaskingShark;
}
