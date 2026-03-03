using UnityEngine;

public enum SampleType
{
    Species,
    Geological,
}

public class Sample : MonoBehaviour
{
    public SampleType Type = SampleType.Species;

    private void Start()
    {
        tag = "Grabbable";
    }
}
