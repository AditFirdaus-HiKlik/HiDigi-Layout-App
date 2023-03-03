using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public static class JsonExtensions
{
    public static T JsonToObject<T>(this string json)
    {
        return JsonUtility.FromJson<T>(json);
    }

    public static string ObjectToJson(this object obj)
    {
        return JsonUtility.ToJson(obj, true);
    }

    public static string DictionaryToJson(this Dictionary<string, object> dictionary)
    {
        return JsonConvert.SerializeObject(dictionary);
    }
}
