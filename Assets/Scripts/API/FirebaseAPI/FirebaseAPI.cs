using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

public class FirebaseAPI : MonoBehaviour
{
    public static FirebaseAPI instance;

    public static FirebaseApp _app;
    public static string appName = Guid.NewGuid().ToString();
    public static FirebaseApp app
    {
        get
        {
            if (_app != null) return _app;

            string _appName = Guid.NewGuid().ToString();

            DebugApp.Log("FirebaseAPI.appName: " + _appName);

            return _app = FirebaseApp.Create(
                FirebaseApp.DefaultInstance.Options,
                appName = _appName
            );
        }
    }
}
