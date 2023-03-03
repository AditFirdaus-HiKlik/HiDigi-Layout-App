using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using Firebase.Storage;

using TriLibCore.SFB;

using TMPro;

public class ProjectInstanceEdit : ScreenBehaviour
{
    public static EditorProjectInstance instanceController => EditorProjectInstance.instance;

    public InstanceData instanceData
    {
        get => screenData.GetValueOrDefault("instanceData", null) as InstanceData;
        set => screenData["instanceData"] = value;
    }

    public bool isNew
    {
        get => (bool)screenData.GetValueOrDefault("isNew", false);
        set => screenData["isNew"] = value;
    }

    public bool isEdit
    {
        get => (bool)screenData.GetValueOrDefault("isEdit", false);
        set => screenData["isEdit"] = value;
    }

    public InstanceDataPreview pickedPreview
    {
        get => screenData.GetValueOrDefault("pickedPreview", new InstanceDataPreview()) as InstanceDataPreview;
        set => screenData["pickedPreview"] = value;
    }

    public InstanceDataModel pickedModel
    {
        get => screenData.GetValueOrDefault("pickedModel", new InstanceDataModel()) as InstanceDataModel;
        set => screenData["pickedModel"] = value;
    }

    [Header("Properties")]
    public bool usePickedPreview = false;
    public bool usePickedModel = false;

    [Header("References")]

    [Header("MVC")]
    public ProjectInstanceEditView view = new ProjectInstanceEditView();
    public ProjectInstanceEditModel model = new ProjectInstanceEditModel();

    public void Init()
    {
        if (isNew) RandomizeGUID();

        model.Init(this);
        view.Init(this);

        view.Refresh();
    }

    public void Close()
    {
        PopClose();
    }

    public void RandomizeGUID()
    {
        instanceData.id = Guid.NewGuid().ToString();
    }

    public void PickPreview()
    {
        StandaloneFileBrowser.OpenFilePanelAsync("Select preview image", "", new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") }, false, (result) =>
        {
            if (result.Count > 0)
            {
                string path = result[0].Name;

                string fileName = Path.GetFileName(path);
                string fileExtension = Path.GetExtension(path);

                InstanceDataPreview previewData = pickedPreview;

                previewData.url = $"file://{path}";
                previewData.fileName = fileName;
                previewData.fileExtension = fileExtension;

                pickedPreview = previewData;
                usePickedPreview = true;

                view.Refresh();
            }
        });
    }

    public void PickModel()
    {
        StandaloneFileBrowser.OpenFilePanelAsync("Select model", "", new[] { new ExtensionFilter("Model Files", "fbx", "obj", "gltf", "glb") }, false, (result) =>
        {
            if (result.Count > 0)
            {
                string path = result[0].Name;

                string fileName = Path.GetFileName(path);
                string fileExtension = Path.GetExtension(path);

                InstanceDataModel modelData = pickedModel;

                modelData.url = $"file://{path}";
                modelData.fileName = fileName;
                modelData.fileExtension = fileExtension;

                pickedModel = modelData;
                usePickedModel = true;

                view.Refresh();
            }
        });
    }

    public void SaveInstance()
    {
        StartCoroutine(SaveInstanceAsync());
    }

    public void DeleteInstance()
    {
        StartCoroutine(DeleteInstanceAsync());
    }

    public IEnumerator SaveInstanceAsync()
    {
        instanceData.name = view.name_InputField.text;

        view.SetLoadingIndicator(true);

        yield return model.SaveInstance(instanceData);

        view.SetLoadingIndicator(false);

        Close();
    }

    public IEnumerator DeleteInstanceAsync()
    {
        view.SetLoadingIndicator(true);

        yield return model.DeleteInstance(instanceData);

        view.SetLoadingIndicator(false);

        Close();
    }
}

[System.Serializable]
public class ProjectInstanceEditView
{
    [HideInInspector] public ProjectInstanceEdit controller;

