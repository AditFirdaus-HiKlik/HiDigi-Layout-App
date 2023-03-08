using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutEditorProject : MonoBehaviour
{
    public static LayoutEditorProject instance;
    public LayoutEditorProjectView view = new LayoutEditorProjectView();
    public LayoutEditorProjectModel model = new LayoutEditorProjectModel();

    public LayoutDataProject layoutDataProject = new LayoutDataProject();

    private void Awake()
    {
        instance = this;

        Init();
    }

    private void Init()
    {
        model.Init(this);
        view.Init(this);
    }

    public void Render(LayoutDataProject layoutDataProject)
    {
        this.layoutDataProject = layoutDataProject;

        view.RenderCategory(layoutDataProject.categories);

        string focusedCategoryID = EditorProjectCategory.focusedCategoryID;

        CategoryData focusedCategory = layoutDataProject.GetCategory(focusedCategoryID);

        if (focusedCategory != null)
        {
            view.RenderInstance(focusedCategory.GetInstances());
        }
        else
        {
            view.RenderInstance(new List<InstanceData>());
        }
    }

    public void RenderCategory(List<CategoryData> categoryDatas)
    {
        view.RenderCategory(categoryDatas);
    }

    public void RenderInstance(List<InstanceData> instanceDatas)
    {
        view.RenderInstance(instanceDatas);
    }
}

[System.Serializable]
public class LayoutEditorProjectView
{
    [HideInInspector] public LayoutEditorProject controller;

    public EditorProjectCategory projectCategoryController;
    public EditorProjectInstance projectInstanceController;

    public void Init(LayoutEditorProject controller)
    {
        this.controller = controller;

        projectCategoryController.Init();
        projectInstanceController.Init();
    }

    public void RenderCategory(List<CategoryData> categoryDatas)
    {
        projectCategoryController.Render(categoryDatas);
    }

    public void RenderInstance(List<InstanceData> instanceDatas)
    {
        projectInstanceController.Render(instanceDatas);
    }
}

[System.Serializable]
public class LayoutEditorProjectModel
{
    [HideInInspector] public LayoutEditorProject controller;

    public void Init(LayoutEditorProject controller)
    {
        this.controller = controller;
    }
}
