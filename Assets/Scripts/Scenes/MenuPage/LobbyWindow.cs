using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LobbyWindow : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameInputField;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        ConnectToPhotonServer();
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomNameInputField.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomNameInputField.text);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to Photon Server");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("Room Created");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        PhotonNetwork.LoadLevel("LayoutEditor");

        Debug.Log("Joined Room");
    }

    public void ConnectToPhotonServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to Photon Server");
    }

}
