using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstanceData
{
    public string id = Guid.NewGuid().ToString();
    public string name = "";
    public string category = "";
    public InstanceDataPreview preview = new InstanceDataPreview();
    public InstanceDataModel model = new InstanceDataModel();

    public Dictionary<string, object> ToDictionary()
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("id", id);
        dictionary.Add("name", name);
        dictionary.Add("preview", preview.ToDictionary());
        dictionary.Add("model", model.ToDictionary());
        return dictionary;
    }

    public static InstanceData FromDictionary(Dictionary<string, object> dictionary)
    {
        var instanceData = new InstanceData();
        if (dictionary.ContainsKey("id")) instanceData.id = dictionary["id"].ToString();
        if (dictionary.ContainsKey("name")) instanceData.name = dictionary["name"].ToString();
        if (dictionary.ContainsKey("preview")) instanceData.preview = InstanceDataPreview.FromDictionary(dictionary["preview"] as Dictionary<string, object>);
        if (dictionary.ContainsKey("model")) instanceData.model = InstanceDataModel.FromDictionary(dictionary["model"] as Dictionary<string, object>);
        return instanceData;
    }
}

[System.Serializable]
public class InstanceDataPreview
{
    public string url = "";
    public string fileName = "";
    public string fileExtension = "";

    public Dictionary<string, object> ToDictionary()
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("url", url);
        dictionary.Add("fileName", fileName);
        dictionary.Add("fileExtension", fileExtension);
        return dictionary;
    }

    public static InstanceDataPreview FromDictionary(Dictionary<string, object> dictionary)
    {
        var instanceDataPreview = new InstanceDataPreview();
        if (dictionary.ContainsKey("url")) instanceDataPreview.url = dictionary["url"].ToString();
        if (dictionary.ContainsKey("fileName")) instanceDataPreview.fileName = dictionary["fileName"].ToString();
        if (dictionary.ContainsKey("fileExtension")) instanceDataPreview.fileExtension = dictionary["fileExtension"].ToString();
        return instanceDataPreview;
    }
}

[System.Serializable]
public class InstanceDataModel
{
    public string url = "";
    public string fileName = "";
    public string fileExtension = "";

    public Dictionary<string, object> ToDictionary()
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("url", url);
        dictionary.Add("fileName", fileName);
        dictionary.Add("fileExtension", fileExtension);
        return dictionary;
    }

    public static InstanceDataModel FromDictionary(Dictionary<string, object> dictionary)
    {
        var instanceDataModel = new InstanceDataModel();
        if (dictionary.ContainsKey("url")) instanceDataModel.url = dictionary["url"].ToString();
        if (dictionary.ContainsKey("fileName")) instanceDataModel.fileName = dictionary["fileName"].ToString();
        if (dictionary.ContainsKey("fileExtension")) instanceDataModel.fileExtension = dictionary["fileExtension"].ToString();
        return instanceDataModel;
    }
}

public class InstanceDataTransform
{
    public Vector3 position = Vector3.zero;
    public Vector3 rotation = Vector3.zero;
    public Vector3 scale = Vector3.one;

    public InstanceDataTransform() { }

    public Dictionary<string, object> ToDictionary()
    {
        var dictionary = new Dictionary<string, object>();
        dictionary.Add("position", position);
        dictionary.Add("rotation", rotation);
        dictionary.Add("scale", scale);
        return dictionary;
    }

    public static InstanceDataTransform FromDictionary(Dictionary<string, object> dictionary)
    {
        var instanceDataTransform = new InstanceDataTransform();
        if (dictionary.ContainsKey("position")) instanceDataTransform.position = (Vector3)dictionary["position"];
        if (dictionary.ContainsKey("rotation")) instanceDataTransform.rotation = (Vector3)dictionary["rotation"];
        if (dictionary.ContainsKey("scale")) instanceDataTransform.scale = (Vector3)dictionary["scale"];
        return instanceDataTransform;
    }
}