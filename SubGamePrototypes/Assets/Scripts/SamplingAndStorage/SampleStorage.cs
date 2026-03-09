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
