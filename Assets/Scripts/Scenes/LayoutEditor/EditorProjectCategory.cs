using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorProjectCategory : MonoBehaviour
{
    public static EditorProjectCategory instance;
    public static LayoutEditorProject projectController => LayoutEditorProject.instance;


    [Header("Properties")]
    public static CategoryData focusedCategory;
    public static string focusedCategoryID = "";

    [Header("References")]
    public GameObject categoryEdit_Window;

    [Header("MVC")]
    public EditorProjectCategoryView view = new EditorProjectCategoryView();
    public EditorProjectCategoryModel model = new EditorProjectCategoryModel();

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        model.Init(this);
        view.Init(this);
    }

    public void Render(List<CategoryData> categoryDatas)
    {
        view.Render(categoryDatas);
    }

    public void SelectCategory(CategoryData category)
    {
        focusedCategory = category;
        focusedCategoryID = category.id;

        Debug.Log(projectController);
        Debug.Log(category);

        projectController.RenderInstance(category.GetInstances());

        Debug.Log("Category selected: " + category.name);
    }

    public void ShowAllInstances()
    {
        focusedCategory = null;
        focusedCategoryID = "";

        projectController.RenderInstance(projectController.layoutDataProject.GetInstances());
    }

    public void AddCategory()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data.Add("categoryData", new CategoryData());
        data.Add("isNew", true);

        ProjectCategoryEdit screen = ScreenManager.instance.OpenScreenReturn(categoryEdit_Window, data) as ProjectCategoryEdit;

        screen.Init(this);
    }

    public void EditCategory(CategoryData category)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data.Add("categoryData", category);
        data.Add("isNew", false);

        ProjectCategoryEdit screen = ScreenManager.instance.OpenScreenReturn(categoryEdit_Window, data) as ProjectCategoryEdit;

        screen.Init(this);
    }
}

[System.Serializable]
public class EditorProjectCategoryView
{
    [HideInInspector] public EditorProjectCategory controller;

    [Header("Add")]
    public Transform add_Button;

    [Header("Item")]
    public Transform categoryItemsParent;
    public GameObject categoryItemPrefab;
    public List<ProjectCategoryItem> categoryItems = new List<ProjectCategoryItem>();

    List<ProjectCategoryItem> isExist = new List<ProjectCategoryItem>();
    List<ProjectCategoryItem> mustDelete = new List<ProjectCategoryItem>();
    List<CategoryData> mustCreate = new List<CategoryData>();

    public void Init(EditorProjectCategory controller)
    {
        this.controller = controller;
    }

    public void Render(List<CategoryData> categoryDatas)
    {
        ValidateCategoryItems(categoryDatas);

        DeleteCategoryItems(mustDelete);
        CreateCategoryItems(mustCreate);
    }

    public void SetAddButtonState(bool state)
    {
        add_Button.gameObject.SetActive(state);
    }

    void ValidateCategoryItems(List<CategoryData> categoryDatas)
    {
        isExist.Clear();
        mustDelete.Clear();
        mustCreate.Clear();

        foreach (var categoryItem in categoryItems)
        {
            bool isFound = categoryDatas.Contains(categoryItem.categoryData);

            if (!isFound)
            {
                mustDelete.Add(categoryItem);
            }
            else
            {
                isExist.Add(categoryItem);
            }

        }

        foreach (var categoryData in categoryDatas)
        {
            bool isFound = isExist.Find(x => x.categoryData == categoryData) != null;

            if (!isFound)
            {
                mustCreate.Add(categoryData);
            }
        }
    }

    void DeleteCategoryItems(List<ProjectCategoryItem> items)
    {
        foreach (var item in items)
        {
            categoryItems.Remove(item);
            item.Remove();
        }
    }

    void CreateCategoryItems(List<CategoryData> items)
    {
        GameObject controllerGameObject = controller.gameObject;

        foreach (var item in items)
        {
            ProjectCategoryItem categoryItem = GameObject.Instantiate(categoryItemPrefab, categoryItemsParent).GetComponent<ProjectCategoryItem>();

            categoryItem.Init(controller);
            categoryItem.SetCategoryData(item);
            categoryItem.Refresh();

            categoryItems.Add(categoryItem);

            isExist.Add(categoryItem);
        }

        add_Button.SetAsLastSibling();
    }
}

[System.Serializable]
public class EditorProjectCategoryModel
{
    [HideInInspector] public EditorProjectCategory controller;
    public void Init(EditorProjectCategory controller)
    {
        this.controller = controller;
    }
}
