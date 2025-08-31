using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : SingletonBase<GameStateManager>
{
    public enum GameState
    {
        MainMenu,
        Loading,
        Setting,
        GamePlaying,
        GamePaused,
        GameOver,
        Saving,
        Quitting
    }

    public GameState CurrentState { get; private set; }

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState)
            return;
        CurrentState = newState;
    }
}
