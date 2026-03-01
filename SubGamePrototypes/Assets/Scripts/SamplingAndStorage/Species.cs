using UnityEngine;

public enum SpeciesType
{
    Fish,
    Plant,
}

public class Species : MonoBehaviour
{
    public SpeciesType Type = SpeciesType.Plant;
    public bool isSampleable = true;
    public bool hasBeenRecorded = false;

    private void Start()
    {
        if (isSampleable)
        {
            // Add a sample component to this species so it can be collected
            Sample sample = gameObject.AddComponent<Sample>();
            sample.Type = SampleType.Species;
        }
    }
}
