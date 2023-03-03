using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CategoryData
{
    public string id = Guid.NewGuid().ToString();
    public string name = "";
    public string icon = "";
    public List<InstanceData> instances = new List<InstanceData>();

    public List<InstanceData> GetInstances()
    {
        instances.ForEach((instance) => instance.category = id);

        return instances;
    }

    public Dictionary<string, object> ToDictionary()
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("id", id);
        dictionary.Add("name", name);
        dictionary.Add("icon", icon);
        dictionary.Add("instances", instances.ConvertAll(category => category.ToDictionary()));
        return dictionary;
    }

    public static CategoryData FromDictionary(Dictionary<string, object> dictionary)
    {
        var categoryData = new CategoryData();
        if (dictionary.ContainsKey("id")) categoryData.id = dictionary["id"].ToString();
        if (dictionary.ContainsKey("name")) categoryData.name = dictionary["name"].ToString();
        if (dictionary.ContainsKey("icon")) categoryData.icon = dictionary["icon"].ToString();
        if (dictionary.ContainsKey("instances")) categoryData.instances = (dictionary["instances"] as List<object>).ConvertAll(instance => InstanceData.FromDictionary(instance as Dictionary<string, object>));
        return categoryData;
    }
}
