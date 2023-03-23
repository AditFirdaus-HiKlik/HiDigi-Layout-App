using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DebugConsole : MonoBehaviour
{
    public string logHistory => DebugApp.logHistory;

    public TMP_Text logText;

    private void Start()
    {
        DebugApp.OnLog.AddListener(OnLog);
    }

    void OnEnable()
    {
        Refresh();
    }

    private void OnLog(string message)
    {
        Refresh();
    }

    public void Refresh()
    {
        logText.text = logHistory;
    }
}
