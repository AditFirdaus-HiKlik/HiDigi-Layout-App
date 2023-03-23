using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class MenuScreen : ScreenBehaviour
{
    public void CloseMenu()
    {
        DebugApp.Log("Close Application");
        PopClose();
    }

    public void QuitApplication()
    {
        StartCoroutine(_QuitApplication());
    }

    IEnumerator _QuitApplication()
    {
        DebugApp.Log("Quit Application");

        PlayerHandler.instance.LeaveRoom();

        yield return new WaitForSeconds(1f);

        Application.Quit();
    }
}
