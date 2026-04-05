using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    //Manage score and handle events that change the score. Create a queue of score change events that are displayed later in the UI in chronological fashion.
    //Manage the timer and cause score loss when over timer. 

    //Variables
    public int funds;
    [SerializeField]private int startingFund;
    public float timeRemaining;
    [SerializeField]private float startingTime;
    public bool timerStarted;
    //public Dictionary<int, string> changeQueue;
    public bool gameOver;
    public bool overtime;
    bool overtimeCharge = true;

    //Events
    //Pass amount of funds changed and the reason
    public UnityEvent<int, string> OnFundsChanged;
    public UnityEvent OnGameOver;
    public UnityEvent OnOvertime;
    //Pass the current amount of time
    public UnityEvent<float> OnTimeChange;
    public UnityEvent OnGameStart;

    //References
    public Photography photography;
    public SampleStorage sampleStorage;

    private void OnEnable()
    {
        sampleStorage.OnSampleStored.AddListener(SampleCollected);
        photography.onSpeciesIdentified.AddListener(SpeciesIdentified);
    }

    public void Start()
    {
        timerStarted = true;
        funds = startingFund;
        timeRemaining = startingTime;
    }

    void SpeciesIdentified(Dictionary<Species, bool> identifiedSpecies)
    {
        foreach (var entry in identifiedSpecies.Where(x => x.Value == false))
        {
            string speciesString = entry.Key.speciesName.ToString();

            ChangeFunds(500, "Identified: " + speciesString);
            Debug.Log("Changed funds because identified species: " + speciesString);

            identifiedSpecies[entry.Key] = true;
        }
    }

    void SampleCollected(Invertebrate _sample)
    {
        string sample = _sample.invertebrateType.ToString();
        ChangeFunds(250, "Collected sample of: " +sample);
        Debug.Log("Changed funds because of collected sample:" + sample);
    }

    public void ChangeFunds(int _funds, string _reason)
    {
        funds += _funds;
        //changeQueue.Add(_funds, _reason);
        OnFundsChanged.Invoke(_funds, _reason);
    }

    public void SetGameOver(bool _gameOver)
    {
        gameOver = _gameOver;
        OnGameOver.Invoke();
    }

    public void StartGame()
    {
        timerStarted = true;
    }

    public void Update()
    {
        if (!timerStarted) return;
        LevelTimer();
    }

    public void LevelTimer()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            overtime = true;
            if(overtimeCharge)
            {
                ChangeFunds(-200, "Over expedition time!");
                overtimeCharge = false;
                StartCoroutine(OvertimeFundChange());
            }
             //OnOvertime.Invoke();
        }
        OnTimeChange.Invoke(timeRemaining);
    }

    IEnumerator OvertimeFundChange()
    {
        yield return new WaitForSeconds(30f);
        overtimeCharge = true;
    }
}
