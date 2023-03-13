using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHandler : MonoBehaviourPunCallbacks
{
    public static PlayerHandler instance;

    public GameObject playerPrefab;
    public Transform spawnPoint;

    public static PlayerObject localPlayerObject;

    public PlayerController localPlayerController;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        GameObject playerObject = PhotonNetwork.Instantiate("Photon_Player", spawnPoint.position, spawnPoint.rotation);
        localPlayerObject = playerObject.GetComponent<PlayerObject>();

        localPlayerController.playerObject = localPlayerObject;
    }

    public void OnApplicationQuit()
    {
        LeaveRoom();
    }

    // This method is called when the player wants to leave the room
    public void LeaveRoom()
    {
        // Call the method to destroy the player object
        DestroyPlayer();

        // Call the PhotonNetwork method to leave the room
        PhotonNetwork.LeaveRoom();
    }

    // This method is called to destroy the player object
    public void DestroyPlayer()
    {
        // Check if this is the owner of the player object
        if (localPlayerObject.photonView.IsMine)
        {
            // Call the PhotonNetwork method to destroy the player object on all clients
            PhotonNetwork.Destroy(localPlayerObject.gameObject);
        }
    }
}
