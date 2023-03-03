using Newtonsoft.Json;
using System.Collections.Generic;

using UnityEngine;

public static class DictionaryExtensions
{
    public static T DictionaryToObject<T>(this Dictionary<string, object> dictionary)
    {
        Debug.Log(dictionary.ObjectToJson());
        var json = dictionary.ObjectToJson();
        var obj = JsonConvert.DeserializeObject<T>(json);
        return obj;
    }


    public static Dictionary<string, object> ObjectToDictionary(this object obj)
    {
        var json = obj.ObjectToJson();
        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        return dictionary;
    }
}