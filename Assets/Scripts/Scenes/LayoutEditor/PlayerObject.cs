using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerObject : MonoBehaviourPunCallbacks, IPunObservable
{
    public static PlayerObject localPlayerInstance;
    public Renderer playerRenderer;
    public string hexColor = "FFFFFF";

    private void Awake()
    {
        localPlayerInstance = this;
        photonView.ObservedComponents.Add(this);
    }

    private void Start()
    {
        RandomizeColor();
    }

    public void RandomizeColor()
    {
        hexColor = ColorUtility.ToHtmlStringRGB(Random.ColorHSV());
    }

    public void SetColor(string hexColorValue)
    {
        hexColor = hexColorValue;

        Color color = ColorUtility.TryParseHtmlString($"#{hexColor}", out color) ? color : Color.white;

        playerRenderer.material.color = color;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hexColor);
        }
        else
        {
            string newHexColor = (string)stream.ReceiveNext();

            if (newHexColor != hexColor) SetColor(newHexColor);
        }
    }

    public Vector3 GetFrontRaycastPosition()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance = 0;

        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}
