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
        if (!File.Exists(path))
        {
            Dictionary<string, List<string>> savingsData = new Dictionary<string, List<string>>
            {
                { Constants.SAVINGS_FILENAME, new List<string> { "", "", "", "", "", "" } },
                { Constants.DATES, new List<string> { "", "", "", "", "", "" } }
            };

            string json = JsonUtility.ToJson(savingsData);
            File.WriteAllText(path, json);
        }
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
            var jsonSavings = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(savingsFile);
            var savingNames = jsonSavings[Constants.SAVINGS_FILENAME];
            var dates = jsonSavings[Constants.DATES];

            for (int i = 0; i < Constants.SAVING_SLOTS_NUMBER; i++)
            {
                if (savingNames[i] != "")
                    result.Add((i, dates[i], savingNames[i]));
            }
        }
        catch (JsonReaderException ex)
        {
            Debug.Log("Incorrect format of JSON song file: " + ex.Message);
        }

        return result;


        //var result = new List<(int id, string date, string name)>();
        //for (int i = 0; i < Constants.SAVING_SLOTS_NUMBER; i++)
        //{
        //    string filePath = GetFilePath(i);

        //    if (File.Exists(filePath))
        //    {
        //        string jsonString = File.ReadAllText(filePath);
        //        State state = JsonUtility.FromJson<State>(jsonString);
        //        result.Add((i, state.Date, state.Name));
        //    }
        //}
        //return result;
    }

    private static void UpdateSavings(int id, string name, string date)
    {
        var path = GetMainFilePath();
        var savingsFile = Resources.Load<TextAsset>(path);

        try
        {
            var jsonSavings = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(savingsFile.text);
            var savingNames = jsonSavings[Constants.SAVINGS_FILENAME];
            var dates = jsonSavings[Constants.DATES];

            savingNames[id] = name;
            dates[id] = date;

            Dictionary<string, List<string>> savingsData = new Dictionary<string, List<string>>
            {
                { Constants.SAVINGS_FILENAME, savingNames },
                { Constants.DATES, dates }
            };
            string json = JsonUtility.ToJson(savingsData);
            File.WriteAllText(path, json);
        }
        catch (JsonReaderException ex)
        {
            Debug.Log("Incorrect format of JSON song file: " + ex.Message);
        }
    }

    private static string GetFilePath(int id)
    {
        return Path + $"/Games/game{id}.json";
    }

    private static string GetMainFilePath()
    {
        return Path + $"/Games/{Constants.SAVINGS_FILENAME}.json";
    }
}
