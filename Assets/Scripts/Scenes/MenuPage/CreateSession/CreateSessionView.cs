using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class CreateSessionView : MonoBehaviour
{
    public TMP_InputField id_InputField;
    public TMP_InputField pin_InputField;
    public TMP_InputField maxUser_InputField;

    public Button createSession_Button;
    public Image loading_Image;

    private void Start()
    {
        loading_Image.gameObject.SetActive(false);
    }

    public string GetId()
    {
        return id_InputField.text;
    }

    public string GetPin()
    {
        return pin_InputField.text;
    }

    public int GetMaxUser()
    {
        return int.Parse(maxUser_InputField.text);
    }

    public void SetState(bool state)
    {
        id_InputField.interactable = state;
        pin_InputField.interactable = state;
        maxUser_InputField.interactable = state;
        createSession_Button.interactable = state;
        loading_Image.gameObject.SetActive(!state);
    }
}
