using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using TMPro;

public class ProjectCategoryItem : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public EditorProjectCategory projectCategoryController;

    [Header("Properties")]
    public CategoryData categoryData = new CategoryData();

    [Header("References")]

    [Header("MVC")]
    public ProjectCategoryItemView view = new ProjectCategoryItemView();
    public ProjectCategoryItemModel model = new ProjectCategoryItemModel();

    public void Init(EditorProjectCategory projectController)
    {
        this.projectCategoryController = projectController;

        model.Init(this);
        view.Init(this);
    }

    public void SetCategoryData(CategoryData categoryData)
    {
        this.categoryData = categoryData;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SelectCategory();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            EditCategory();
        }
    }

    public void SelectCategory()
    {
        DebugApp.Log("Selecting category: " + categoryData.name);

        projectCategoryController.SelectCategory(categoryData);
    }

    public void EditCategory()
    {
        DebugApp.Log("Editing category: " + categoryData.name);

        projectCategoryController.EditCategory(categoryData);
    }

    public void Refresh()
    {
        view.Refresh();
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}

[System.Serializable]
public class ProjectCategoryItemView
{
    [HideInInspector] public ProjectCategoryItem controller;

    public TMP_Text text_Name;

    public void Init(ProjectCategoryItem controller)
    {
        this.controller = controller;
    }

    public void Refresh()
    {
        text_Name.text = controller.categoryData.name;
    }
}

[System.Serializable]
public class ProjectCategoryItemModel
{
    [HideInInspector] public ProjectCategoryItem controller;

    public void Init(ProjectCategoryItem controller)
    {
        this.controller = controller;
    }
}
