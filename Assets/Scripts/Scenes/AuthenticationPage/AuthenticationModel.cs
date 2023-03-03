using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationModel : MonoBehaviour
{
    public Coroutine Login(string username, string password)
    {
        return StartCoroutine(LoginAsync(username, password));
    }

    public Coroutine Logout()
    {
        return StartCoroutine(LogoutAsync());
    }

    public IEnumerator LoginAsync(string username, string password)
    {
        yield return AuthAPI.Login(username, password, (userData) =>
        {
            UserManager.isUserLoggedIn = true;
            UserManager.userData = userData;

            Debug.Log("Login success");
        });
    }

    public IEnumerator LogoutAsync()
    {
        UserManager.isUserLoggedIn = false;
        UserManager.userData = null;

        yield return null;
    }
}
