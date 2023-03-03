using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour
{
    static CoroutineHandler _instance;

    public static CoroutineHandler instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CoroutineHandler").AddComponent<CoroutineHandler>();
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
