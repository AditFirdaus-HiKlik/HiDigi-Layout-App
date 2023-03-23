using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShortcutsManager : MonoBehaviour
{
    public static ShortcutsManager instance;
    public static Dictionary<string, ShortcutData> shortcuts = new Dictionary<string, ShortcutData>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (var shortcut in shortcuts.Values)
            {
                bool isActive = true;

                foreach (var key in shortcut.keys)
                {
                    if (!Input.GetKey(key))
                    {
                        isActive = false;
                        break;
                    }
                }

                if (isActive)
                {
                    shortcut.Invoke();
                }
            }
        }
    }

    public static void CreateShortcut(string id, List<KeyCode> keys)
    {
        shortcuts.Add(id, new ShortcutData()
        {
            id = id,
            keys = keys,
        });
    }

    public static void SetShortcutCallback(string id, UnityAction<string> callback)
    {
        if (shortcuts.ContainsKey(id))
        {
            shortcuts[id].onShortcut.AddListener(callback);
        }
    }
}

public class ShortcutData
{
    public string id = "";
    public List<KeyCode> keys = new List<KeyCode>();
    public UnityEvent<string> onShortcut = new UnityEvent<string>();

    public void Invoke()
    {
        onShortcut.Invoke(id);
    }
}