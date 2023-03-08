using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorProjectInstance : MonoBehaviour
{
    public static EditorProjectInstance instance;
    public static LayoutEditorProject projectController = LayoutEditorProject.instance;

    [Header("Properties")]
    public static InstanceData focusedInstance;
    public static string focusedInstanceID;

    [Header("References")]
    public GameObject instanceEdit_Window;

    [Header("MVC")]
    public EditorProjectInstanceView view = new EditorProjectInstanceView();
    public EditorProjectInstanceModel model = new EditorProjectInstanceModel();

    private void Awake()
    {
        instance = this;

        Refresh();
    }

    public void Init()
    {
        model.Init(this);
        view.Init(this);
    }

    public void Render(List<InstanceData> instanceDatas)
    {
        view.Render(instanceDatas);
    }

    public void Refresh()
    {
        view.SetAddButtonState(EditorProjectCategory.focusedCategoryID.Length > 0);
    }

    public void SelectInstance(InstanceData instance)
    {
        focusedInstance = instance;
        focusedInstanceID = instance.id;

        LayoutInstanceData layoutInstanceData = new LayoutInstanceData();

        layoutInstanceData.data = instance;

        LayoutManager.InstantiateLayoutObject(layoutInstanceData);

        Debug.Log("Instance selected: " + instance.name);
    }

    public void AddInstance()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data.Add("instanceData", new InstanceData());
        data.Add("isNew", true);
        data.Add("isEdit", true);

        ProjectInstanceEdit screen = ScreenManager.instance.OpenScreenReturn(instanceEdit_Window, data) as ProjectInstanceEdit;

        screen.Init();
    }

    public void EditInstance(InstanceData instance)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data.Add("instanceData", instance);
        data.Add("isNew", false);
        data.Add("isEdit", true);

        ProjectInstanceEdit screen = ScreenManager.instance.OpenScreenReturn(instanceEdit_Window, data) as ProjectInstanceEdit;

        screen.Init();
    }
}

[System.Serializable]
public class EditorProjectInstanceView
{
    [HideInInspector] public EditorProjectInstance controller;

    [Header("Add")]
    public Transform add_Button;

    [Header("Item")]
    public Transform instanceItemsParent;
    public GameObject instanceItemPrefab;
    public List<ProjectInstanceItem> instanceItems = new List<ProjectInstanceItem>();

    List<ProjectInstanceItem> isExist = new List<ProjectInstanceItem>();
    List<ProjectInstanceItem> mustDelete = new List<ProjectInstanceItem>();
    List<InstanceData> mustCreate = new List<InstanceData>();

    public void Init(EditorProjectInstance controller)
    {
        this.controller = controller;
    }

    public void Render(List<InstanceData> instanceDatas)
    {
        ValidateInstanceItems(instanceDatas);

        DeleteInstanceItems(mustDelete);
        CreateInstanceItems(mustCreate);

        controller.Refresh();
    }

    public void SetAddButtonState(bool state)
    {
        add_Button.gameObject.SetActive(state);
    }

    public void ValidateInstanceItems(List<InstanceData> instanceDatas)
    {
        isExist.Clear();
        mustDelete.Clear();
        mustCreate.Clear();

        foreach (var instanceItem in instanceItems)
        {
            if (instanceItem == null) continue;

            bool isFound = instanceDatas.Contains(instanceItem.instanceData);

            if (!isFound)
            {
                mustDelete.Add(instanceItem);
            }
            else
            {
                isExist.Add(instanceItem);
            }
        }

        foreach (var instanceData in instanceDatas)
        {
            bool isFound = isExist.Find(x => x.instanceData == instanceData) != null;

            if (!isFound)
            {
                mustCreate.Add(instanceData);
            }
        }
    }

    public void DeleteInstanceItems(List<ProjectInstanceItem> items)
    {
        foreach (var item in items)
        {
            instanceItems.Remove(item);
            item.Remove();
        }
    }

    public void CreateInstanceItems(List<InstanceData> items)
    {
        GameObject controllerGameObject = controller.gameObject;

        foreach (var item in items)
        {
            ProjectInstanceItem instanceItem = GameObject.Instantiate(instanceItemPrefab, instanceItemsParent).GetComponent<ProjectInstanceItem>();

            instanceItem.Init(controller);
            instanceItem.SetInstanceData(item);
            instanceItem.Refresh();

            instanceItems.Add(instanceItem);

            isExist.Add(instanceItem);
        }

        add_Button.SetAsLastSibling();
    }
}

[System.Serializable]
public class EditorProjectInstanceModel
{
    [HideInInspector] public EditorProjectInstance controller;
    public void Init(EditorProjectInstance controller)
    {
        this.controller = controller;
    }
}
