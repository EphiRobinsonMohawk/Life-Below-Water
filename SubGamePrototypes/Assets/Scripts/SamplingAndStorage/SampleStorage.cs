using UnityEngine;

public class SampleStorage : MonoBehaviour
{
    public Sample[] sampleStorage = new Sample[10];
    private int _storedCount = 0;

    public bool TryStoreSample(Sample sample)
    {
        if (_storedCount >= sampleStorage.Length)
        {
            Debug.LogWarning("Storage is full!");
            return false;
        }

        sampleStorage[_storedCount] = sample;
        _storedCount++;
        Debug.Log($"Stored sample: {sample.name}. Total: {_storedCount}");
        return true;
    }
}
