using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Firestore;

[System.Serializable]
public class LayoutData
{
    public string id = "";
    public string name = "";

    public LayoutDataProject project = new LayoutDataProject();
    public List<LayoutInstanceData> instances = new List<LayoutInstanceData>();

    public Dictionary<string, object> ToDictionary()
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("id", id);
        dictionary.Add("name", name);
        dictionary.Add("project", project.ToDictionary());
        dictionary.Add("instances", instances.ConvertAll(instance => instance.ToDictionary()));
        return dictionary;
    }

    public static LayoutData FromDictionary(Dictionary<string, object> dictionary)
    {
        var layoutData = new LayoutData();
        if (dictionary.ContainsKey("id")) layoutData.id = dictionary["id"].ToString();
        if (dictionary.ContainsKey("name")) layoutData.name = dictionary["name"].ToString();
        if (dictionary.ContainsKey("project")) layoutData.project = LayoutDataProject.FromDictionary(dictionary["project"] as Dictionary<string, object>);
        if (dictionary.ContainsKey("instances")) layoutData.instances = (dictionary["instances"] as List<object>).ConvertAll(instance => LayoutInstanceData.FromDictionary(instance as Dictionary<string, object>));
        return layoutData;
    }
}

[System.Serializable]
public class LayoutDataProject
{
    public List<CategoryData> categories = new List<CategoryData>();

    public CategoryData GetCategory(string id)
    {
        foreach (var category in categories)
        {
            if (category.id == id) return category;
        }
        return null;
    }

    public InstanceData GetInstance(string id)
    {
        InstanceData instanceData = null;

        categories.ForEach((System.Action<CategoryData>)(category =>
        {
            instanceData = category.GetInstances().Find((System.Predicate<InstanceData>)(instance => instance.id == id));
        }));

        return instanceData;
    }

    public List<InstanceData> GetInstances(string category)
    {
        var instanceDatas = new List<InstanceData>();

        CategoryData categoryData = GetCategory(category);

        if (categoryData != null) instanceDatas.AddRange(categoryData.GetInstances());

        return instanceDatas;
    }

    public List<InstanceData> GetInstances()
    {
        var instanceDatas = new List<InstanceData>();

        categories.ForEach(category => instanceDatas.AddRange(category.GetInstances()));

        return instanceDatas;
    }


    public Dictionary<string, object> ToDictionary()
    {
        var dictionary = new Dictionary<string, object>();

        dictionary.Add("categories", categories.ConvertAll(category => category.ToDictionary()));

        return dictionary;
    }

    public static LayoutDataProject FromDictionary(Dictionary<string, object> dictionary)
    {
        var layoutDataProject = new LayoutDataProject();

        if (dictionary.ContainsKey("categories")) layoutDataProject.categories = (dictionary["categories"] as List<object>).ConvertAll(category => CategoryData.FromDictionary(category as Dictionary<string, object>));

        return layoutDataProject;
    }
}