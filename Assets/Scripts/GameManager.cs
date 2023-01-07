using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    GameState state;
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
        switch (newState)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.Farming:
                break;
            case GameState.Destroying:
                break;
            case GameState.RoundTransition:
                break;
        }
    }

    private void HandleStarting()
    {
        return;
    }
}

public enum GameState
{
    Starting,
    PlayerTurn,  // Player input happens here
    Farming,
    Destroying,
    RoundTransition,  // FarmValue calculations, animations done here
}
