using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerObject : MonoBehaviour
{
    public PhotonView photonView;
    public Renderer playerRenderer;
    public string hexColor = null;

    [PunRPC]
    public void SetColor(string hexColorValue)
    {
        hexColor = hexColorValue;

        Color color = ColorUtility.TryParseHtmlString($"#{hexColor}", out color) ? color : Color.white;

        playerRenderer.material.color = color;
    }

    public void SetColorAcrossNetwork(string hexColorValue)
    {
        hexColor = hexColorValue;
        photonView.RPC("SetColor", RpcTarget.OthersBuffered, hexColorValue);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}
