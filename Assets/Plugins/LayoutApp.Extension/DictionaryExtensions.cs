using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public static class DictionaryUtillity
{
    public static Dictionary<string, object> ClassToDict(object obj)
    {
        var dict = new Dictionary<string, object>();
        var fields = obj.GetType().GetFields();
        foreach (var field in fields)
        {
            // if type of list
            if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var list = field.GetValue(obj) as IList<object>;
                var listDict = new List<Dictionary<string, object>>();
                foreach (var item in list)
                {
                    listDict.Add(ClassToDict(item));
                }
                dict[field.Name] = listDict;
            }
            else
            {
                dict[field.Name] = field.GetValue(obj);
            }
        }
        return dict;
    }

    public static T DictToClass<T>(Dictionary<string, object> dict) where T : new()
    {
        var obj = new T();
        var fields = typeof(T).GetFields();
        foreach (var field in fields)
        {
            if (dict.TryGetValue(field.Name, out object value))
            {
                if (value is Dictionary<string, object> nestedDict)
                {
                    field.SetValue(obj, DictToClass(field.FieldType, nestedDict));
                }
                else
                {
                    field.SetValue(obj, value);
                }
            }
        }
        return obj;
    }

    private static object DictToClass(Type type, Dictionary<string, object> dict)
    {
        var obj = Activator.CreateInstance(type);
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (dict.TryGetValue(field.Name, out object value))
            {
                if (value is Dictionary<string, object> nestedDict)
                {
                    field.SetValue(obj, DictToClass(field.FieldType, nestedDict));
                }
                else
                {
                    field.SetValue(obj, value);
                }
            }
        }
        return obj;
    }
}