using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;

public class PhotonModule : MonoBehaviourPunCallbacks
{
    public static PhotonModule Instance;

    private void Awake()
    {
        InitSingleton();

        PhotonNetwork.AutomaticallySyncScene = true;

        ConnectToPhotonServer();
    }

    private void OnApplicationQuit()
    {
        DisconnectFromPhotonServer();
    }

    void InitSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void ConnectToPhotonServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        DebugApp.Log("Connecting to Photon Server");
    }

    public void DisconnectFromPhotonServer(UnityAction callback = null)
    {
        PhotonNetwork.Disconnect();
        DebugApp.Log("Disconnecting from Photon Server");
    }
}
