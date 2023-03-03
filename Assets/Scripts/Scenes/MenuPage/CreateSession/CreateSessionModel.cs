using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class CreateSessionModel : MonoBehaviourPunCallbacks
{
    bool isCreatingSession = false;

    public Coroutine CreateSession(string sessionID, int maxUser)
    {
        return StartCoroutine(CreateSessionAsync(sessionID, maxUser));
    }

    public IEnumerator CreateSessionAsync(string sessionID, int maxUser)
    {
        isCreatingSession = true;

        LayoutManager.layoutID = sessionID;

        PhotonNetwork.JoinOrCreateRoom(
            sessionID,
            new Photon.Realtime.RoomOptions
            {
                MaxPlayers = (byte)maxUser,
                IsOpen = true,
                IsVisible = true
            },
            TypedLobby.Default
        );

        while (isCreatingSession)
        {
            yield return null;
        }

        yield return null;
    }

    public override void OnCreatedRoom()
    {
        isCreatingSession = false;
        Debug.Log("Room created successfully");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        isCreatingSession = false;
        Debug.Log("Room creation failed: " + message);
    }
}
