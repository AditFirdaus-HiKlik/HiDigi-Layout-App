using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class JoinSessionView : MonoBehaviour
{
    public TMP_InputField globalID_InputField;
    public TMP_InputField pin_InputField;
    public Button join_Button;
    public Image loading_Image;

    private void Start()
    {
        loading_Image.gameObject.SetActive(false);
    }

    public string GetSessionID()
    {
        return globalID_InputField.text;
    }

    public string GetPin()
    {
        return pin_InputField.text;
    }

    public void SetState(bool state)
    {
        globalID_InputField.interactable = state;
        pin_InputField.interactable = state;
        join_Button.interactable = state;
        loading_Image.gameObject.SetActive(!state);
    }
}