    public GameObject save_Button;
    public GameObject delete_Button;
    public Button pickPreview_Button;
    public Button pickModel_Button;

    public TMP_InputField name_InputField;
    public Image preview_Image;
    public TMP_Text model_Text;
    public LoadingIndicator loadingIndicator;

    Coroutine refreshPreviewCoroutine;

    public void Init(ProjectInstanceEdit controller)
    {
        this.controller = controller;

        Refresh();
    }

    public void Refresh()
    {
        Debug.Log(controller.instanceData.ObjectToJson());
        name_InputField.text = controller.instanceData.name;

        SetSaveButton(controller.isEdit);
        SetDeleteButton(controller.isEdit);

        RefreshPreview();
        RefreshModel();
    }

    public void RefreshPreview()
    {
        if (refreshPreviewCoroutine != null) controller.StopCoroutine(refreshPreviewCoroutine);
        refreshPreviewCoroutine = controller.StartCoroutine(RefreshPreviewAsync());
    }

    public void RefreshModel()
    {
        string fileName = "";

        if (controller.instanceData.model != null)
        {
            fileName = controller.instanceData.model.fileName;
        }

        if (controller.usePickedModel)
        {
            if (controller.pickedModel != null)
            {
                fileName = controller.pickedModel.fileName;
            }
        }

        if (fileName == "") fileName = "No model";

        model_Text.text = fileName;
    }

    public IEnumerator RefreshPreviewAsync()
    {
        SetPickPreviewButton(false);

        var previewData = controller.instanceData.preview;

        if (previewData != null)
        {
            string url = previewData.url;

            if (controller.usePickedPreview)
            {
                previewData = controller.pickedPreview;
                url = previewData.url;
            }

            // Using unityWebRequest to load the image
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

            // Wait for the request to complete
            yield return www.SendWebRequest();

            // Check if there were any errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Get the texture from the request
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                // Set the texture to the image
                preview_Image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
        }

        SetPickPreviewButton(true);
    }

    public void SetSaveButton(bool value)
    {
        save_Button.SetActive(value);
    }

    public void SetPickPreviewButton(bool value)
    {
        pickPreview_Button.interactable = value;
    }

    public void SetPickModelButton(bool value)
    {
        pickModel_Button.interactable = value;
    }

    public void SetDeleteButton(bool value)
    {
        delete_Button.SetActive(value);
    }

    public string GetNameInput()
    {
        return name_InputField.text;
    }

    public void SetLoadingIndicator(bool value)
    {
        loadingIndicator.SetLoading(value);
    }
}

[System.Serializable]
public class ProjectInstanceEditModel
{
    [HideInInspector] public ProjectInstanceEdit controller;

    public void Init(ProjectInstanceEdit controller)
    {
        this.controller = controller;
    }

    public Coroutine SaveInstance(InstanceData instanceData)
    {
        return controller.StartCoroutine(SaveInstanceAsync(instanceData));
    }

    public Coroutine DeleteInstance(InstanceData instanceData)
    {
        return controller.StartCoroutine(DeleteInstanceAsync(instanceData));
    }

    public Coroutine UploadImage()
    {
        return controller.StartCoroutine(UploadImageAsync());
    }

    public Coroutine UploadModel()
    {
        return controller.StartCoroutine(UploadModelAsync());
    }

    private IEnumerator SaveInstanceAsync(InstanceData instanceData)
    {
        LayoutDataProject projectData = LayoutManager.layoutData.project;

        string focusedCategory = EditorProjectCategory.focusedCategoryID;
        CategoryData categoryData = projectData.GetCategory(focusedCategory);

        if (categoryData != null)
        {
            if (controller.isNew)
            {
                categoryData.instances.Add(instanceData);
            }
            else
            {
                int index = categoryData.instances.FindIndex(x => x.id == instanceData.id);

                if (index >= 0)
                {
                    categoryData.instances[index] = instanceData;
                }
            }
        }

        if (controller.usePickedPreview)
        {
            yield return UploadImage();
        }

        if (controller.usePickedModel)
        {
            yield return UploadModel();
        }

        LayoutManager.layoutData.project = projectData;

        yield return LayoutManager.SaveLayoutData();
    }

