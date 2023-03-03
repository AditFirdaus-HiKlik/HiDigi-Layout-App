using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : ScreenBehaviour
{
    public void CloseMenu()
    {
        Debug.Log("Close Application");
        PopClose();
    }

    public void QuitApplication()
    {
        Debug.Log("Quit Application");
        PopClose();
        Application.Quit();
    }
}
