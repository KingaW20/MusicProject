using System;
using System.IO;
using UnityEngine;

public class Logs : MonoBehaviour
{
    private string logFilePath;

    void OnEnable()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "UnityLog.txt");
        Application.logMessageReceived += LogCallback;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogCallback;
    }

    void LogCallback(string logString, string stackTrace, LogType type)
    {
        string logEntry = $"{DateTime.Now}: {type} - {logString}\n{stackTrace}\n";
        File.AppendAllText(logFilePath, logEntry);
    }
}
