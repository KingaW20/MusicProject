using Newtonsoft.Json;
using Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DataManager
{
    public static string Path = Application.persistentDataPath;

    public static void CreateMainFile()
    {
        var path = GetMainFilePath();        
    }

    public static void SaveData(int id, string name)
    {
        try
        {
            var state = new State(name);
            string jsonString = JsonUtility.ToJson(state);
            File.WriteAllText(GetFilePath(id), jsonString);
            UpdateSavings(id, name, state.Date);
        }
        catch (Exception ex)
        {
            Debug.Log($"Source: {ex.Source}\nMessage: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }
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
            if (GameManager.State.CurrentGameContext == GameContext.SongContext)
                SongManager.GetCurrentSong();
            GameManager.State.CurrentMenuContext = MenuContext.LoadContext;
            GameManager.GameLoadedOnSong = GameManager.State.CurrentGameContext == GameContext.SongContext;

            return true;
        }

        return false;
    }

    public static List<(int id, string date, string name)> GetSlotsAvailability()
    {
        var path = GetMainFilePath();
        var savingsFile = File.ReadAllText(path);
        var result = new List<(int id, string date, string name)>();

        try
        {
            var jsonSavings = JsonUtility.FromJson<Savings>(savingsFile);
            for (int i = 0; i < Constants.SAVING_SLOTS_NUMBER; i++)
            {
                if (jsonSavings.Names[i] != "")
                    result.Add((i, jsonSavings.Dates[i], jsonSavings.Names[i]));
            }
        }
        catch (JsonReaderException ex)
        {
            Debug.Log("Incorrect format of JSON song file: " + ex.Message);
        }

        return result;
    }

    private static void UpdateSavings(int id, string name, string date)
    {
        var path = GetMainFilePath();
        var savingsFile = File.ReadAllText(path);

        try
        {
            var jsonSavings = JsonUtility.FromJson<Savings>(savingsFile);
            jsonSavings.Names[id] = name;
            jsonSavings.Dates[id] = date;
            string json = JsonUtility.ToJson(jsonSavings);
            File.WriteAllText(path, json);
        }
        catch (JsonReaderException ex)
        {
            Debug.Log("Incorrect format of JSON song file: " + ex.Message);
        }
    }

    private static string GetFilePath(int id)
    {
        if (!Directory.Exists(Path + "/Games"))
            Directory.CreateDirectory(Path + "/Games");
        return Path + $"/Games/game{id}.json";
    }

    private static string GetMainFilePath()
    {
        if (!Directory.Exists(Path + "/Games"))
            Directory.CreateDirectory(Path + "/Games");

        var path = Path + $"/Games/{Constants.SAVINGS_FILENAME}.json";
        if (!File.Exists(path))
        {
            var savings = new Savings();
            string json = JsonUtility.ToJson(savings);
            File.WriteAllText(path, json);
        }

        return path;
    }
}
