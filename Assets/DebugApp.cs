using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugApp : MonoBehaviour
{
    public static UnityEvent<string> OnLog = new UnityEvent<string>();

    public static string logHistory = "";
    public static int logNumber = 0;

    public static void Log(string message)
    {
        logHistory += $"{logNumber} | {message} \n\n";

        logNumber++;

        Debug.Log(message);

        OnLog.Invoke(message);
    }

    // Open Log File
    public static void OpenLogFile()
    {
        string logPath = Application.persistentDataPath + "/log.txt";

        Debug.Log(logPath);

        Application.OpenURL(logPath);
    }
}
