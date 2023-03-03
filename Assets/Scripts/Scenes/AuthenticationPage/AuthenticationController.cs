using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationController : MonoBehaviour
{
    public bool validateUserOnStart = true;
    public AuthenticationModel authenticationModel;
    public AuthenticationView authenticationView;

    private void Start()
    {
        if (validateUserOnStart) ValidateUser();
    }

    public void ValidateUser()
    {
        if (UserManager.isUserLoggedIn)
        {
            LoadScene("MenuPage");
        }
        else
        {
            // LoadScene("MainPage");
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void Login()
    {
        StartCoroutine(LoginAsync());
    }

    public void Logout()
    {
        StartCoroutine(LogoutAsync());
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        authenticationView.SetState(false);
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    public IEnumerator LoginAsync()
    {
        authenticationView.SetState(false);
        yield return authenticationModel.Login(authenticationView.GetUsername(), authenticationView.GetPassword());
        authenticationView.SetState(true);

        if (UserManager.isUserLoggedIn)
        {
            LoadScene("MenuPage");
        }
    }

    public IEnumerator LogoutAsync()
    {
        authenticationView.SetState(false);
        yield return authenticationModel.Logout();
        authenticationView.SetState(true);
    }
}
