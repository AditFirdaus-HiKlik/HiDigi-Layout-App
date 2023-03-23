using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class SessionManager : MonoBehaviourPunCallbacks
{
    public static SessionManager instance;

    public static string roomSessionPin = "";
    public static string errorMessage = "";
    public static bool hasError = false;

    public static UnityEvent<bool> OnRoomCallback = new UnityEvent<bool>();

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

    public static Coroutine CreateRoom(string roomID, string roomPin, int roomSize = 0)
    {
        return instance.StartCoroutine(_CreateRoom(roomID, roomPin, roomSize));
    }

    public static Coroutine JoinRoom(string roomID, string roomPin)
    {
        return instance.StartCoroutine(_JoinRoom(roomID, roomPin));
    }

    static IEnumerator _CreateRoom(string roomID, string roomPin, int roomSize = 0)
    {
        roomSessionPin = roomPin;

        hasError = false;

        PhotonNetwork.CreateRoom(
            roomID,
            new Photon.Realtime.RoomOptions
            {
                MaxPlayers = (byte)roomSize,
                IsOpen = true,
                IsVisible = true,
                CleanupCacheOnLeave = false,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
                {
                    { "pin", roomPin }
                },
            },
            Photon.Realtime.TypedLobby.Default
        );

        yield return new WaitUntil(() => PhotonNetwork.InRoom || hasError);

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LoadLevel("LayoutCloudEditor");
        }

        OnRoomCallback.Invoke(!hasError);

        yield return null;
    }

    static IEnumerator _JoinRoom(string roomID, string roomPin)
    {
        roomSessionPin = roomPin;

        DebugApp.Log("Starting _JoinRoom");
        hasError = false;

        PhotonNetwork.JoinRoom(roomID);

        DebugApp.Log("Waiting for PhotonNetwork.InRoom");
        yield return new WaitUntil(() => PhotonNetwork.InRoom || hasError);

        OnRoomCallback.Invoke(!hasError);

        yield return null;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        hasError = true;
        SessionManager.errorMessage = message;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        hasError = true;
        SessionManager.errorMessage = message;
    }
}
