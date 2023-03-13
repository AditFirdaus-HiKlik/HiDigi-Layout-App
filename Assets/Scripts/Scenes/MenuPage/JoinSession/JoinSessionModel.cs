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

        // Get lists of rooms
        Debug.Log("Getting room list");
        Debug.Log("Room list: " + PhotonNetwork.CountOfRooms);

        PhotonNetwork.JoinRoom(
            sessionID
        );

        while (isJoiningSession)
        {
            yield return null;
        }

        yield return null;
    }

    public override void OnJoinedRoom()
    {
        isJoiningSession = false;
        Debug.Log("Room joined");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        isJoiningSession = false;
        Debug.Log("Room join failed: " + message);
    }
}
