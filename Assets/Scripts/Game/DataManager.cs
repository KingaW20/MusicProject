using Scripts;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DataManager
{
    public static string Path = Application.persistentDataPath;

    public static void SaveData(int id, string name)
    {
        var state = new State(name);
        string jsonString = JsonUtility.ToJson(state);
        File.WriteAllText(GetFilePath(id), jsonString);
    }

    public static bool LoadData(int id)
    {
        string filePath = GetFilePath(id);

        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            State state = JsonUtility.FromJson<State>(jsonString);

            GameManager.State = new GameState(state.GameState);
            SongManager.State = new SongState(state.SongState);
            GameManager.State.CurrentMenuContext = MenuContext.LoadContext;
            GameManager.GameLoadedOnSong = GameManager.State.CurrentGameContext == GameContext.SongContext;

            return true;
        }

        return false;
    }

    public static List<(int id, string date, string name)> GetSlotsAvailability()
    {
        var result = new List<(int id, string date, string name)>();
        for (int i = 0; i < Constants.SAVING_SLOTS_NUMBER; i++)
        {
            string filePath = GetFilePath(i);

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                State state = JsonUtility.FromJson<State>(jsonString);
                result.Add((i, state.Date, state.Name));
            }
        }
        return result;
    }

    private static string GetFilePath(int id)
    {
        return Path + $"/game{id}.json";
    }
}
