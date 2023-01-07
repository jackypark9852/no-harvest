using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public UnityEvent Starting;
    public UnityEvent PlayerTurn;
    public UnityEvent Farming;
    public UnityEvent Destroying;
    public UnityEvent RoundTransition;
    public UnityEvent GameOver;
    public UnityEvent StateChanged; 

    FarmerActionInfo farmerActionInfo;
    PlayerActionInfo playerActionInfo;

    public GameState state = GameState.NotStarted;
    public GameState State
    {
        get { return state; }
    }

    void Start()
    {
        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState)
    {
        if (newState == state)
        {
            return;
        }

        state = newState;
        HandleStateChanged();
        switch (newState)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.PlayerTurn:
                HandlePlayerTurn();
                break;
            case GameState.Destroying:
                HandleDestroying();
                break;
            case GameState.Farming:
                HandleFarming();
                break;
            case GameState.RoundTransition:
                HandleRoundTransition(); 
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
        }
    }

    private void HandleStarting()
    {
        Debug.Log("Starting");
        Starting.Invoke();
        ChangeState(GameState.PlayerTurn); 
        return;
    }
    private void HandlePlayerTurn()
    {
        PlayerTurn.Invoke();
        return;
    }
    private void HandleFarming()
    {
        Farming.Invoke();
        return;
    }
    private void HandleDestroying()
    {
        Destroying.Invoke();
        return;
    }
    private void HandleRoundTransition()
    {
        RoundTransition.Invoke();
        return;
    }
    private void HandleGameOver()
    {
        GameOver.Invoke();
        return;
    }
    private void HandleStateChanged()
    {
        StateChanged.Invoke();
        return;
    }
    public void EndPlayerTurn()
    {
        ChangeState(GameState.Destroying);
    }
    public void SetFarmerActionInfo(FarmerActionInfo farmerActionInfo)
    {
        this.farmerActionInfo = farmerActionInfo;
    }

}

public enum GameState
{
    NotStarted, 
    Starting,
    PlayerTurn,  // Player input happens here
    Destroying,
    Farming,
    RoundTransition,  // FarmValue calculations, animations done here
    GameOver, 
}
