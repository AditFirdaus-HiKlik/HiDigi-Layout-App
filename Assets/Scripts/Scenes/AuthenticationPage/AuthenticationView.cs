using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;

public class AuthenticationView : MonoBehaviour
{
    public TMP_InputField username_InputField;
    public TMP_InputField password_InputField;
    public Button login_Button;
    public Image loading_Image;

    private void Start()
    {
        loading_Image.gameObject.SetActive(false);
    }

    public string GetUsername()
    {
        return username_InputField.text;
    }

    public string GetPassword()
    {
        return password_InputField.text;
    }

    public void SetState(bool state)
    {
        username_InputField.interactable = state;
        password_InputField.interactable = state;
        login_Button.interactable = state;
        loading_Image.gameObject.SetActive(!state);
    }
}
