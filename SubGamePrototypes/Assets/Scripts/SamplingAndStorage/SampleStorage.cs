using UnityEngine;
using UnityEngine.Events;

public class SampleStorage : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<Invertebrate> OnSampleStored = new UnityEvent<Invertebrate>();
    
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
        OnSampleStored.Invoke(sample.GetComponent<Invertebrate>());
        return true;
    }

    public int GetStoredCount(InvertebrateType invertebrateType)
    {
        int count = 0;
        for (int i = 0; i < _storedCount; i++)
        {
            if (sampleStorage[i].GetComponent<Invertebrate>().invertebrateType == invertebrateType)
            {
                count++;
            }
        }
        return count;
    }
}
