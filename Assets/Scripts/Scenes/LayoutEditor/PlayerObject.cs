using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerObject : MonoBehaviour
{
    public static PlayerObject localPlayerInstance;

    public PhotonView photonView;
    public Renderer playerRenderer;
    public string hexColor = null;

    private void Awake()
    {
        localPlayerInstance = this;
    }

    private void Start()
    {
        RandomizeColor();
    }

    public void RandomizeColor()
    {
        hexColor = ColorUtility.ToHtmlStringRGB(Random.ColorHSV());

        SetColorAcrossNetwork(hexColor);
    }

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
