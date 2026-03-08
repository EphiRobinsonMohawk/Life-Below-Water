using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Checkbook : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameObject checkbookEntryPrefab;
    public GameObject container;

    public void AddCheckbookEntry(int _fundChange, string _reason)
    {
        GameObject newEntry = Instantiate(checkbookEntryPrefab, container.transform);
        CheckbookEntry entryScript = newEntry.GetComponent<CheckbookEntry>();
        entryScript.SetText(_fundChange, _reason);
    }

    
}
