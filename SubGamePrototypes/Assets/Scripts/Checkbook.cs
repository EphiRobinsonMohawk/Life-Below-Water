using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Checkbook : MonoBehaviour
{
    [field: SerializeField]
    public List<string> reasons { get; private set; }
    public List<int> fundChanges { get; private set; }
    public ScoreManager scoreManager;

    public GameObject checkbookEntryPrefab;
    public GameObject container;

    public void UpdateCheckbook()
    {
        for(int i = 0; i < fundChanges.Count; i++)
        {
            GameObject newEntry = Instantiate(checkbookEntryPrefab, container.transform);
            CheckbookEntry entryScript = newEntry.GetComponent<CheckbookEntry>();
            if(entryScript != null )
            {
                entryScript.SetText(fundChanges[i], reasons[i]);
                
            }
        }
        reasons.Clear();
        fundChanges.Clear();
    }

    public void AddCheckbookEntry(int _fundChange, string _reason)
    {
        GameObject newEntry = Instantiate(checkbookEntryPrefab, container.transform);
        CheckbookEntry entryScript = newEntry.GetComponent<CheckbookEntry>();
        entryScript.SetText(_fundChange, _reason);
    }

    public void AddToQueue(int _funds, string _reason)
    {
        reasons.Add(_reason);
        fundChanges.Add(_funds);
    }
}
