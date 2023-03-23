using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class JoinSessionWindow : MonoBehaviour
{
    public JoinSessionWindowView view;
    public JoinSessionWindowModel model;

    private void Awake()
    {
        view.Init(this);
        model.Init(this);
    }

    public void JoinSession()
    {
        StartCoroutine(_JoinSession());
    }

    public IEnumerator _JoinSession()
    {
        view.SetCanvasGroupActive(false);

        bool isRoomDataValid = true;

        string roomID = view.GetRoomID();
        string roomPin = view.GetRoomPin();

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
            DebugApp.Log("Joining session...");
            yield return model.JoinSession(roomID, roomPin);
            DebugApp.Log("Session joined.");
        }

        if (SessionManager.hasError)
        {
            message = SessionManager.errorMessage;
        }

        view.SetErrorText(message);

        view.SetCanvasGroupActive(true);
    }

    [System.Serializable]
    public class JoinSessionWindowView
    {
        JoinSessionWindow controller;

        [Header("UI")]
        public CanvasGroup canvasGroup;
        public TMP_InputField roomIDInput;
        public TMP_InputField roomPinInput;

        public TMP_Text errorText;

        public string GetRoomID() => roomIDInput.text;
        public string GetRoomPin() => roomPinInput.text;

        public void SetErrorText(string text)
        {
            errorText.text = text;
            errorText.gameObject.SetActive(text != "");
        }

        public void SetCanvasGroupActive(bool active)
        {
            canvasGroup.interactable = active;
        }

        public void Init(JoinSessionWindow controller)
        {
            this.controller = controller;
        }
    }

    [System.Serializable]
    public class JoinSessionWindowModel
    {
        JoinSessionWindow controller;
        Coroutine createSessionCoroutine;

        public void Init(JoinSessionWindow controller)
        {
            this.controller = controller;
        }

        public Coroutine JoinSession(string roomID, string roomPin)
        {
            if (createSessionCoroutine != null) controller.StopCoroutine(createSessionCoroutine);
            return createSessionCoroutine = controller.StartCoroutine(_JoinSession(roomID, roomPin));
        }

        IEnumerator _JoinSession(string roomID, string roomPin)
        {
            yield return SessionManager.JoinRoom(roomID, roomPin);
        }
    }
}