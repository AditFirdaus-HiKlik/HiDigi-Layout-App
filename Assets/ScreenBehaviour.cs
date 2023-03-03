using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenBehaviour : MonoBehaviour
{
    public bool isOpen = true;
    public Dictionary<string, object> screenData = new Dictionary<string, object>();
    public UnityAction<Dictionary<string, object>> screenCallback = new UnityAction<Dictionary<string, object>>(data => { });

    [HideInInspector]
    public ScreenManager screenManager;

    public void PopClose()
    {
        screenManager.CloseScreen();
    }

    public void OnClose()
    {
        isOpen = false;
        if (screenCallback != null) screenCallback.Invoke(screenData);
    }

    public Coroutine WaitForClose()
    {
        return StartCoroutine(WaitForCloseCoroutine());
    }

    public ScreenBehaviour SetInitialData(Dictionary<string, object> data)
    {
        this.screenData = data;
        return this;
    }

    public ScreenBehaviour SetCallback(UnityAction<Dictionary<string, object>> onReturnData)
    {
        this.screenCallback = onReturnData;
        return this;
    }

    IEnumerator WaitForCloseCoroutine()
    {
        while (isOpen) yield return null;
    }
}
