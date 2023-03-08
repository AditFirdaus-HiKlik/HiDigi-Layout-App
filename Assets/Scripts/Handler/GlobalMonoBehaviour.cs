using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMonoBehaviour : MonoBehaviour
{
    static GlobalMonoBehaviour _instance;

    public static GlobalMonoBehaviour instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("GlobalMonoBehaviour").AddComponent<GlobalMonoBehaviour>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
}
