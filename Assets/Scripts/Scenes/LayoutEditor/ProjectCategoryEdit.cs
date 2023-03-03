using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ProjectCategoryEdit : ScreenBehaviour
{
    [HideInInspector] public EditorProjectCategory projectCategoryController;
    public CategoryData categoryData
    {
        get => screenData.GetValueOrDefault("categoryData", null) as CategoryData;
        set => screenData["categoryData"] = value;
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

    [Header("Properties")]

    [Header("References")]

    [Header("MVC")]
    public ProjectCategoryEditView view = new ProjectCategoryEditView();
    public ProjectCategoryEditModel model = new ProjectCategoryEditModel();

    public void Init(EditorProjectCategory projectController)
    {
        this.projectCategoryController = projectController;

        model.Init(this);
        view.Init(this);
    }

    public void Close()
    {
        PopClose();
    }

    public void SaveCategory()
    {
        StartCoroutine(SaveCategoryAsync());
    }

    public void DeleteCategory()
    {
        StartCoroutine(DeleteCategoryAsync());
    }

    public IEnumerator SaveCategoryAsync()
    {
        categoryData.name = view.nameInput.text;

        view.SetLoadingIndicator(true);

        yield return model.SaveCategory(categoryData);

        view.SetLoadingIndicator(false);

        Close();
    }

    public IEnumerator DeleteCategoryAsync()
    {
        view.SetLoadingIndicator(true);

        yield return model.DeleteCategory(categoryData);

        view.SetLoadingIndicator(false);

        Close();
    }
}

[System.Serializable]
public class ProjectCategoryEditView
{
    [HideInInspector] public ProjectCategoryEdit controller;

    public TMP_InputField nameInput;
    public LoadingIndicator loadingIndicator;

    public void Init(ProjectCategoryEdit controller)
    {
        this.controller = controller;

        Refresh();
    }

    public void Refresh()
    {
        nameInput.text = controller.categoryData.name;
    }

    public string GetNameInput()
    {
        return nameInput.text;
    }

    public void SetLoadingIndicator(bool value)
    {
        loadingIndicator.SetLoading(value);
    }
}

[System.Serializable]
public class ProjectCategoryEditModel
{
    [HideInInspector] public ProjectCategoryEdit controller;

    public void Init(ProjectCategoryEdit controller)
    {
        this.controller = controller;
    }

    public Coroutine SaveCategory(CategoryData categoryData)
    {
        return controller.StartCoroutine(SaveCategoryAsync(categoryData));
    }

    public Coroutine DeleteCategory(CategoryData categoryData)
    {
        return controller.StartCoroutine(DeleteCategoryAsync(categoryData));
    }

    private IEnumerator SaveCategoryAsync(CategoryData categoryData)
    {
        LayoutDataProject projectData = LayoutManager.layoutData.project;

        if (controller.isNew)
        {
            projectData.categories.Add(categoryData);
        }
        else
        {
            int index = projectData.categories.FindIndex(x => x.id == categoryData.id);

            if (index >= 0)
            {
                projectData.categories[index] = categoryData;
            }
        }

        LayoutManager.layoutData.project = projectData;

        yield return LayoutManager.SaveLayoutData();
    }

    private IEnumerator DeleteCategoryAsync(CategoryData categoryData)
    {
        LayoutDataProject projectData = LayoutManager.layoutData.project;

        int index = projectData.categories.FindIndex(x => x.id == categoryData.id);

        if (index >= 0)
        {
            projectData.categories.RemoveAt(index);
        }

        LayoutManager.layoutData.project = projectData;

        yield return LayoutManager.SaveLayoutData();
    }
}
