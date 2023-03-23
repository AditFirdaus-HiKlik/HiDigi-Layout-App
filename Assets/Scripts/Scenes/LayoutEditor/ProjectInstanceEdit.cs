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

    public InstanceData instanceDataTemp = new InstanceData();

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
        get => screenData.GetValueOrDefault("pickedPreview", instanceDataTemp.preview) as InstanceDataPreview;
        set => screenData["pickedPreview"] = value;
    }

    public InstanceDataModel pickedModel
    {
        get => screenData.GetValueOrDefault("pickedModel", instanceDataTemp.model) as InstanceDataModel;
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

        CreateTempInstanceData();

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
        StartCoroutine(PickPreviewAsync());
    }

    public void PickModel()
    {
        StartCoroutine(PickModelAsync());
    }

    public void SetZip(bool value)
    {
        view.instanceIsZipped = value;
    }

    public void SetName(string value)
    {
        view.instanceName = value;
    }

    public void SaveInstance()
    {
        StartCoroutine(SaveInstanceAsync());
    }

    public void DeleteInstance()
    {
        StartCoroutine(DeleteInstanceAsync());
    }

    public void CreateTempInstanceData()
    {
        instanceDataTemp = JsonUtility.FromJson<InstanceData>(JsonUtility.ToJson(instanceData));

        pickedPreview = instanceDataTemp.preview;
        pickedModel = instanceDataTemp.model;
    }

    IEnumerator SaveInstanceAsync()
    {
        instanceData = JsonUtility.FromJson<InstanceData>(JsonUtility.ToJson(instanceDataTemp));

        view.SetLoadingIndicator(true);

        yield return model.SaveInstance(instanceData);

        view.SetLoadingIndicator(false);

        Close();
    }

    IEnumerator DeleteInstanceAsync()
    {
        view.SetLoadingIndicator(true);

        yield return model.DeleteInstance(instanceData);

        view.SetLoadingIndicator(false);

        Close();
    }

    IEnumerator PickPreviewAsync()
    {
        yield return model.PickPreview();

        view.RefreshPreview();
    }

    IEnumerator PickModelAsync()
    {
        yield return model.PickModel();

        view.RefreshModel();
    }
}

[System.Serializable]
public class ProjectInstanceEditView
{
    [HideInInspector] public ProjectInstanceEdit controller;

    public InstanceData tempData => controller.instanceDataTemp;

    public GameObject save_Button;
    public GameObject delete_Button;
    public Button pickPreview_Button;
    public Button pickModel_Button;
    public Toggle zip_toggle;

    public TMP_InputField name_InputField;
    public Image preview_Image;
    public TMP_Text model_Text;
    public LoadingIndicator loadingIndicator;

    public bool instanceIsZipped
    {
        set
        {
            instanceModel.isZipped = value;
            RefreshZipToggle();
        }
        get => tempData.model.isZipped;
    }

    public string instanceName
    {
        set
        {
            tempData.name = value;
            RefreshName();
        }
        get => tempData.name;
    }

    public InstanceDataPreview instancePreview
    {
        set
        {
            tempData.preview = value;

            if (tempData.preview.url != value.url)
            {
                RefreshPreview();
            }
        }
        get => tempData.preview;
    }

    public InstanceDataModel instanceModel
    {
        set
        {
            tempData.model = value;
            RefreshModel();
        }
        get => tempData.model;
    }

    Coroutine refreshPreviewCoroutine;

    public void Init(ProjectInstanceEdit controller)
    {
        this.controller = controller;

        Refresh();
    }

    public void Refresh()
    {
        SetSaveButton(controller.isEdit);
        SetDeleteButton(controller.isEdit);

        RefreshZipToggle();
        RefreshName();
        RefreshPreview();
        RefreshModel();
    }

    public void RefreshZipToggle()
    {
        zip_toggle.isOn = instanceIsZipped;
    }

    public void RefreshName()
    {
        name_InputField.text = instanceName;
    }

    public void RefreshPreview()
    {
        if (refreshPreviewCoroutine != null) controller.StopCoroutine(refreshPreviewCoroutine);
        refreshPreviewCoroutine = controller.StartCoroutine(RefreshPreviewAsync());
    }

