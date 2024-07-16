using Scripts;
using System;
using UnityEngine;

[Serializable]
public class State
{
    [SerializeField] public string Name;
    [SerializeField] public string Date;
    [SerializeField] public GameState GameState;
    [SerializeField] public SongState SongState;

    public State(string name)
    {
        Name = name;
        Date = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy");
        GameState = new GameState(GameManager.State);
        SongState = new SongState(SongManager.State);
    }
}
