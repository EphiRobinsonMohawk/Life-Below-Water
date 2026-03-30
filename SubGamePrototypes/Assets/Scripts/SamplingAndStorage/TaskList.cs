using UnityEngine;

public class TaskList : MonoBehaviour
{
    public SampleStorage sampleStorage;
    JournalManager journalManager;

    public SpriteRenderer TunaCheck;
    public SpriteRenderer BaskingShark;
    public SpriteRenderer StarfishCheck;
    public SpriteRenderer TubewormCheck;
    public SpriteRenderer ClamCheck;
    public SpriteRenderer CrabCheck;

    public SpriteRenderer ClamSample1;
    public SpriteRenderer ClamSample2;
    public SpriteRenderer ClamSample3;
    public SpriteRenderer StarfishSample1;
    public SpriteRenderer StarfishSample2;
    public SpriteRenderer StarfishSample3;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        journalManager = JournalManager.Instance;

        TunaCheck.enabled = false;
        BaskingShark.enabled = false;
        StarfishCheck.enabled = false;
        TubewormCheck.enabled = false;
        ClamCheck.enabled = false;
        CrabCheck.enabled = false;
        ClamSample1.enabled = false;
        ClamSample2.enabled = false;
        ClamSample3.enabled = false;
        StarfishSample1.enabled = false;
        StarfishSample2.enabled = false;
        StarfishSample3.enabled = false;
    }

    private void Update()
    {
        UpdateTaskList();
    }

    public void UpdateTaskList()
    {
        if (journalManager.identifiedFish.Contains(FishType.Tuna))
        {
            TunaCheck.enabled = true;
        }
        if (journalManager.identifiedFish.Contains(FishType.BaskingShark))
        {
            BaskingShark.enabled = true;
        }
        if (journalManager.identifiedInvertebrates.Contains(InvertebrateType.Starfish))
        {
            StarfishCheck.enabled = true;
        }
        if (journalManager.identifiedInvertebrates.Contains(InvertebrateType.TubeWorm))
        {
            TubewormCheck.enabled = true;
        }
        if (journalManager.identifiedInvertebrates.Contains(InvertebrateType.Clam))
        {
            ClamCheck.enabled = true;
        }
        if (journalManager.identifiedInvertebrates.Contains(InvertebrateType.Crab))
        {
            CrabCheck.enabled = true;
        }

        if (sampleStorage.GetStoredCount(InvertebrateType.Clam) >= 1)
        {
            ClamSample1.enabled = true;
        }
        if (sampleStorage.GetStoredCount(InvertebrateType.Clam) >= 2)
        {
            ClamSample2.enabled = true;
        }
        if (sampleStorage.GetStoredCount(InvertebrateType.Clam) >= 3)
        {
            ClamSample3.enabled = true;
        }

        if (sampleStorage.GetStoredCount(InvertebrateType.Starfish) >= 1)
        {
            StarfishSample1.enabled = true;
        }
        if (sampleStorage.GetStoredCount(InvertebrateType.Starfish) >= 2)
        {
            StarfishSample2.enabled = true;
        }
        if (sampleStorage.GetStoredCount(InvertebrateType.Starfish) >= 3)
        {
            StarfishSample3.enabled = true;
        }
    }
}
