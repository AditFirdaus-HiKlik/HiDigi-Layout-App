using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHandler : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint;

    public static PlayerObject localPlayerObject;

    public PlayerController localPlayerController;

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
}