    public void RefreshModel()
    {
        string fileName = "";

        if (instanceModel != null)
        {
            fileName = instanceModel.fileName;
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

        if (instancePreview != null)
        {
            string url = instancePreview.url;

            if (controller.usePickedPreview)
            {
                instancePreview = controller.pickedPreview;
                url = instancePreview.url;
            }

            // Using unityWebRequest to load the image
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

            // Wait for the request to complete
            yield return www.SendWebRequest();

            // Check if there were any errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                DebugApp.Log(www.error);
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

    public void SetLoadingIndicator(bool value)
    {
        loadingIndicator.SetLoading(value);
    }
}

[System.Serializable]
public class ProjectInstanceEditModel
{
    [HideInInspector] public ProjectInstanceEdit controller;

    public InstanceData tempData => controller.instanceDataTemp;

    public bool instanceIsZipped
    {
        set => instanceModel.isZipped = value;
        get => tempData.model.isZipped;
    }

    public string instanceName
    {
        set => tempData.name = value;
        get => tempData.name;
    }

    public InstanceDataPreview instancePreview => controller.instanceDataTemp.preview;
    public InstanceDataModel instanceModel => controller.instanceDataTemp.model;

    public void Init(ProjectInstanceEdit controller)
    {
        this.controller = controller;
    }

    public Coroutine PickPreview()
    {
        return controller.StartCoroutine(PickPreviewAsync());
    }

    public Coroutine PickModel()
    {
        return controller.StartCoroutine(PickModelAsync());
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

    public Coroutine DeletePreview()
    {
        return controller.StartCoroutine(DeletePreviewAsync());
    }

    public Coroutine DeleteModel()
    {
        return controller.StartCoroutine(DeleteModelAsync());
    }

    IEnumerator PickPreviewAsync()
    {
        DebugApp.Log("PickPreviewAsync: Opening File Panel...");

        bool isPicked = false;

        StandaloneFileBrowser.OpenFilePanelAsync("Select preview image", "", new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") }, false, (result) =>
        {
            DebugApp.Log("PickPreviewAsync: File Panel Closed");

            if (result.Count > 0)
            {
                string path = result[0].Name;

                DebugApp.Log($"PickPreviewAsync: Picked file: {path}");

                instancePreview.url = $"file://{path}";
                instancePreview.fileName = Path.GetFileName(path);
                instancePreview.fileExtension = Path.GetExtension(path);

                controller.usePickedPreview = true;

                isPicked = true;
            }
        });

        yield return new WaitUntil(() => isPicked);
    }

    IEnumerator PickModelAsync()
    {
        bool isPicked = false;

        ExtensionFilter[] filters = instanceIsZipped ? new[] { new ExtensionFilter("Model Files", "zip") } : new[] { new ExtensionFilter("Model Files", "fbx", "obj") };

        StandaloneFileBrowser.OpenFilePanelAsync("Select model file", "", filters, false, (result) =>
        {
            if (result.Count > 0)
            {
                string path = result[0].Name;

                instanceModel.url = $"file://{path}";
                instanceModel.isZipped = instanceIsZipped;
                instanceModel.fileName = Path.GetFileName(path);
                instanceModel.fileExtension = Path.GetExtension(path);

                controller.usePickedModel = true;

                isPicked = true;
            }
        });

        yield return new WaitUntil(() => isPicked);
    }

    IEnumerator SaveInstanceAsync(InstanceData instanceData)
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

    IEnumerator DeleteInstanceAsync(InstanceData instanceData)
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

    IEnumerator UploadImageAsync()
    {
        // Using unityWebRequest to load the image
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(instancePreview.url);

        // Wait for the request to complete
        yield return www.SendWebRequest();

        // Check if there were any errors
        if (www.result != UnityWebRequest.Result.Success)
        {
            DebugApp.Log(www.error);
        }
        else
        {
            // Get the texture from the request
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            // Set the texture to the image
            byte[] bytes = texture.EncodeToJPG(75);

            string previewPath = $"sessions/{LayoutManager.layoutID}/previews/{tempData.id}.jpg";

            StorageReference metadata = null;

            yield return StorageAPI.UploadFile(previewPath, bytes, callback => metadata = callback.Reference);

            if (metadata != null)
            {
                var task = metadata.GetDownloadUrlAsync();

                yield return new WaitUntil(() => task.IsCompleted);

                if (task.IsFaulted)
                {
                    DebugApp.Log(task.Exception.Message);
                }
                else
                {
                    string url = task.Result.ToString();

                    instancePreview.url = url;

                    controller.instanceData.preview = instancePreview;
                }
            }
        }
    }

    IEnumerator UploadModelAsync()
    {
        // Using unityWebRequest to load the image
        UnityWebRequest www = UnityWebRequest.Get(instanceModel.url);

        // Wait for the request to complete
        yield return www.SendWebRequest();

        // Check if there were any errors
        if (www.result != UnityWebRequest.Result.Success)
        {
            DebugApp.Log(www.error);
        }
        else
        {
            // Get the texture from the request
            byte[] bytes = www.downloadHandler.data;

            string modelPath = $"sessions/{LayoutManager.layoutID}/models/{tempData.id}{instanceModel.fileExtension}";

            StorageReference metadata = null;

            yield return StorageAPI.UploadFile(modelPath, bytes, callback => metadata = callback.Reference);

            if (metadata != null)
            {
                var task = metadata.GetDownloadUrlAsync();

                yield return new WaitUntil(() => task.IsCompleted);

                if (task.IsFaulted)
                {
                    DebugApp.Log(task.Exception.Message);
                }
                else
                {
                    string url = task.Result.ToString();

                    instanceModel.url = url;

                    controller.instanceData.model = instanceModel;
                }
            }
        }
    }

    IEnumerator DeletePreviewAsync()
    {
        string previewPath = $"sessions/{LayoutManager.layoutID}/previews/{tempData.id}.jpg";

        yield return StorageAPI.DeleteFile(previewPath, () => { });
    }

    IEnumerator DeleteModelAsync()
    {
        string modelPath = $"sessions/{LayoutManager.layoutID}/models/{tempData.id}{instanceModel.fileExtension}";

        yield return StorageAPI.DeleteFile(modelPath, () => { });
    }
}