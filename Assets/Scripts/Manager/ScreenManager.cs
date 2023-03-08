using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;

    public Transform screenParent;
    public Stack<ScreenBehaviour> screenStack = new Stack<ScreenBehaviour>();

    private void Awake()
    {
        instance = this;
    }

    public void CloseAllScreens()
    {
        while (screenStack.Count > 0) CloseScreen();
    }

    public void OpenScreenVoidRaw(GameObject screenPrefab)
    {
        CreateScreen(screenPrefab, null, null);
    }

    public void OpenScreenVoid(GameObject screenPrefab, Dictionary<string, object> data = null, UnityAction<Dictionary<string, object>> callback = null)
    {
        CreateScreen(screenPrefab, data, callback);
    }

    public ScreenBehaviour OpenScreenReturn(GameObject screenPrefab, Dictionary<string, object> data = null, UnityAction<Dictionary<string, object>> callback = null)
    {
        ScreenBehaviour screenBehaviour = CreateScreen(screenPrefab, data, callback);

        return screenBehaviour;
    }

    public void CloseScreen()
    {
        if (screenStack.Count > 0)
        {
            ScreenBehaviour screenBehaviour = screenStack.Pop();

            screenBehaviour.OnClose();

            Debug.Log($"Screen closed: {screenBehaviour.name} | With data: {screenBehaviour.screenData.DictionaryToJson()}");

            Destroy(screenBehaviour.gameObject);
        }
    }

    public ScreenBehaviour CreateScreen(GameObject screenPrefab, Dictionary<string, object> data = null, UnityAction<Dictionary<string, object>> callback = null)
    {
        GameObject screenObject = Instantiate(screenPrefab, screenParent);
        ScreenBehaviour screenBehaviour = screenObject.GetComponent<ScreenBehaviour>();

        screenBehaviour.screenManager = this;

        if (data != null) screenBehaviour.SetInitialData(data);
        if (callback != null) screenBehaviour.SetCallback(callback);

        Debug.Log($"Screen opened: {screenBehaviour.name} | With data: {screenBehaviour.screenData.DictionaryToJson()}");

        screenStack.Push(screenBehaviour);
        return screenBehaviour;
    }
}
