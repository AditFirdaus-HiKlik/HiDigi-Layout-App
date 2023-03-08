using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LayoutInstanceData
{
    public string id = System.Guid.NewGuid().ToString();

    public InstanceData data = new InstanceData();
    public LayoutTransform transform = new LayoutTransform();
    public LayoutInstanceMetadata metadata = new LayoutInstanceMetadata();

    public Dictionary<string, object> ToDictionary()
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("id", id);
        dictionary.Add("data", data.ToDictionary());
        dictionary.Add("transform", transform.ToDictionary());
        dictionary.Add("metadata", metadata.ToDictionary());
        return dictionary;
    }

    public static LayoutInstanceData FromDictionary(Dictionary<string, object> dictionary)
    {
        var layoutInstanceData = new LayoutInstanceData();
        if (dictionary.ContainsKey("id")) layoutInstanceData.id = dictionary["id"].ToString();
        if (dictionary.ContainsKey("data")) layoutInstanceData.data = InstanceData.FromDictionary(dictionary["data"] as Dictionary<string, object>);
        if (dictionary.ContainsKey("transform")) layoutInstanceData.transform = LayoutTransform.FromDictionary(dictionary["transform"] as Dictionary<string, object>);
        if (dictionary.ContainsKey("metadata")) layoutInstanceData.metadata = LayoutInstanceMetadata.FromDictionary(dictionary["metadata"] as Dictionary<string, object>);
        return layoutInstanceData;
    }

}

[System.Serializable]
public class LayoutTransform
{
    public Vector3 position = new Vector3();
    public Vector3 rotation = new Vector3();
    public Vector3 scale = new Vector3(1, 1, 1);

    public Dictionary<string, object> ToDictionary()
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("position", position.Vector3ToDictionary());
        dictionary.Add("rotation", rotation.Vector3ToDictionary());
        dictionary.Add("scale", scale.Vector3ToDictionary());
        return dictionary;
    }

    public static LayoutTransform FromDictionary(Dictionary<string, object> dictionary)
    {
        var layoutTransform = new LayoutTransform();
        layoutTransform.position = (dictionary["position"] as Dictionary<string, object>).DictionaryToVector3();
        layoutTransform.rotation = (dictionary["rotation"] as Dictionary<string, object>).DictionaryToVector3();
        layoutTransform.scale = (dictionary["scale"] as Dictionary<string, object>).DictionaryToVector3();
        return layoutTransform;
    }
}

[System.Serializable]
public class LayoutInstanceMetadata
{
    public string title = "";
    public string description = "";

    public Dictionary<string, object> ToDictionary()
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("title", title);
        dictionary.Add("description", description);
        return dictionary;
    }

    public static LayoutInstanceMetadata FromDictionary(Dictionary<string, object> dictionary)
    {
        var layoutInstanceMetadata = new LayoutInstanceMetadata();
        if (dictionary.ContainsKey("title")) layoutInstanceMetadata.title = dictionary["title"].ToString();
        if (dictionary.ContainsKey("description")) layoutInstanceMetadata.description = dictionary["description"].ToString();
        return layoutInstanceMetadata;
    }
}

public static class Vector3DictionaryConverter
{
    public static Dictionary<string, float> Vector3ToDictionary(this Vector3 vector)
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        dict.Add("x", vector.x);
        dict.Add("y", vector.y);
        dict.Add("z", vector.z);
        return dict;
    }

    public static Vector3 DictionaryToVector3(this Dictionary<string, object> dict)
    {
        float x = 0;
        float y = 0;
        float z = 0;

        if (dict.ContainsKey("x")) x = float.Parse(dict["x"].ToString());
        if (dict.ContainsKey("y")) y = float.Parse(dict["y"].ToString());
        if (dict.ContainsKey("z")) z = float.Parse(dict["z"].ToString());

        return new Vector3(x, y, z);
    }
}
