using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static bool isUserLoggedIn
    {
        set
        {
            PlayerPrefs.SetInt("isUserLoggedIn", value ? 1 : 0);
        }
        get
        {
            return PlayerPrefs.GetInt("isUserLoggedIn") == 1;
        }
    }
    public static string userDataJson
    {
        get
        {
            return PlayerPrefs.GetString("userData");
        }
        set
        {
            PlayerPrefs.SetString("userData", value);
        }
    }
    public static UserData userData
    {
        get
        {
            return JsonUtility.FromJson<UserData>(userDataJson);
        }
        set
        {
            userDataJson = value.ObjectToJson();
        }
    }
}