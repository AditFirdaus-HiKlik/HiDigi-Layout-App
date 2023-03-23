using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CreateSessionWindow : MonoBehaviour
{
    public CreateSessionWindowView view;
    public CreateSessionWindowModel model;

    private void Awake()
    {
        view.Init(this);
        model.Init(this);
    }

    public void CreateSession()
    {
        StartCoroutine(_CreateSession());
    }

    public IEnumerator _CreateSession()
    {
        view.SetCanvasGroupActive(false);

        bool isRoomDataValid = true;

        string roomID = view.GetRoomID();
        string roomPin = view.GetRoomPin();
        int roomSize = view.GetRoomSize();

        string message = "";

        if (roomID == "")
        {
            message = "Room ID cannot be empty";
            isRoomDataValid = false;
        }
        // else if (roomPin == "")
        // {
        //     message = "Room PIN cannot be empty";
        //     isRoomDataValid = false;
        // }

        if (isRoomDataValid)
        {
            LayoutManager.layoutID = roomID;
            yield return model.CreateSession(roomID, roomPin, roomSize);
        }

        if (SessionManager.hasError)
        {
            message = SessionManager.errorMessage;
        }

        view.SetErrorText(message);

        view.SetCanvasGroupActive(true);
    }
}

[System.Serializable]
public class CreateSessionWindowView
{
    CreateSessionWindow controller;

    [Header("UI")]
    public CanvasGroup canvasGroup;
    public TMP_InputField roomIDInput;
    public TMP_InputField roomPinInput;
    public TMP_InputField roomSizeInput;

    public TMP_Text errorText;

    public string GetRoomID() => roomIDInput.text;
    public string GetRoomPin() => roomPinInput.text;
    public int GetRoomSize() => int.Parse(roomSizeInput.text, 0);

    public void SetErrorText(string text)
    {
        errorText.text = text;
        errorText.gameObject.SetActive(text != "");
    }

    public void SetCanvasGroupActive(bool active)
    {
        canvasGroup.interactable = active;
    }

    public void Init(CreateSessionWindow controller)
    {
        this.controller = controller;
    }
}

[System.Serializable]
public class CreateSessionWindowModel
{
    CreateSessionWindow controller;
    Coroutine createSessionCoroutine;

    public void Init(CreateSessionWindow controller)
    {
        this.controller = controller;
    }

    public Coroutine CreateSession(string roomID, string roomPin, int roomSize)
    {
        if (createSessionCoroutine != null) controller.StopCoroutine(createSessionCoroutine);
        return createSessionCoroutine = controller.StartCoroutine(_CreateSession(roomID, roomPin, roomSize));
    }

    IEnumerator _CreateSession(string roomID, string roomPin, int roomSize)
    {
        yield return SessionManager.CreateRoom(roomID, roomPin, roomSize);
    }
}