using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

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

    //Events
    //Pass amount of funds changed and the reason
    public UnityEvent<int, string> OnFundsChanged;
    public UnityEvent OnGameOver;
    public UnityEvent OnOvertime;
    //Pass the current amount of time
    public UnityEvent<float> OnTimeChange;
    public UnityEvent OnGameStart;


    public void Start()
    {
        timerStarted = true;
        funds = startingFund;
        timeRemaining = startingTime;
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
        if (!overtime)
        {
            if (timeRemaining <= 0)
            {
                overtime = true;
                OnOvertime.Invoke();
            }
        }
        OnTimeChange.Invoke(timeRemaining);
    }
}
