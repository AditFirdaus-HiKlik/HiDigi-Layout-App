using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LayoutSessionManager : MonoBehaviourPunCallbacks
{
    public static string globalSessionID;

    public static void CreateSession(string sessionID)
    {
        string salt = Random.Range(0, 999999).ToString("D6");
        globalSessionID = salt + sessionID;
    }

    public static void JoinSession(string sessionID)
    {
        globalSessionID = sessionID;
    }
}