    private IEnumerator DeleteInstanceAsync(InstanceData instanceData)
    {
        LayoutDataProject projectData = LayoutManager.layoutData.project;

        string focusedCategory = EditorProjectCategory.focusedCategoryID;
        CategoryData categoryData = projectData.GetCategory(focusedCategory);

        if (categoryData != null)
        {
            if (controller.isNew)
            {
                categoryData.instances.Add(instanceData);
            }
            else
            {
                int index = categoryData.instances.FindIndex(x => x.id == instanceData.id);

                if (index >= 0)
                {
                    categoryData.instances.RemoveAt(index);
                }
            }
        }

        yield return DeletePreview();
        yield return DeleteModel();

        LayoutManager.layoutData.project = projectData;

        yield return LayoutManager.SaveLayoutData();
    }

    public Coroutine DeletePreview()
    {
        return controller.StartCoroutine(DeletePreviewAsync());
    }

    public Coroutine DeleteModel()
    {
        return controller.StartCoroutine(DeleteModelAsync());
    }

    public IEnumerator UploadImageAsync()
    {
        InstanceData instanceData = controller.instanceData;
        InstanceDataPreview previewData = controller.pickedPreview;

        // Using unityWebRequest to load the image
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(previewData.url);

        // Wait for the request to complete
        yield return www.SendWebRequest();

        // Check if there were any errors
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Get the texture from the request
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            // Set the texture to the image
            byte[] bytes = texture.EncodeToJPG(75);

            string previewPath = $"sessions/{LayoutManager.layoutID}/previews/{instanceData.id}.jpg";

            StorageReference metadata = null;

            yield return StorageAPI.UploadFile(previewPath, bytes, callback => metadata = callback.Reference);

            if (metadata != null)
            {
                var task = metadata.GetDownloadUrlAsync();

                yield return new WaitUntil(() => task.IsCompleted);

                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }
                else
                {
                    string url = task.Result.ToString();

                    previewData.url = url;

                    controller.instanceData.preview = previewData;
                }
            }
        }
    }

    public IEnumerator UploadModelAsync()
    {
        InstanceData instanceData = controller.instanceData;
        InstanceDataModel modelData = controller.pickedModel;

        // Using unityWebRequest to load the image
        UnityWebRequest www = UnityWebRequest.Get(modelData.url);

        // Wait for the request to complete
        yield return www.SendWebRequest();

        // Check if there were any errors
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Get the texture from the request
            byte[] bytes = www.downloadHandler.data;

            string modelPath = $"sessions/{LayoutManager.layoutID}/models/{instanceData.id}{modelData.fileExtension}";

            StorageReference metadata = null;

            yield return StorageAPI.UploadFile(modelPath, bytes, callback => metadata = callback.Reference);

            if (metadata != null)
            {
                var task = metadata.GetDownloadUrlAsync();

                yield return new WaitUntil(() => task.IsCompleted);

                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }
                else
                {
                    string url = task.Result.ToString();

                    modelData.url = url;

                    controller.instanceData.model = modelData;
                }
            }
        }
    }

    public IEnumerator DeletePreviewAsync()
    {
        InstanceData instanceData = controller.instanceData;
        InstanceDataPreview previewData = controller.pickedPreview;

        string previewPath = $"sessions/{LayoutManager.layoutID}/previews/{instanceData.id}.jpg";

        yield return StorageAPI.DeleteFile(previewPath, () => { });
    }

    public IEnumerator DeleteModelAsync()
    {
        InstanceData instanceData = controller.instanceData;
        InstanceDataModel modelData = controller.pickedModel;

        string modelPath = $"sessions/{LayoutManager.layoutID}/models/{instanceData.id}{modelData.fileExtension}";

        yield return StorageAPI.DeleteFile(modelPath, () => { });
    }
}