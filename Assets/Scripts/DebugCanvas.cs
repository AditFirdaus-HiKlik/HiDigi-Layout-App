using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugCanvas : MonoBehaviour
{
    public static DebugCanvas instance;

    public bool isDebugScreenActive = true;
    public List<KeyCode> debugShortcut = new List<KeyCode>() { KeyCode.LeftControl, KeyCode.LeftAlt, KeyCode.D };
    public GameObject debugScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetDebugScreenActive(isDebugScreenActive);
        InitializeShortcuts();
    }

    public void Toogle()
    {
        SetDebugScreenActive(isDebugScreenActive = !isDebugScreenActive);
    }

    public void SetDebugScreenActive(bool value)
    {
        debugScreen.SetActive(isDebugScreenActive = value);
    }

    public void InitializeShortcuts()
    {
        ShortcutsManager.CreateShortcut("debug", debugShortcut);
        ShortcutsManager.SetShortcutCallback("debug", (string id) => { Toogle(); });
    }
}
