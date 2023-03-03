using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class CreateSessionController : MonoBehaviour
{
    public CreateSessionModel model;
    public CreateSessionView view;

    public void CreateSession()
    {
        StartCoroutine(CreateSessionAsync());
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        view.SetState(false);
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    public IEnumerator CreateSessionAsync()
    {
        view.SetState(false);
        yield return model.CreateSession(
            view.GetId(),
            view.GetMaxUser()
        );
        view.SetState(true);
        if (PhotonNetwork.InRoom)
        {
            LoadScene("LayoutCloudEditor");
        }
    }
}
