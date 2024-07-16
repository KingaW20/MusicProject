using System;
using UnityEngine;

[Serializable]
public class SongState
{
    [SerializeField] public int CurrentCategoryId;
    [SerializeField] public int CurrentSongId;
    [SerializeField] public int CurrentLineId;
    [SerializeField] public string SongSourcePath;
    [SerializeField] public float CurrentTime;
    [SerializeField] public bool IsAnswered;
    [SerializeField] public bool IsPlaying;

    public SongState()
    {
        CurrentCategoryId = 0;
        CurrentSongId = 0;
        CurrentTime = 0f;
        CurrentLineId = 0;
        SongSourcePath = "";
        IsAnswered = false;
        IsPlaying = false;
    }

    public SongState(SongState ss)
    {
        CurrentCategoryId = ss.CurrentCategoryId;
        CurrentSongId = ss.CurrentSongId;
        CurrentTime = ss.CurrentTime;
        CurrentLineId = ss.CurrentLineId;
        SongSourcePath = ss.SongSourcePath;
        IsAnswered = ss.IsAnswered;
        IsPlaying = ss.IsPlaying;
    }
}
