using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class JoinSessionModel : MonoBehaviourPunCallbacks
{
    bool isJoiningSession = false;

    public Coroutine JoinSession(string sessionID, string pin)
    {
        return StartCoroutine(JoinSessionAsync(sessionID, pin));
    }

    public IEnumerator JoinSessionAsync(string sessionID, string pin)
    {
        isJoiningSession = true;

        LayoutManager.layoutID = sessionID;

        PhotonNetwork.JoinRoom(
            sessionID
        );

        while (isJoiningSession)
        {
            yield return null;
        }

        yield return null;
    }

    public override void OnCreatedRoom()
    {
        isJoiningSession = false;
        Debug.Log("Room created successfully");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        isJoiningSession = false;
        Debug.Log("Room creation failed: " + message);
    }
}
