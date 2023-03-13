using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class JoinSessionController : MonoBehaviour
{
    public JoinSessionModel model;
    public JoinSessionView view;

    public void JoinSession()
    {
        StartCoroutine(JoinSessionAsync());
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

    public IEnumerator JoinSessionAsync()
    {
        view.SetState(false);
        yield return model.JoinSession(
            view.GetSessionID(),
            view.GetPin()
        );
        Debug.Log("Join session complete");
        // Wait until user is in room
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        view.SetState(true);
        LoadScene("LayoutCloudEditor");
    }
}
