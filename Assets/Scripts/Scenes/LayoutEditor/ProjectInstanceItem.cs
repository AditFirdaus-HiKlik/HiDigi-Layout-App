using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Photon.Pun;

using TMPro;

public class ProjectInstanceItem : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public EditorProjectInstance projectInstanceController;

    [Header("Properties")]
    public InstanceData instanceData;

    [Header("References")]

    [Header("MVC")]
    public ProjectInstanceItemView view = new ProjectInstanceItemView();
    public ProjectInstanceItemModel model = new ProjectInstanceItemModel();

    private void Update()
    {
        view.Update();
    }

    public void Init(EditorProjectInstance projectController)
    {
        this.projectInstanceController = projectController;

        model.Init(this);
        view.Init(this);
    }

    public void SetInstanceData(InstanceData instanceData)
    {
        this.instanceData = instanceData;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SelectInstance();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            EditInstance();
        }
    }

    public void SelectInstance()
    {
        DebugApp.Log("Selecting instance: " + instanceData.name);

        projectInstanceController.SelectInstance(instanceData);
    }

    public void EditInstance()
    {
        DebugApp.Log("Editing instance: " + instanceData.name);

        projectInstanceController.EditInstance(instanceData);
    }

    public void Refresh()
    {
        view.Refresh();
    }

    public void Remove()
    {
        GameObject.Destroy(gameObject);
    }
}

[System.Serializable]
public class ProjectInstanceItemView
{
    [HideInInspector] public ProjectInstanceItem controller;

    public Image loadingImage;
    public Image preview_Image;
    public TMP_Text name_Text;

    public bool isLoadingImage;

    public void Init(ProjectInstanceItem controller)
    {
        this.controller = controller;
    }

    public void Refresh()
    {
        name_Text.text = controller.instanceData.name;

        SetLoadingState(isLoadingImage);

        LoadPreview(controller.instanceData.preview.url);
    }

    public void Update()
    {
        if (isLoadingImage)
        {
            loadingImage.transform.Rotate(0, 0, -180 * Time.deltaTime);
        }
    }

    public void SetLoadingState(bool isLoading)
    {
        isLoadingImage = isLoading;

        loadingImage.gameObject.SetActive(isLoading);
    }

    public void LoadPreview(string url)
    {
        controller.StartCoroutine(LoadPreviewAsync(url));
    }

    public IEnumerator LoadPreviewAsync(string url)
    {
        if (url.Length == 0) yield break;

        SetLoadingState(true);

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            DebugApp.Log(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            preview_Image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        SetLoadingState(false);
    }
}

[System.Serializable]
public class ProjectInstanceItemModel
{
    [HideInInspector] public ProjectInstanceItem controller;

    public void Init(ProjectInstanceItem controller)
    {
        this.controller = controller;
    }
}
